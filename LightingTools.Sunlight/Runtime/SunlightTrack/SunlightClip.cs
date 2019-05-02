using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LightUtilities.Sun
{
    [System.Serializable]
    public class SunlightClipPlayable : PlayableBehaviour
    {
        public bool overrideYAxis = false;
        public bool overrideLattitude = false;
        public bool overrideTimeOfDay = false;
        public SunlightOrientationParameters orientationParameters;
        public bool overrideIntensity = false;
        public float intensity = 1000;
        public bool overrideColor = false;
        public Color color;


        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
        }

        public override void OnGraphStop(Playable playable)
        {
            overrideIntensity = false;
            base.OnGraphStop(playable);
        }
    }

    [System.Serializable]
    public class SunlightClip : PlayableAsset, ITimelineClipAsset {

        public SunlightClipPlayable sunlightClip = new SunlightClipPlayable();

        // Create the runtime version of the clip, by creating a copy of the template
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
            return ScriptPlayable<SunlightClipPlayable>.Create(graph, sunlightClip);
        }

        // Use this to tell the Timeline Editor what features this clip supports
        public ClipCaps clipCaps {
            get { return ClipCaps.Blending | ClipCaps.Extrapolation; }
        }
    }
}
