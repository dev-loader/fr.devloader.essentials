/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250218

using UnityEditor;
using UnityEngine;

namespace Devloader.Maths.CustomInspector
{
    [CustomPropertyDrawer(typeof(ClampRange)), System.Obsolete]
    public class ClampRangePropertyDrawer : PropertyDrawer
    {
        SerializedProperty min;
        SerializedProperty max;
        SerializedProperty current;

        int indentLevel = 0;


        float minMaxWidth, currentWidth;
        float labelHeight, fieldHeight;

        Rect minLabelRect, currentLabelRect, maxLabelRect, minValueRect, currentValueRect, maxValueRect;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            min = property.serializedObject.FindProperty(property.name + ".min");
            max = property.serializedObject.FindProperty(property.name + ".max");
            current = property.serializedObject.FindProperty(property.name + ".current");

            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(
                position.position,
                new Vector2(position.width, EditorGUIUtility.singleLineHeight)
            );

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, label);

            indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel + 1;

            if (property.isExpanded)
            {
                EvaluatePropertyRects(foldoutRect);
                ShowProperties();
            }

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndFoldoutHeaderGroup();

            EditorGUI.EndProperty();

            CheckMinMax(ref min, ref max);
            CheckCurrent(ref current, min.floatValue, max.floatValue);
        }

        private void CheckMinMax(ref SerializedProperty min, ref SerializedProperty max)
        {
            if (min.floatValue > max.floatValue)
                (min.floatValue, max.floatValue) = (max.floatValue, min.floatValue);
        }

        private void CheckCurrent(ref SerializedProperty current, float min, float max) => current.floatValue = Mathf.Clamp(current.floatValue, min, max);

        private void EvaluatePropertyRects(Rect position)
        {
            minMaxWidth = position.width / 5;
            currentWidth = position.width - 2 * position.width / 5;

            labelHeight = EditorGUIUtility.singleLineHeight;
            fieldHeight = EditorGUIUtility.singleLineHeight;

            minLabelRect = new Rect(
                new Vector2(position.x, position.y + labelHeight),
                new Vector2(minMaxWidth, labelHeight)
            );

            currentLabelRect = new Rect(
                new Vector2(minLabelRect.x + minMaxWidth, minLabelRect.y),
                new Vector2(currentWidth, labelHeight)
            );

            maxLabelRect = new Rect(
                new Vector2(currentLabelRect.x + currentWidth, minLabelRect.y),
                new Vector2(minMaxWidth, labelHeight)
            );


            minValueRect = new Rect(
                new Vector2(minLabelRect.x, minLabelRect.y + labelHeight + EditorGUIUtility.standardVerticalSpacing),
                new Vector2(minMaxWidth, fieldHeight)
            );

            currentValueRect = new Rect(
                new Vector2(minLabelRect.x + minMaxWidth, minValueRect.y),
                new Vector2(currentWidth, fieldHeight)
            );

            maxValueRect = new Rect(
                new Vector2(currentValueRect.x + currentWidth, minValueRect.y),
                new Vector2(minMaxWidth, fieldHeight)
            );
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
                totalLines += 2;

            return EditorGUIUtility.singleLineHeight * totalLines + EditorGUIUtility.standardVerticalSpacing * (totalLines-1);
        }

        private void ShowProperties()
        {
            EditorGUI.LabelField(minLabelRect, new GUIContent("Min"));
            EditorGUI.LabelField(currentLabelRect, new GUIContent("Current value"));
            EditorGUI.LabelField(maxLabelRect, new GUIContent("Max"));

            EditorGUI.PropertyField(minValueRect, min, GUIContent.none);
            current.floatValue = EditorGUI.Slider(currentValueRect, current.floatValue, min.floatValue, max.floatValue);
            EditorGUI.PropertyField(maxValueRect, max, GUIContent.none);
        }
    }
}