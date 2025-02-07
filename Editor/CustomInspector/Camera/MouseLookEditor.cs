/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240620

using UnityEditor;
using UnityEngine;

namespace Devloader.CustomInspector.CameraManagement
{
    [CustomEditor(typeof(MouseLook))]
    public class MouseLookEditor : Editor
    {
        SerializedProperty axis;

#if ENABLE_LEGACY_INPUT_MANAGER
        SerializedProperty xAxisInputName;
        SerializedProperty yAxisInputName;
#else
        SerializedProperty deltaInputValue;
#endif

        SerializedProperty sensitivityX;
        SerializedProperty sensitivityY;

        SerializedProperty minimumX;
        SerializedProperty maximumX;

        SerializedProperty minimumY;
        SerializedProperty maximumY;

        protected virtual void OnEnable()
        {
            axis = serializedObject.FindProperty("axis");

#if ENABLE_LEGACY_INPUT_MANAGER
            xAxisInputName = serializedObject.FindProperty("xAxisInputName");
            yAxisInputName = serializedObject.FindProperty("yAxisInputName");
#else
            deltaInputValue = serializedObject.FindProperty("deltaInputValue");
#endif

            sensitivityX = serializedObject.FindProperty("sensitivityX");
            sensitivityY = serializedObject.FindProperty("sensitivityY");

            minimumX = serializedObject.FindProperty("minimumX");
            maximumX = serializedObject.FindProperty("maximumX");

            minimumY = serializedObject.FindProperty("minimumY");
            maximumY = serializedObject.FindProperty("maximumY");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Base settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            /// Verrouillage du curseur
            MouseLook.isMouseLock = EditorGUILayout.Toggle("Is Mouse Lock", MouseLook.isMouseLock);

            /// Axes de rotation
            EditorGUILayout.PropertyField(axis, new GUIContent("Rotation axis"));
            EditorGUILayout.Space();

#if ENABLE_LEGACY_INPUT_MANAGER
            EditorGUILayout.PropertyField(xAxisInputName, new GUIContent("X axis input name"));
            EditorGUILayout.PropertyField(yAxisInputName, new GUIContent("Y axis input name"));
#else
            EditorGUILayout.PropertyField(deltaInputValue, new GUIContent("Delta input value"));
#endif
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            /// Réglage rotation horizontale
            EditorGUILayout.LabelField("Sensitivity settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(sensitivityX, new GUIContent("X (horizontal axis)"));
            EditorGUILayout.PropertyField(sensitivityY, new GUIContent("Y (vertical axis)"));

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            /// Réglage rotation verticale
            EditorGUILayout.LabelField("Vertical settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(minimumY, new GUIContent("Minimum angle"));
            EditorGUILayout.PropertyField(maximumY, new GUIContent("Maximum angle"));

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            /// Debug
            EditorGUILayout.HelpBox("Is Mouse Lock: " + MouseLook.isMouseLock + "\nRotation Y: " + (serializedObject.targetObject as MouseLook).YRotation, MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }
    }
}