/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

using UnityEditor;
using UnityEngine;

namespace Devloader.Maths.CustomInspector
{
    [CustomPropertyDrawer(typeof(RangedVector3))]
    public class RangedVector3PropertyDrawer : PropertyDrawer
    {
        private SerializedProperty aProp;
        private SerializedProperty bProp;
        private SerializedProperty currentValueProp;

        private float sliderValue = 0f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            aProp = property.FindPropertyRelative("_aBound");
            bProp = property.FindPropertyRelative("_bBound");
            currentValueProp = property.FindPropertyRelative("_currentValue");

            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(
                position.position,
                new Vector2(position.width, EditorGUIUtility.singleLineHeight)
            );

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, label + " [" + aProp.vector3Value + ".." + bProp.vector3Value + "]");

            if (property.isExpanded)
            {
                float lineHeight = EditorGUIUtility.singleLineHeight;
                float spacing = EditorGUIUtility.standardVerticalSpacing;

                // Ligne 1: Labels A et B
                DrawABLabels(position, lineHeight, spacing);

                // Ligne 2: Valeurs A et B
                DrawABValues(position, lineHeight, spacing);

                // Ligne 3: Slider 0-1
                sliderValue = DrawSlider(position, lineHeight, spacing);

                // Ligne 4: Label "Current"
                DrawCurrentLabel(position, lineHeight, spacing);

                // Ligne 5: Valeurs Current (mises à jour par le slider)
                currentValueProp.vector3Value = Vector3.Lerp(aProp.vector3Value, bProp.vector3Value, sliderValue);
                DrawCurrentValues(position, lineHeight, spacing);
            }

            EditorGUI.EndFoldoutHeaderGroup();

            EditorGUI.EndProperty();
        }

        private void DrawABLabels(Rect position, float lineHeight, float spacing)
        {
            float y = position.y + lineHeight + spacing;
            Rect aLabelRect = new Rect(position.x, y, 30, lineHeight);
            Rect bLabelRect = new Rect(position.x + position.width - 30, y, 30, lineHeight);

            EditorGUI.LabelField(aLabelRect, "A");
            EditorGUI.LabelField(bLabelRect, "B");
        }

        private void DrawABValues(Rect position, float lineHeight, float spacing)
        {
            float y = position.y + lineHeight * 2 + spacing * 2;
            Rect aValueRect = new Rect(position.x, y, 100, lineHeight);
            Rect bValueRect = new Rect(position.x + position.width - 100, y, 100, lineHeight);

            EditorGUI.PropertyField(aValueRect, aProp, GUIContent.none);
            EditorGUI.PropertyField(bValueRect, bProp, GUIContent.none);
        }

        private float DrawSlider(Rect position, float lineHeight, float spacing)
        {
            float y = position.y + lineHeight * 3 + spacing * 3;
            Rect sliderRect = new Rect(position.x + 30, y, position.width - 60, lineHeight);

            return EditorGUI.Slider(sliderRect, sliderValue, 0f, 1f);
        }

        private void DrawCurrentLabel(Rect position, float lineHeight, float spacing)
        {
            float y = position.y + lineHeight * 4 + spacing * 4;
            Rect currentLabelRect = new Rect(position.x, y, 50, lineHeight);

            EditorGUI.LabelField(currentLabelRect, "Current");
        }

        private void DrawCurrentValues(Rect position, float lineHeight, float spacing)
        {
            float y = position.y + lineHeight * 5 + spacing * 5;
            Rect currentValueRect = new Rect(position.x, y, position.width, lineHeight);

            EditorGUI.PropertyField(currentValueRect, currentValueProp, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            return EditorGUIUtility.singleLineHeight * 6 +
                   EditorGUIUtility.standardVerticalSpacing * 5;
        }
    }
}
