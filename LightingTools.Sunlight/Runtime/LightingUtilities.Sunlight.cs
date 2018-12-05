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

        public SunlightOrientationParameters orientationParameters = new SunlightOrientationParameters();
        public AnimationCurve intensityCurve;
        public Gradient colorGradient;
        public LightParameters lightParameters = new LightParameters();
        //public ProceduralSkyboxParameters proceduralSkyParameters = new ProceduralSkyboxParameters();
    }

    //[System.Serializable]
    //public class ProceduralSkyboxParameters
    //{
    //    [Range(0.01f,1)]
    //    public float sunSize = 0.05f;
    //    [Range(0.1f,4.5f)]
    //    public float atmosphereThickness = 1;
    //    public Color skyTint = Color.gray;
    //    public Color Ground = Color.gray;
    //    [Range(0,8)]
    //    public float exposure = 1.5f;
    //}

    public static class SunlightLightingUtilities
    {
        public static SunlightParameters LerpSunlightParameters(SunlightParameters from, SunlightParameters to, float weight)
        {
            var lerpSunlightParameters = new SunlightParameters();
            //Orientation
            lerpSunlightParameters.orientationParameters.lattitude = Mathf.Lerp(from.orientationParameters.lattitude, to.orientationParameters.lattitude, weight);
            lerpSunlightParameters.orientationParameters.yAxis = Mathf.Lerp(from.orientationParameters.yAxis, to.orientationParameters.yAxis, weight);
            lerpSunlightParameters.orientationParameters.timeOfDay = Mathf.Lerp(from.orientationParameters.timeOfDay, to.orientationParameters.timeOfDay, weight);
            lerpSunlightParameters.orientationParameters.roll = Mathf.Lerp(from.orientationParameters.roll, to.orientationParameters.roll, weight);

            lerpSunlightParameters.lightParameters = LightingUtilities.LerpLightParameters(from.lightParameters, to.lightParameters, weight);

            return lerpSunlightParameters;
        }

        //public static void SetProceduralSkyboxParameters(ProceduralSkyboxParameters skyParameters)
        //{
        //    var skyboxMaterial = RenderSettings.skybox;
        //    if(skyboxMaterial.shader.name == "Skybox/Procedural")
        //    {
        //        skyboxMaterial.SetFloat("_SunSize", skyParameters.sunSize);
        //        skyboxMaterial.SetFloat("_AtmosphereThickness", skyParameters.atmosphereThickness);
        //        skyboxMaterial.SetColor("_SkyTint", skyParameters.skyTint);
        //        skyboxMaterial.SetColor("_GroundColor", skyParameters.Ground);
        //        skyboxMaterial.SetFloat("_Exposure", skyParameters.exposure);
        //    }
        //    else
        //    {
        //        Debug.Log("Skybox material not using Procedural Skybox shader");
        //    }
        //}
    }
}