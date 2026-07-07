/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250413

using UnityEditor;
using UnityEngine;

namespace Devloader.Maths.CustomInspector
{
    [CustomPropertyDrawer(typeof(ClampedVector3))]
    public class ClampedVector3PropertyDrawer : PropertyDrawer
    {
        SerializedProperty current, min, max;

        int indentLevel = 0;


        float minMaxWidth, currentWidth;
        float labelHeight, fieldHeight;

        Rect currentXLabelRect, currentYLabelRect,  currentZLabelRect,
            currentXValueRect,  currentYValueRect,  currentZValueRect,
            maxXLabelRect,      maxYLabelRect,      maxZLabelRect,
            maxXValueRect,      maxYValueRect,      maxZValueRect,
            minXLabelRect,      minYLabelRect,      minZLabelRect,
            minXValueRect,      minYValueRect,      minZValueRect;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            min = property.FindPropertyRelative("_min");
            max = property.FindPropertyRelative("_max");
            current = property.FindPropertyRelative("_current");

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
            CheckCurrent(ref current, min.vector3Value, max.vector3Value);
        }

        private void CheckMinMax(ref SerializedProperty min, ref SerializedProperty max)
        {
            Vector3 tMin = min.vector3Value, tMax = max.vector3Value;

            if(tMin.x > tMax.x)
                (tMin.x, tMax.x) = (tMax.x, tMin.x);

            if(tMin.y > tMax.y)
                (tMin.y, tMax.y) = (tMax.y, tMin.y);

            if (tMin.z > tMax.z)
                (tMin.z, tMax.z) = (tMax.z, tMin.z);

            min.vector3Value = tMin;
            max.vector3Value = tMax;
        }

        private void CheckCurrent(ref SerializedProperty current, Vector3 min, Vector3 max) => current.vector3Value = new Vector3(
            Mathf.Clamp(current.vector3Value.x, min.x, max.x),
            Mathf.Clamp(current.vector3Value.y, min.y, max.y),
            Mathf.Clamp(current.vector3Value.z, min.z, max.z)
        );

        private void EvaluatePropertyRects(Rect position)
        {
            minMaxWidth = position.width / 5;
            currentWidth = position.width - 2 * position.width / 5;

            labelHeight = EditorGUIUtility.singleLineHeight;
            fieldHeight = EditorGUIUtility.singleLineHeight;

            minXLabelRect = new Rect(
                new Vector2(position.x, position.y + labelHeight),
                new Vector2(minMaxWidth, labelHeight)
            );

            currentXLabelRect = new Rect(
                new Vector2(minXLabelRect.x + minMaxWidth, minXLabelRect.y),
                new Vector2(currentWidth, labelHeight)
            );

            maxXLabelRect = new Rect(
                new Vector2(currentXLabelRect.x + currentWidth, minXLabelRect.y),
                new Vector2(minMaxWidth, labelHeight)
            );


            minXValueRect = new Rect(
                new Vector2(minXLabelRect.x, minXLabelRect.y + labelHeight + EditorGUIUtility.standardVerticalSpacing),
                new Vector2(minMaxWidth, fieldHeight)
            );

            currentXValueRect = new Rect(
                new Vector2(minXLabelRect.x + minMaxWidth, minXValueRect.y),
                new Vector2(currentWidth, fieldHeight)
            );

            maxXValueRect = new Rect(
                new Vector2(currentXValueRect.x + currentWidth, minXValueRect.y),
                new Vector2(minMaxWidth, fieldHeight)
            );



            minYLabelRect = new Rect(
                new Vector2(position.x, minXValueRect.y + minXValueRect.height + fieldHeight + EditorGUIUtility.standardVerticalSpacing * 1.5f),
                new Vector2(minMaxWidth, labelHeight)
            );

            currentYLabelRect = new Rect(
                new Vector2(minYLabelRect.x + minMaxWidth, minYLabelRect.y),
                new Vector2(currentWidth, labelHeight)
            );

            maxYLabelRect = new Rect(
                new Vector2(currentYLabelRect.x + currentWidth, minYLabelRect.y),
                new Vector2(minMaxWidth, labelHeight)
            );


            minYValueRect = new Rect(
                new Vector2(minYLabelRect.x, minYLabelRect.y + labelHeight + EditorGUIUtility.standardVerticalSpacing),
                new Vector2(minMaxWidth, fieldHeight)
            );

            currentYValueRect = new Rect(
                new Vector2(minYLabelRect.x + minMaxWidth, minYValueRect.y),
                new Vector2(currentWidth, fieldHeight)
            );

            maxYValueRect = new Rect(
                new Vector2(currentYLabelRect.x + currentWidth, minYValueRect.y),
                new Vector2(minMaxWidth, fieldHeight)
            );



            minZLabelRect = new Rect(
                new Vector2(position.x, minYValueRect.y + minYValueRect.height + fieldHeight + EditorGUIUtility.standardVerticalSpacing * 1.5f),
                new Vector2(minMaxWidth, labelHeight)
            );

            currentZLabelRect = new Rect(
                new Vector2(minZLabelRect.x + minMaxWidth, minZLabelRect.y),
                new Vector2(currentWidth, labelHeight)
            );

            maxZLabelRect = new Rect(
                new Vector2(currentZLabelRect.x + currentWidth, minZLabelRect.y),
                new Vector2(minMaxWidth, labelHeight)
            );


            minZValueRect = new Rect(
                new Vector2(minZLabelRect.x, minZLabelRect.y + labelHeight + EditorGUIUtility.standardVerticalSpacing),
                new Vector2(minMaxWidth, fieldHeight)
            );

            currentZValueRect = new Rect(
                new Vector2(minZLabelRect.x + minMaxWidth, minZValueRect.y),
                new Vector2(currentWidth, fieldHeight)
            );

            maxZValueRect = new Rect(
                new Vector2(currentZLabelRect.x + currentWidth, minZValueRect.y),
                new Vector2(minMaxWidth, fieldHeight)
            );
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
                totalLines += 8;

            return EditorGUIUtility.singleLineHeight * totalLines + EditorGUIUtility.standardVerticalSpacing * (totalLines-1);
        }

        private void ShowProperties()
        {
            Vector3 currentValue = current.vector3Value;
            Vector3 maxValue = max.vector3Value;
            Vector3 minValue = min.vector3Value;

            EditorGUI.LabelField(minXLabelRect, new GUIContent("Min X value"));
            EditorGUI.LabelField(currentXLabelRect, new GUIContent("Current X value"));
            EditorGUI.LabelField(maxXLabelRect, new GUIContent("Max X value"));

            minValue.x = EditorGUI.FloatField(minXValueRect, minValue.x);
            currentValue.x = EditorGUI.Slider(currentXValueRect, current.vector3Value.x, min.vector3Value.x, max.vector3Value.x);
            maxValue.x = EditorGUI.FloatField(maxXValueRect, maxValue.x);


            EditorGUI.LabelField(minYLabelRect, new GUIContent("Min Y value"));
            EditorGUI.LabelField(currentYLabelRect, new GUIContent("Current Y value"));
            EditorGUI.LabelField(maxYLabelRect, new GUIContent("Max Y value"));

            minValue.y = EditorGUI.FloatField(minYValueRect, minValue.y);
            currentValue.y = EditorGUI.Slider(currentYValueRect, current.vector3Value.y, min.vector3Value.y, max.vector3Value.y);
            maxValue.y = EditorGUI.FloatField(maxYValueRect, maxValue.y);


            EditorGUI.LabelField(minZLabelRect, new GUIContent("Min Z value"));
            EditorGUI.LabelField(currentZLabelRect, new GUIContent("Current Z value"));
            EditorGUI.LabelField(maxZLabelRect, new GUIContent("Max Z value"));

            minValue.z = EditorGUI.FloatField(minZValueRect, minValue.z);
            currentValue.z = EditorGUI.Slider(currentZValueRect, current.vector3Value.z, min.vector3Value.z, max.vector3Value.z);
            maxValue.z = EditorGUI.FloatField(maxZValueRect, maxValue.z);

            current.vector3Value = currentValue;
            max.vector3Value = maxValue;
            min.vector3Value = minValue;
        }
    }
}