/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

using UnityEditor;
using UnityEngine;

namespace Devloader.Maths.CustomInspector
{
    [CustomPropertyDrawer(typeof(RangedFloat))]
    public class RangedFloatPropertyDrawer : PropertyDrawer
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
                label + " [" + aProp.floatValue + ".." + bProp.floatValue + "]"
            );

            indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel + 1;

            if (property.isExpanded)
            {
                EvaluatePropertyRects(position);

                // Détermine si A > B pour l'affichage
                bool aGreaterThanB = aProp.floatValue > bProp.floatValue;
                ShowProperties(aGreaterThanB);
            }

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndFoldoutHeaderGroup();

            // Clamp currentValue avec les bornes actuelles (même si A > B)
            CheckCurrent(ref currentValueProp, Mathf.Min(aProp.floatValue, bProp.floatValue), Mathf.Max(aProp.floatValue, bProp.floatValue));

            EditorGUI.EndProperty();
        }

        private void CheckCurrent(ref SerializedProperty current, float min, float max)
        {
            current.floatValue = Mathf.Clamp(current.floatValue, min, max);
        }

        private void EvaluatePropertyRects(Rect position)
        {
            minMaxWidth = position.width / 5;
            currentWidth = position.width - 2 * minMaxWidth;

            labelHeight = EditorGUIUtility.singleLineHeight;
            fieldHeight = EditorGUIUtility.singleLineHeight;

            // Position des labels (sera ajustée dans ShowProperties)
            minLabelRect = new Rect(position.x, position.y + labelHeight, minMaxWidth, labelHeight);
            currentLabelRect = new Rect(minLabelRect.x + minMaxWidth, minLabelRect.y, currentWidth, labelHeight);
            maxLabelRect = new Rect(currentLabelRect.x + currentWidth, minLabelRect.y, minMaxWidth, labelHeight);

            // Position des champs (sera ajustée dans ShowProperties)
            minValueRect = new Rect(minLabelRect.x, minLabelRect.y + labelHeight + EditorGUIUtility.standardVerticalSpacing, minMaxWidth, fieldHeight);
            currentValueRect = new Rect(minLabelRect.x + minMaxWidth, minValueRect.y, currentWidth, fieldHeight);
            maxValueRect = new Rect(currentValueRect.x + currentWidth, minValueRect.y, minMaxWidth, fieldHeight);
        }

        private void ShowProperties(bool aGreaterThanB)
        {
            // Affiche toujours "A" et "B" comme labels, mais inverse leur position si A > B
            EditorGUI.LabelField(aGreaterThanB ? maxLabelRect : minLabelRect, new GUIContent("A"));
            EditorGUI.LabelField(!aGreaterThanB ? maxLabelRect : minLabelRect, new GUIContent("B"));
            EditorGUI.LabelField(currentLabelRect, new GUIContent("Current Value"));

            // Affiche les champs en inversant leur position si A > B
            if (aGreaterThanB)
            {
                // Si A > B, on affiche B à gauche et A à droite
                EditorGUI.PropertyField(minValueRect, bProp, GUIContent.none);
                EditorGUI.PropertyField(maxValueRect, aProp, GUIContent.none);
            }
            else
            {
                // Sinon, on affiche A à gauche et B à droite
                EditorGUI.PropertyField(minValueRect, aProp, GUIContent.none);
                EditorGUI.PropertyField(maxValueRect, bProp, GUIContent.none);
            }

            // Slider avec les bonnes bornes (min et max)
            currentValueProp.floatValue = EditorGUI.Slider(
                currentValueRect,
                currentValueProp.floatValue,
                Mathf.Min(aProp.floatValue, bProp.floatValue),
                Mathf.Max(aProp.floatValue, bProp.floatValue)
            );
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;
            if (property.isExpanded)
                totalLines += 2;
            return EditorGUIUtility.singleLineHeight * totalLines +
                   EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
        }
    }
}
