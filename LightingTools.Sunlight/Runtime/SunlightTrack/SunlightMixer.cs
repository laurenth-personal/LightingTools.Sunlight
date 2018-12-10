using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace LightUtilities.Sun
{
    public class SunlightMixer : PlayableBehaviour
    {
        // Called each frame the mixer is active, after inputs are processed
        public override void ProcessFrame(Playable handle, FrameData info, object playerData) {

            if (playerData == null)
                return;

            Volume volume = playerData as Volume;

            VolumeProfile volumeProfile = Application.isPlaying ? volume.profile : volume.sharedProfile;
            SunlightProperties sunprops = ScriptableObject.CreateInstance<SunlightProperties>();

            SunlightOrientationParameters neutralOrientationParameters = new SunlightOrientationParameters(true);
            SunlightOrientationParameters mixedOrientationParameters = new SunlightOrientationParameters(true);
            float overriddenIntensity = 0;
            Color overriddenColor = Color.black;

            bool overrideYAxis = false;
            bool overrideLattitude = false;
            bool overrideTimeOfDay = false;
            bool overrideIntensity = false;
            bool overrideColor = false;

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
                            var weightedSunlightParameters = SunlightLightingUtilities.LerpSunlightOrientationParameters(neutralOrientationParameters, data.orientationParameters, weight);
                            overriddenIntensity += data.intensity * weight;
                            overriddenColor += data.color * weight;

                            mixedOrientationParameters += weightedSunlightParameters;

                            if (data.overrideYAxis)
                                overrideYAxis = true;
                            if (data.overrideLattitude)
                                overrideLattitude = true;
                            if (data.overrideTimeOfDay)
                                overrideTimeOfDay = true;
                            if (data.overrideIntensity)
                                overrideIntensity = true;
                            if (data.overrideColor)
                                overrideColor = true;
                        }
                    }
                }
                sunprops.YAxis.overrideState = overrideYAxis;
                if(overrideYAxis)
                {
                    sunprops.YAxis.value = mixedOrientationParameters.yAxis;
                }
                sunprops.lattitude.overrideState = overrideLattitude;
                if (overrideLattitude)
                {
                    sunprops.lattitude.value = mixedOrientationParameters.lattitude;
                }
                sunprops.timeOfDay.overrideState = overrideTimeOfDay;
                if (overrideTimeOfDay)
                {
                    sunprops.timeOfDay.value = mixedOrientationParameters.timeOfDay;
                }
                sunprops.intensity.overrideState = overrideIntensity;
                if (overrideIntensity)
                {
                    sunprops.intensity.value = overriddenIntensity;
                }
                sunprops.color.overrideState = overrideColor;
                if (overrideColor)
                {
                    sunprops.color.value = overriddenColor;
                }
                //sunprops.cookieTexture.value = mixedSunlightParameters.lightParameters.lightCookie;
                //sunprops.cookieSize.value = mixedSunlightParameters.lightParameters.cookieSize;
            }
        }
    }
}
