using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Experimental.Rendering;

namespace LightUtilities.Sun
{
    [TrackColor(1.0f, 0.96f, 0.85f)]
    [TrackBindingType(typeof(Volume))]
    [TrackClipType(typeof(SunlightClip))]
    public class SunlightTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
            return ScriptPlayable<SunlightMixer>.Create(graph, inputCount);
        }
    }
}
