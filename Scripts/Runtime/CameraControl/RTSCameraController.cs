/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260116

using UnityEngine;
using UnityEngine.Events;

using Devloader.Utils;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Devloader.CameraControl
{
    public class RTSCameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] Camera usedCamera;
        [SerializeField] Vector3 cameraAxis = new Vector3(-1, 2, -1);
        [SerializeField] float cameraAxisMultiplier = 20;
        [SerializeField] float cameraSpeed = 2;
        [SerializeField] float cameraAngularSpeed = 90;

        [Header("Pivot Target Settings")]
        [SerializeField] Transform pivotTargetTransform;
        [SerializeField] Vector3 pivotTargetPositionOffset;

        [Header("Camera Target Settings")]
        [SerializeField] Transform cameraTargetTransform;
        [SerializeField] Vector3 cameraTargetPositionOffset;

        [Header("Click Settings")]
        [SerializeField] bool _canClick = true;
#if ENABLE_LEGACY_INPUT_MANAGER
        [SerializeField] KeyCode clickButton = KeyCode.Mouse0;
#else
        [SerializeField] InputActionReference clickAction;
#endif
        [SerializeField] UnityEvent<Vector3> onClick = new UnityEvent<Vector3>();

        private Vector3 worldPoint;
        private Ray worldRay;
        private RaycastHit worldHit;

        public bool CanClick
        {
            get => _canClick;
            set => _canClick = value;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(!usedCamera)
                usedCamera = CameraUtils.Active;

            if (pivotTargetTransform)
            {
                transform.position = pivotTargetTransform.position + pivotTargetPositionOffset;
                transform.eulerAngles = pivotTargetTransform.eulerAngles;
            }

            if (usedCamera)
                usedCamera.transform.LookAt(cameraTargetTransform.position + cameraTargetPositionOffset);
        }
#endif

        private void LateUpdate()
        {
            if(pivotTargetTransform)
                transform.Translate((pivotTargetTransform.position + pivotTargetPositionOffset - transform.position) * Time.fixedDeltaTime * cameraSpeed);

            if (usedCamera)
                UpdateCameraTransform(Time.fixedDeltaTime);
        }

        void Update()
        {
            if (!CanClick)
                return;

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(clickButton))
            {
                if (EventSystem.current.currentSelectedGameObject)
                    return;

                worldRay = usedCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(worldRay, out worldHit, usedCamera.farClipPlane))
                    worldPoint = worldHit.point;

                onClick.Invoke(worldPoint);
            }
#endif
        }

        void UpdateCameraTransform(float deltaTime)
        {
            usedCamera.transform.localPosition = cameraAxis * cameraAxisMultiplier;

            if (cameraTargetTransform)
                usedCamera.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(usedCamera.transform.forward, cameraTargetTransform.position + cameraTargetPositionOffset - usedCamera.transform.position, Mathf.Deg2Rad * deltaTime * cameraAngularSpeed, 0));
            else
                usedCamera.transform.localEulerAngles = new Vector3(50, 30, 0);
        }
    }
}