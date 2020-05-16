using UnityEngine;
using UnityEngine.Rendering;

namespace LightUtilities.Sun
{
    [System.Serializable]
    public class SunlightProperties : VolumeComponent
    {
        public ClampedFloatParameter YAxis = new ClampedFloatParameter(0f, -180f, 180f);
        public ClampedFloatParameter lattitude = new ClampedFloatParameter(0f, -90f, 90f);
        public ClampedFloatParameter timeOfDay = new ClampedFloatParameter(10f, 0f, 24f);
        public FloatParameter intensity = new FloatParameter(1000);
        public FloatParameter indirectMultiplier = new FloatParameter(1);
        public ColorParameter color = new ColorParameter(Color.white);
        public TextureParameter cookieTexture = new TextureParameter(null);
        public FloatParameter cookieSize = new FloatParameter(1);
        public IntParameter shadowResolution = new IntParameter(1024);
        public ColorParameter shadowTint = new ColorParameter(Color.black);
        public ColorParameter penumbraTint = new ColorParameter(Color.white);
    }
}
