using UnityEngine;
using UnityEditor;
using LightUtilities.Sun;

namespace EditorLightUtilities.Sun
{
    [CustomEditor(typeof(Sunlight))]
    public class SunlightEditor : Editor
    {
        [MenuItem("GameObject/Light/Sunlight", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject sunlight = new GameObject("Sunlight");
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(sunlight, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(sunlight, "Create " + sunlight.name);
            Selection.activeObject = sunlight;
            sunlight.AddComponent<Sunlight>();
        }

        public Sunlight sunlight;
        SerializedProperty sunlightParameters;
        SerializedProperty drawGizmo;
        SerializedProperty gizmoSize;

        void OnEnable()
        {
            sunlight = (Sunlight)serializedObject.targetObject;
            sunlightParameters = serializedObject.FindProperty("sunlightParameters");
            drawGizmo = serializedObject.FindProperty("drawGizmo");
            gizmoSize = serializedObject.FindProperty("gizmoSize");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField("Default Values", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(sunlightParameters, true);

            LightUIUtilities.DrawSplitter();
            LightUIUtilities.DrawHeader("Visualization");
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(drawGizmo);
            EditorGUILayout.PropertyField(gizmoSize);

            serializedObject.ApplyModifiedProperties();
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawSunlightGizmo(Sunlight scr, GizmoType gizmoType)
        {
            var childrentransforms = scr.GetComponentsInChildren<Transform>();
            GameObject sun = null ;
            GameObject yAxis = null ;
            GameObject lattitude = null ;
            GameObject timeOfDay = null ;

            for (int i = 0; i<childrentransforms.Length; i++)
            {
                if( childrentransforms[i].name == "DirectionalLight" ) sun = childrentransforms[i].gameObject;
                if( childrentransforms[i].name == "SunlightYAxis" ) yAxis = childrentransforms[i].gameObject;
                if( childrentransforms[i].name == "SunlightLattitude" ) lattitude = childrentransforms[i].gameObject;
                if( childrentransforms[i].name == "SunlightTimeofdayDummy" ) timeOfDay = childrentransforms[i].gameObject;
            }
            Gizmos.color = Handles.color = new Color(1, 1, 1, 0.3f);
            if(yAxis != null) Handles.DrawWireDisc(yAxis.transform.position, yAxis.transform.up, scr.gizmoSize);
            if(lattitude != null) Handles.DrawWireArc(scr.gameObject.transform.position, lattitude.transform.right, lattitude.transform.forward, 180, scr.gizmoSize);
            var gizmoColor = scr.sunlightParameters.lightParameters.colorFilter;
            Gizmos.color = Handles.color = gizmoColor;
            if(timeOfDay != null) Gizmos.DrawLine(timeOfDay.transform.position, sun.transform.position);
            Handles.DrawWireDisc(sun.transform.position, Camera.current.transform.forward, scr.gizmoSize / 10);
        }
    }
}
