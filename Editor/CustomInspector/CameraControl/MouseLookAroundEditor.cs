/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260116

using Devloader.CameraControl;

using UnityEditor;
using UnityEngine;

namespace Devloader.CustomInspector.CameraControl
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MouseLookAround))]
    public class MouseLookAroundEditor : MouseLookEditor
    {
        SerializedProperty cameraHeight;
        SerializedProperty cameraTransform;
        SerializedProperty distanceRatio;
        SerializedProperty eyeTrackerHeight;
        SerializedProperty eyeTrackerTransform;
        SerializedProperty followSpeed;
        SerializedProperty followTarget;
        SerializedProperty hitDistance;
        SerializedProperty hitPosition;
        SerializedProperty isHitting;
        SerializedProperty lockCursorOnAwake;
        SerializedProperty lookAtOffset;
        SerializedProperty lookAtTarget;
        SerializedProperty smoothLookAt;
        SerializedProperty targetTransform;
        SerializedProperty maxDistance;
        SerializedProperty minDistance;
        SerializedProperty obstacleLayerMask;
        SerializedProperty zoomAxisDelta;
        SerializedProperty zoomAxisSensitivity;

        bool foldoutMouseLook = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            cameraHeight = serializedObject.FindProperty("cameraHeight");
            cameraTransform = serializedObject.FindProperty("cameraTransform");
            distanceRatio = serializedObject.FindProperty("distanceRatio");
            eyeTrackerHeight = serializedObject.FindProperty("eyeTrackerHeight");
            eyeTrackerTransform = serializedObject.FindProperty("eyeTrackerTransform");
            followSpeed = serializedObject.FindProperty("followSpeed");
            followTarget = serializedObject.FindProperty("followTarget");
            hitDistance = serializedObject.FindProperty("hitDistance");
            hitPosition = serializedObject.FindProperty("hitPosition");
            isHitting = serializedObject.FindProperty("isHitting");
            lockCursorOnAwake = serializedObject.FindProperty("lockCursorOnAwake");
            lookAtOffset = serializedObject.FindProperty("lookAtOffset");
            lookAtTarget = serializedObject.FindProperty("lookAtTarget");
            smoothLookAt = serializedObject.FindProperty("smoothLookAt");
            targetTransform = serializedObject.FindProperty("targetTransform");
            maxDistance = serializedObject.FindProperty("maxDistance");
            minDistance = serializedObject.FindProperty("minDistance");
            obstacleLayerMask = serializedObject.FindProperty("obstacleLayerMask");
            zoomAxisDelta = serializedObject.FindProperty("zoomAxisDelta");
            zoomAxisSensitivity = serializedObject.FindProperty("zoomAxisSensitivity");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            MouseLookAround myScript = serializedObject.targetObject as MouseLookAround;

            EditorGUILayout.LabelField("Cursor settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            /// Lock cursor on awake
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - lock cursor on awake");

            EditorGUILayout.PropertyField(lockCursorOnAwake, new GUIContent("Lock cursor on awake"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Look at settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            /// Target transform
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - target transform");

            EditorGUILayout.PropertyField(targetTransform, new GUIContent("Target transform"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            /// Look at target
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - look at target");

            EditorGUILayout.PropertyField(lookAtTarget, new GUIContent("Look at target"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            /// Look at target
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - smooth look at");

            EditorGUILayout.PropertyField(smoothLookAt, new GUIContent("Smooth look at"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            /// Look at offset
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - look at offset");

            EditorGUILayout.PropertyField(lookAtOffset, new GUIContent("Look at offset"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Camera settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            /// Camera transform
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - camera transform");

            EditorGUILayout.PropertyField(cameraTransform, new GUIContent("Camera transform"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            EditorGUILayout.Space();

            /// Follow target
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - follow target");

            EditorGUILayout.PropertyField(followTarget, new GUIContent("Follow target"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            /// Camera height
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - camera height");

            EditorGUILayout.PropertyField(cameraHeight, new GUIContent("Camera height"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            EditorGUILayout.Space();

            /// Min distance
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - min distance");

            EditorGUILayout.PropertyField(minDistance, new GUIContent("Min distance"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            /// Max distance
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - max distance");

            EditorGUILayout.PropertyField(maxDistance, new GUIContent("Max distance"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            /// Distance ratio
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - distance ratio");

            EditorGUILayout.PropertyField(distanceRatio, new GUIContent("Distance ratio"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            /// Obstacle Layer Mask
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - obstacle layer mask");

            EditorGUILayout.PropertyField(obstacleLayerMask, new GUIContent("Obstacle Layer Mask"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            EditorGUILayout.Space();

#if !ENABLE_LEGACY_INPUT_MANAGER
            /// Zoom axis input action
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - zoom axis input action");

            EditorGUILayout.PropertyField(zoomAxisDelta, new GUIContent("Zoom Axis Delta Input"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);
#endif

            /// Zoom axis sensitivity
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - zoom axis sensitivity");

            EditorGUILayout.PropertyField(zoomAxisSensitivity, new GUIContent("Zoom Axis Sensitivity"));

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(myScript);

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Eye tracker settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            /// Eye tracker transform
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - eye tracker transform");

            EditorGUILayout.PropertyField(eyeTrackerTransform, new GUIContent("Eye tracker transform"));

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            /// Eye tracker transform
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(myScript, "Mouse Look - eye tracker transform");

            EditorGUILayout.PropertyField(eyeTrackerHeight, new GUIContent("Eye tracker height"));

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            /// Debug
            EditorGUILayout.HelpBox(
                "\nCurrent distance: " + myScript.currentDistance +
                "\nCurrent offset: " + myScript.currentOffset +
                "\nis hitting: " + isHitting.boolValue +
                "\nHit distance: " + hitDistance.floatValue +
                "\nHit position: " + hitPosition.vector3Value +
                "\nHit offset: " + myScript.hitOffset
            , MessageType.Info);

            foldoutMouseLook = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutMouseLook, "Mouse Look Settings");
            if (foldoutMouseLook)
                base.OnInspectorGUI();
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUI.BeginDisabledGroup(!targetTransform.objectReferenceValue);
            if (GUILayout.Button("Update camera position"))
            {
                myScript.UpdateCameraPosition(false);
                myScript.UpdateEyeTrackerPosition();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
        }