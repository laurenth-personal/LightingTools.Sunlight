using UnityEditor;
using LightUtilities.Sun;

namespace EditorLightUtilities.Sun
{
    [CustomEditor(typeof(SunlightClip))]
    public class SunlightClipPlayableEditor : Editor
    {
        //public Sunlight sunlight;
        //SerializedProperty sunlightParameters;
        //SerializedProperty drawGizmo;
        //SerializedProperty gizmoSize;

        void OnEnable()
        {
            /*
            sunlight = (Sunlight)serializedObject.targetObject;
            sunlightParameters = serializedObject.FindProperty("sunlightParameters");
            //intensityCurve = serializedObject.FindProperty("sunlightParameters.intensityCurve");
            //gradient = serializedObject.FindProperty("sunlightParameters.colorGradient");
            drawGizmo = serializedObject.FindProperty("drawGizmo");
            gizmoSize = serializedObject.FindProperty("gizmoSize");
            */
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            /*
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField("Default Values", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(sunlightParameters, true);

            LightUIUtilities.DrawSplitter();
            LightUIUtilities.DrawHeader("Visualization");
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(drawGizmo);
            EditorGUILayout.PropertyField(gizmoSize);
            */
            serializedObject.ApplyModifiedProperties();
        }
    }
}