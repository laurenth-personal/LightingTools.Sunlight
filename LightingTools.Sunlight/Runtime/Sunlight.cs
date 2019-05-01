using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace LightUtilities.Sun
{
    [ExecuteInEditMode]
    public class Sunlight : MonoBehaviour
    {
        public SunlightParameters sunlightParameters;
        [SerializeField][HideInInspector]
        private GameObject sunlight;
        [SerializeField][HideInInspector]
        private GameObject sunlightLattitude;
        [SerializeField][HideInInspector]
        private GameObject sunlightYAxis;
        [SerializeField][HideInInspector]
        private GameObject sunlightTimeofdayDummy;
        public bool drawGizmo = true;
        public float gizmoSize = 5;
        public bool showEntities = true;
        private SunlightOrientationParameters modifiedOrientationParameters;

        [SerializeField]
        [HideInInspector]
        private Light directionalLight;
        [SerializeField]
        [HideInInspector]
        private HDAdditionalLightData additionalLightData;
        [SerializeField]
        [HideInInspector]
        private AdditionalShadowData shadowData;

        private VolumeStack stack;

        private void OnEnable()
        {
            CreateLightYAxis();
            CreateLightLattitude();
            CreateSunlightTimeofdayDymmy();
            CreateSunlight();
            //Enable if it has been disabled
            if (sunlight != null) { sunlight.GetComponent<Light>().enabled = true; }

            stack = VolumeManager.instance.stack;
        }

        private void OnDisable()
        {
            if (sunlight != null) { sunlight.GetComponent<Light>().enabled = false; }
        }

	    void Update ()
        {
            GatherOverrides();

            SetSunlightTransform();
            SetLightSettings();
            ApplyShowFlags(showEntities);
        }

        private void GatherOverrides()
        {
            modifiedOrientationParameters = SunlightOrientationParameters.DeepCopy(sunlightParameters.orientationParameters);

            if (stack == null)
                return;

            var sunProps = stack.GetComponent<SunlightProperties>();

            if (sunlightParameters.lightParameters == null || modifiedOrientationParameters == null )
                return;

            if (sunProps.lattitude.overrideState)
                modifiedOrientationParameters.lattitude = sunProps.lattitude.value;
            if (sunProps.YAxis.overrideState)
                modifiedOrientationParameters.yAxis = sunProps.YAxis.value;
            if (sunProps.timeOfDay.overrideState)
                modifiedOrientationParameters.timeOfDay = sunProps.timeOfDay.value;

            //If overridden intensity is constant, otherwise drive by curve
            if (sunProps.intensity.overrideState)
                sunlightParameters.lightParameters.intensity = sunProps.intensity.value;
            else if (sunlightParameters.intensityCurve != null)
                sunlightParameters.lightParameters.intensity = sunlightParameters.intensityCurve.Evaluate(sunlightParameters.orientationParameters.timeOfDay);
            if (sunlightParameters.intensityCurve == null)
                Debug.LogWarning("Sun intensity curve is null");

            //If overridden intensity is constant, otherwise driven by gradient
            if (sunProps.color.overrideState)
                sunlightParameters.lightParameters.colorFilter = sunProps.color.value;
            else if(sunlightParameters.colorGradient != null)
                sunlightParameters.lightParameters.colorFilter = sunlightParameters.colorGradient.Evaluate(sunlightParameters.orientationParameters.timeOfDay/24);

            if (sunProps.indirectMultiplier.overrideState)
                sunlightParameters.lightParameters.indirectIntensity = sunProps.indirectMultiplier.value;
            if (sunProps.cookieTexture.overrideState)
                sunlightParameters.lightParameters.lightCookie = sunProps.cookieTexture.value;
            if (sunProps.cookieSize.overrideState)
                sunlightParameters.lightParameters.cookieSize = sunProps.cookieSize.value;
            if (sunProps.shadowResolution.overrideState)
                sunlightParameters.lightParameters.shadowResolution = sunProps.shadowResolution.value;
        }

        public void SetLightSettings()
        {
            LightingUtilities.ApplyLightParameters(directionalLight, sunlightParameters.lightParameters);
        }


        void CreateLightYAxis()
        {
            if(sunlightYAxis == null)
                sunlightYAxis = new GameObject("SunlightYAxis");
            sunlightYAxis.transform.parent = gameObject.transform;
            sunlightYAxis.transform.localPosition = Vector3.zero;
            sunlightYAxis.transform.rotation = Quaternion.identity;
        }

        void CreateLightLattitude()
        {
            if (sunlightLattitude == null)
                sunlightLattitude = new GameObject("SunlightLattitude");
            sunlightLattitude.transform.parent = sunlightYAxis.transform;
            sunlightLattitude.transform.localPosition = Vector3.zero;
            sunlightLattitude.transform.localRotation = Quaternion.identity;
        }

        void CreateSunlightTimeofdayDymmy()
        {
            if (sunlightTimeofdayDummy == null)
                sunlightTimeofdayDummy = new GameObject("SunlightTimeofdayDummy");
            sunlightTimeofdayDummy.transform.parent = sunlightLattitude.transform;
            sunlightTimeofdayDummy.transform.localPosition = Vector3.zero;
            sunlightTimeofdayDummy.transform.localRotation = Quaternion.identity;
        }

        void CreateSunlight()
        {

            if (sunlight == null)
            {
                sunlight = new GameObject("DirectionalLight");
                //Init defaults
                sunlightParameters = new SunlightParameters();
                sunlightParameters.lightParameters.type = LightType.Directional;
            }

            sunlight.transform.parent = sunlightTimeofdayDummy.transform;
            sunlight.transform.localPosition = -Vector3.forward * gizmoSize;

            var lightComponent = sunlight.GetComponent<Light>();
            directionalLight =  lightComponent == null ? sunlight.AddComponent<Light>() : lightComponent;
            var additionalDataComponent = sunlight.GetComponent<HDAdditionalLightData>();
            var additionalLightData = additionalDataComponent == null ? sunlight.AddComponent<HDAdditionalLightData>() : additionalDataComponent;
            var shadowComponent = sunlight.GetComponent<AdditionalShadowData>();
            var shadowData = shadowComponent == null ? sunlight.AddComponent<AdditionalShadowData>() : shadowComponent;

            directionalLight.type = LightType.Directional;
        }

        public void SetSunlightTransform()
        {
            SetSunlightTransform(modifiedOrientationParameters.timeOfDay);
        }

        void SetSunlightTransform(float timeOfDay)
        {
            if (sunlightYAxis != null && sunlightLattitude != null && sunlight != null && sunlightParameters != null && sunlightYAxis.transform.parent == gameObject.transform)
            {
                sunlightYAxis.transform.rotation = Quaternion.Euler(new Vector3(0, modifiedOrientationParameters.yAxis, 0));
                sunlightLattitude.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180 - modifiedOrientationParameters.lattitude));
                sunlightTimeofdayDummy.transform.localRotation = Quaternion.Euler(new Vector3(timeOfDay * 15f + 90, 0, 0));
                sunlight.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, modifiedOrientationParameters.roll));
            }
        }

        private void OnDestroy()
        {
            DestroyImmediate(sunlight);
            DestroyImmediate(sunlightYAxis);
            DestroyImmediate(sunlightLattitude);
            DestroyImmediate(sunlightTimeofdayDummy);
        }

        void ApplyShowFlags(bool show)
        {
            if (sunlight != null)
            {
                if (!show) { sunlight.hideFlags = HideFlags.HideInHierarchy; }
                if (show)
                {
                    sunlight.hideFlags = HideFlags.None;
                }
            }
            if (sunlightYAxis != null)
            {
                if (!show) { sunlightYAxis.hideFlags = HideFlags.HideInHierarchy; }
                if (show)
                {
                    sunlightYAxis.hideFlags = HideFlags.None;
                }
            }
            if (sunlightLattitude != null)
            {
                if (!show) { sunlightLattitude.hideFlags = HideFlags.HideInHierarchy; }
                if (show)
                {
                    sunlightLattitude.hideFlags = HideFlags.None;
                }
            }
        }
    }
}
