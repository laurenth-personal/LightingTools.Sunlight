using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using LightUtilities.Sun;

public class SunlightMixer : PlayableBehaviour
{
    // Called each frame the mixer is active, after inputs are processed
    public override void ProcessFrame(Playable handle, FrameData info, object playerData) {

        Volume volume = playerData as Volume;
        VolumeProfile volumeProfile = Application.isPlaying ? volume.profile : volume.sharedProfile;
        SunlightProperties sunprops = ScriptableObject.CreateInstance<SunlightProperties>();

        SunlightParameters neutralSunlightParameters = new SunlightParameters(true);
        SunlightParameters mixedSunlightParameters = new SunlightParameters(true);

        if(volumeProfile.TryGet<SunlightProperties>(out sunprops))
        {
            var count = handle.GetInputCount();
            for (var i = 0; i < count; i++)
            {
                var inputHandle = handle.GetInput(i);
                var weight = handle.GetInputWeight(i);

                if (inputHandle.IsValid() &&
                    inputHandle.GetPlayState() == PlayState.Playing &&
                    weight > 0)
                {
                    var data = ((ScriptPlayable<SunlightClipPlayable>)inputHandle).GetBehaviour();
                    if (data != null)
                    {
                        var weightedSunlightParameters = SunlightLightingUtilities.LerpSunlightParameters(neutralSunlightParameters, data.sunlightParameters, weight);

                        mixedSunlightParameters += weightedSunlightParameters;
                    }
                }
            }

            sunprops.YAxis.value = mixedSunlightParameters.orientationParameters.yAxis;
            sunprops.lattitude.value = mixedSunlightParameters.orientationParameters.lattitude;
            sunprops.timeOfDay.value = mixedSunlightParameters.orientationParameters.timeOfDay;
            sunprops.intensity.value = mixedSunlightParameters.lightParameters.intensity;
            sunprops.color.value = mixedSunlightParameters.lightParameters.colorFilter;
            sunprops.cookieTexture.value = mixedSunlightParameters.lightParameters.lightCookie;
            sunprops.cookieSize.value = mixedSunlightParameters.lightParameters.cookieSize;
        }
    }
}
