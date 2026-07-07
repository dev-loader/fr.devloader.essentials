/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260116

using UnityEditor;
#if UNITY_UI_TOOLKIT
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#else
using UnityEngine;
#endif

namespace Devloader.InspectorProperty
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
#if UNITY_UI_TOOLKIT
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField propertyRoot = new PropertyField(property);
            propertyRoot.SetEnabled(false);
            return propertyRoot;
        }
#else
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
#endif
    }
}