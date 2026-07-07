/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

using UnityEditor;
using UnityEngine;

namespace Devloader.Maths.CustomInspector
{
    [CustomPropertyDrawer(typeof(RangedInt))]
    public class RangedIntPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty aProp;
        private SerializedProperty bProp;
        private SerializedProperty currentValueProp;

        private int indentLevel = 0;
        private float minMaxWidth, currentWidth;
        private float labelHeight, fieldHeight;
        private Rect minLabelRect, currentLabelRect, maxLabelRect;
        private Rect minValueRect, currentValueRect, maxValueRect;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            aProp = property.FindPropertyRelative("_aBound");
            bProp = property.FindPropertyRelative("_bBound");
            currentValueProp = property.FindPropertyRelative("_currentValue");

            EditorGUI.BeginProperty(position, label, property);

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(
                new Rect(position.position, new Vector2(position.width, EditorGUIUtility.singleLineHeight)),
                property.isExpanded,
                label + " [" + aProp.intValue + ".." + bProp.intValue + "]"
            );

            indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel + 1;

            if (property.isExpanded)
            {
                EvaluatePropertyRects(position);
                bool aGreaterThanB = aProp.intValue > bProp.intValue;
                ShowProperties(aGreaterThanB);
            }

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndFoldoutHeaderGroup();

            currentValueProp.intValue = Mathf.Clamp(
                currentValueProp.intValue,
                Mathf.Min(aProp.intValue, bProp.intValue),
                Mathf.Max(aProp.intValue, bProp.intValue)
            );

            EditorGUI.EndProperty();
        }

        private void EvaluatePropertyRects(Rect position)
        {
            minMaxWidth = position.width / 5;
            currentWidth = position.width - 2 * minMaxWidth;
            labelHeight = EditorGUIUtility.singleLineHeight;
            fieldHeight = EditorGUIUtility.singleLineHeight;

            minLabelRect = new Rect(position.x, position.y + labelHeight, minMaxWidth, labelHeight);
            currentLabelRect = new Rect(minLabelRect.x + minMaxWidth, minLabelRect.y, currentWidth, labelHeight);
            maxLabelRect = new Rect(currentLabelRect.x + currentWidth, minLabelRect.y, minMaxWidth, labelHeight);

            minValueRect = new Rect(minLabelRect.x, minLabelRect.y + labelHeight + EditorGUIUtility.standardVerticalSpacing, minMaxWidth, fieldHeight);
            currentValueRect = new Rect(minLabelRect.x + minMaxWidth, minValueRect.y, currentWidth, fieldHeight);
            maxValueRect = new Rect(currentValueRect.x + currentWidth, minValueRect.y, minMaxWidth, fieldHeight);
        }

        private void ShowProperties(bool aGreaterThanB)
        {
            EditorGUI.LabelField(aGreaterThanB ? maxLabelRect : minLabelRect, new GUIContent("A"));
            EditorGUI.LabelField(!aGreaterThanB ? maxLabelRect : minLabelRect, new GUIContent("B"));
            EditorGUI.LabelField(currentLabelRect, new GUIContent("Current Value"));

            if (aGreaterThanB)
            {
                EditorGUI.PropertyField(minValueRect, bProp, GUIContent.none);
                EditorGUI.PropertyField(maxValueRect, aProp, GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(minValueRect, aProp, GUIContent.none);
                EditorGUI.PropertyField(maxValueRect, bProp, GUIContent.none);
            }

            currentValueProp.intValue = EditorGUI.IntSlider(
                currentValueRect,
                currentValueProp.intValue,
                Mathf.Min(aProp.intValue, bProp.intValue),
                Mathf.Max(aProp.intValue, bProp.intValue)
            );
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;
            if (property.isExpanded)
                totalLines += 2;
            return EditorGUIUtility.singleLineHeight * totalLines + EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
        }
    }
}
