using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LightUtilities.Sun
{
    [System.Serializable]
    public class SunlightClipPlayable : PlayableBehaviour
    {
        public SunlightParameters sunlightParameters;
        [HideInInspector]

        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
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
