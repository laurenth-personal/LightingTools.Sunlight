using UnityEditor;
using LightUtilities.Sun;

namespace EditorLightUtilities.Sun
{
    [CustomEditor(typeof(SunlightClip))]
    public class SunlightClipPlayableEditor : Editor
    {
        SerializedProperty sunlightParameters;
        SerializedProperty overrideYAxis;
        SerializedProperty overrideLattitude;
        SerializedProperty overrideTimeOfDay;
        SerializedProperty overrideIntensity;
        SerializedProperty overrideColor;

        void OnEnable()
        {
            sunlightParameters = serializedObject.FindProperty("sunlightParameters");
            overrideYAxis = serializedObject.FindProperty("overrideYAxis");
            overrideLattitude = serializedObject.FindProperty("overrideLattitude");
            overrideTimeOfDay = serializedObject.FindProperty("overrideTimeOfDay");
            overrideIntensity = serializedObject.FindProperty("overrideIntensity");
            overrideColor = serializedObject.FindProperty("overrideColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            //EditorGUILayout.PropertyField(overrideYAxis);
            //EditorGUILayout.PropertyField(overrideLattitude);
            //EditorGUILayout.PropertyField(overrideTimeOfDay);
            //EditorGUILayout.PropertyField(overrideIntensity);
            //EditorGUILayout.PropertyField(overrideColor);
            //EditorGUILayout.PropertyField(sunlightParameters, true);

            /*
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField("Default Values", EditorStyles.boldLabel);

            

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