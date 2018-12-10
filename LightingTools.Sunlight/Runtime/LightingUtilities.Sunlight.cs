using UnityEngine;

namespace LightUtilities.Sun
{
    [System.Serializable]
    public class SunlightOrientationParameters
    {
        public SunlightOrientationParameters() { }

        public SunlightOrientationParameters(bool neutral)
        {
            yAxis = 0;
            timeOfDay = 0;
            lattitude = 0;
            roll = 0;
        }

        public static SunlightOrientationParameters operator +(SunlightOrientationParameters x, SunlightOrientationParameters y)
        {
            var addition = new SunlightOrientationParameters
            {
                yAxis = x.yAxis + y.yAxis,
                timeOfDay = x.timeOfDay + y.timeOfDay,
                lattitude = x.lattitude + y.lattitude,
                roll = x.roll + y.roll
            };
            return addition;
        }

        public static SunlightOrientationParameters DeepCopy(SunlightOrientationParameters c)
        {
            SunlightOrientationParameters temp = new SunlightOrientationParameters();
            temp.yAxis = c.yAxis;
            temp.lattitude = c.lattitude;
            temp.timeOfDay = c.timeOfDay;
            temp.roll = c.roll;
            return temp;
        }

        [Range(-180f, 180f)]
        public float yAxis = 0f;
        [Range(0f, 24f)]
        public float timeOfDay = 10f;
        [Range(-90f, 90f)]
        public float lattitude = 35f;
        [Range(-180f, 180f)]
        public float roll = 0.1f;
    }

    [System.Serializable]
    public class SunlightParameters
    {
        public SunlightParameters() { }

        public SunlightParameters(bool neutral)
        {
            orientationParameters = new SunlightOrientationParameters(true);
            lightParameters = new LightParameters(LightType.Directional,LightmapPresetBakeType.Mixed, true);
        }

        public static SunlightParameters operator +(SunlightParameters x, SunlightParameters y)
        {
            var addition = new SunlightParameters
            {
                lightParameters = x.lightParameters + y.lightParameters,
                orientationParameters = x.orientationParameters + y.orientationParameters
            };
            return addition;
        }

        public static SunlightParameters DeepCopy(SunlightParameters c)
        {
            SunlightParameters temp = new SunlightParameters();
            temp.orientationParameters = SunlightOrientationParameters.DeepCopy(c.orientationParameters);
            temp.lightParameters = LightParameters.DeepCopy(c.lightParameters);
            temp.colorGradient = c.colorGradient;
            temp.intensityCurve = c.intensityCurve;
            return temp;
        }

        public SunlightOrientationParameters orientationParameters = new SunlightOrientationParameters();
        public AnimationCurve intensityCurve;
        public Gradient colorGradient;
        public LightParameters lightParameters = new LightParameters();
    }

    public static class SunlightLightingUtilities
    {
        public static SunlightParameters LerpSunlightParameters(SunlightParameters from, SunlightParameters to, float weight)
        {
            var lerpSunlightParameters = new SunlightParameters();
            lerpSunlightParameters.orientationParameters = LerpSunlightOrientationParameters(from.orientationParameters, to.orientationParameters, weight);
            lerpSunlightParameters.lightParameters = LightingUtilities.LerpLightParameters(from.lightParameters, to.lightParameters, weight);
            return lerpSunlightParameters;
        }

        public static SunlightOrientationParameters LerpSunlightOrientationParameters(SunlightOrientationParameters from, SunlightOrientationParameters to, float weight)
        {
            var lerpSunlightOrientationParameters = new SunlightOrientationParameters();
            //Orientation
            lerpSunlightOrientationParameters.lattitude = Mathf.Lerp(from.lattitude, to.lattitude, weight);
            lerpSunlightOrientationParameters.yAxis = Mathf.Lerp(from.yAxis, to.yAxis, weight);
            lerpSunlightOrientationParameters.timeOfDay = Mathf.Lerp(from.timeOfDay, to.timeOfDay, weight);
            lerpSunlightOrientationParameters.roll = Mathf.Lerp(from.roll, to.roll, weight);

            return lerpSunlightOrientationParameters;
        }
    }
}