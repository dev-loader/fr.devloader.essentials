/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

using UnityEditor;
using UnityEngine;

namespace Devloader.Maths.CustomInspector
{
    [CustomPropertyDrawer(typeof(RangedDouble))]
    public class RangedDoublePropertyDrawer : PropertyDrawer
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

            double aVal = aProp.doubleValue;
            double bVal = bProp.doubleValue;
            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(
                new Rect(position.position, new Vector2(position.width, EditorGUIUtility.singleLineHeight)),
                property.isExpanded,
                label + " [" + aVal + ".." + bVal + "]"
            );

            indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel + 1;

            if (property.isExpanded)
            {
                EvaluatePropertyRects(position);
                bool aGreaterThanB = aVal > bVal;
                ShowProperties(aGreaterThanB);
            }

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndFoldoutHeaderGroup();

            // Clamp currentValue avec les bornes actuelles
            float minVal = System.Math.Min((float)aVal, (float)bVal);
            float maxVal = System.Math.Max((float)aVal, (float)bVal);
            currentValueProp.doubleValue = System.Math.Clamp((float)currentValueProp.doubleValue, minVal, maxVal);

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

            double aVal = aProp.doubleValue;
            double bVal = bProp.doubleValue;

            if (aGreaterThanB)
            {
                bProp.doubleValue = EditorGUI.DoubleField(minValueRect, GUIContent.none, bVal);
                aProp.doubleValue = EditorGUI.DoubleField(maxValueRect, GUIContent.none, aVal);
            }
            else
            {
                aProp.doubleValue = EditorGUI.DoubleField(minValueRect, GUIContent.none, aVal);
                bProp.doubleValue = EditorGUI.DoubleField(maxValueRect, GUIContent.none, bVal);
            }

            double minVal = System.Math.Min((float)aProp.doubleValue, (float)bProp.doubleValue);
            double maxVal = System.Math.Max((float)aProp.doubleValue, (float)bProp.doubleValue);
            currentValueProp.doubleValue = EditorGUI.DoubleField(
                currentValueRect,
                currentValueProp.doubleValue
            );
            currentValueProp.doubleValue = System.Math.Clamp((float)currentValueProp.doubleValue, (float)minVal, (float)maxVal);
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
