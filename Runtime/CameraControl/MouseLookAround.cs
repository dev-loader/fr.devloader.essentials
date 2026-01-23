/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260116

using UnityEngine;
using Devloader.Utils;
using Devloader.Extensions;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Devloader.CameraControl
{
    [AddComponentMenu("Devloader/Camera Control/Mouse Look Around")]
    public class MouseLookAround : MouseLook
    {
        [Header("Cursor settings")]
        [SerializeField] private bool lockCursorOnAwake = true;

        [Header("Look at settings")]
        [SerializeField] private Transform targetTransform;
        [Space]

        [SerializeField] private bool lookAtTarget = true;
        [SerializeField] private bool smoothLookAt = true;
        [Space]

        [SerializeField] private Vector3 lookAtOffset = Vector3.up * 0.7f;

        [Header("Camera settings")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private bool followTarget = true;
        [SerializeField] private float followSpeed = 1.0f;

        [SerializeField] private float cameraHeight = 2f;

        [SerializeField] private float minDistance = 0.5f;
        [SerializeField] private float maxDistance = 3.5f;

        [SerializeField, Range(0,1)] private float distanceRatio = 0.5f;

        [SerializeField] private LayerMask obstacleLayerMask;

#if ENABLE_INPUT_SYSTEM
        [SerializeField] private InputActionReference zoomAxisDelta;
#endif
        [SerializeField] private float zoomAxisSensitivity = 1;

        [Header("Eye tracker settings")]
        [SerializeField] private Transform eyeTrackerTransform;
        [SerializeField] private float eyeTrackerHeight = 1.22f;

        // Hit management
        RaycastHit hit;
        [SerializeField] private Vector3 hitPosition;
        [SerializeField] private float hitDistance;
        [SerializeField] private bool isHitting = false;

        float cameraHitSphereRadius = 1f;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                return;

            UpdateCameraPosition(false);
            UpdateCameraLookAt(false);
            UpdateEyeTrackerPosition();
        }

        private void Reset()
        {
            UpdateCameraPosition(false);
            UpdateCameraLookAt(false);
            UpdateEyeTrackerPosition();
        }
#endif

        private void Awake()
        {
            Object[] mouseLookArounds = this.FindSimilar();

            // S'il y en a déjà un, on supprime celui-ci
            if (mouseLookArounds.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            // Make the rigid body not change rotation
            if (TryGetComponent(out Rigidbody rb))
                rb.freezeRotation = true;

            isMouseLock = lockCursorOnAwake;
        }

#if ENABLE_INPUT_SYSTEM
        protected override void OnEnable()
        {
            base.OnEnable();

            if(zoomAxisDelta is not null && zoomAxisDelta.action is not null)
                zoomAxisDelta.action.performed += ZoomAxisDeltaHandler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if(zoomAxisDelta is not null && zoomAxisDelta.action is not null)
                zoomAxisDelta.action.performed -= ZoomAxisDeltaHandler;
        }
#endif

        protected override void Start()
        {
            ResetCamera();
            UpdateEyeTrackerPosition();
        }

#if !ENABLE_INPUT_SYSTEM
        private void FixedUpdate()
        {
            float wheelValue = Input.GetAxis("Mouse ScrollWheel");

            if(wheelValue != 0)
                distanceRatio = Mathf.Clamp01(distanceRatio + wheelValue * Time.deltaTime * zoomAxisSensitivity);
        }
#endif

        private void LateUpdate()
        {
            float distance = isHitting ? hitDistance : Vector3.Distance(cameraPosition, targetPosition);

            if (Physics.Raycast(targetPosition, (cameraPosition - targetPosition).normalized, out hit, distance + cameraHitSphereRadius, obstacleLayerMask))
            {
                hitPosition = hit.point;
                hitDistance = Vector3.Distance(targetTransform.position + Vector3.up * cameraHeight, hit.point);

                isHitting = Mathf.Min(Vector3.Distance(transform.position, hit.point), currentDistance) != currentDistance;
                //PlayerController.SetOpacity(.2f);
            }
            else
            {
                isHitting = false;
                //PlayerController.SetOpacity(1f);
            }

            Follow();

            UpdateCameraPosition();
            UpdateCameraLookAt(SmoothLookAt);

            UpdateEyeTrackerPosition();
        }

        public void Follow()
        {
            if (followTarget && targetTransform && transform.position != targetTransform.position + Vector3.up * cameraHeight)
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position + Vector3.up * cameraHeight, Mathf.Max(followSpeed, Vector3.Distance(transform.position, targetTransform.position)));
        }

        public void ResetCamera()
        {
            UpdateCameraPosition(false);
            UpdateCameraLookAt(false);
        }

        public void UpdateCameraPosition(bool smoothPosition = true)
        {
            if (!cameraTransform)
                cameraTransform = transform.Find("Camera");

            if (!cameraTransform)
                return;

            if (isHitting)
                cameraTransform.position = hitPosition + cameraForward * cameraHitSphereRadius;
            else if(smoothPosition)
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, currentOffset, Time.deltaTime * forward.magnitude);
            else
                cameraTransform.localPosition = currentOffset;
        }

        public void UpdateCameraLookAt(bool smoothLookAt = true)
        {
            if (lookAtTarget && targetTransform)
            {
                if (smoothLookAt)
                    cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, Quaternion.LookRotation(targetPosition - cameraPosition, Vector3.up), Time.deltaTime);
                //cameraTransform.LookAt(Vector3.MoveTowards(cameraPosition + cameraForward, targetPosition - cameraPosition, Time.deltaTime));
                else
                    cameraTransform.LookAt(targetPosition);
            }
        }

        public void UpdateEyeTrackerPosition()
        {
            if (!eyeTrackerTransform)
                eyeTrackerTransform = transform.Find("EyeTracker");

            if (!eyeTrackerTransform)
                return;

            Vector3 offset = -currentOffset;
            offset.y = eyeTrackerHeight;

            eyeTrackerTransform.localPosition = offset;
        }

#if ENABLE_INPUT_SYSTEM
        private void ZoomAxisDeltaHandler(InputAction.CallbackContext context)
        {
            distanceRatio = Mathf.Clamp01(distanceRatio + context.ReadValue<float>() * Time.deltaTime * zoomAxisSensitivity);
        }
#endif

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(cameraPosition, forward);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(targetPosition, (cameraPosition - targetPosition).normalized * Vector3.Distance(cameraPosition, targetPosition));

            if (isHitting)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hitPosition, .1f);
            }
        }

        public Vector3 cameraForward { get => CameraUtils.Active.transform.forward; }
        public Vector3 cameraPosition { get => CameraUtils.Active.transform.position; }

        public float currentDistance { get => deltaDistance * distanceRatio + minDistance; }
        public Vector3 currentOffset { get => Vector3.up * cameraHeight + Vector3.back * currentDistance; }

        public float deltaDistance { get => maxDistance - minDistance; }
        public Vector3 forward { get => Vector3.Distance(targetTransform ? targetTransform.position + lookAtOffset : lookAtOffset, cameraPosition) * cameraForward; }
        public Vector3 hitOffset { get => Vector3.up * cameraHeight + Vector3.back * hitDistance; }

        public bool lockCursor
        { set { isMouseLock = value; } }

        public Vector3 targetPosition { get => targetTransform ? targetTransform.position + lookAtOffset : lookAtOffset; }

        public float DistanceRation
        {
            get => distanceRatio;
            set => distanceRatio = value;
        }

        public Transform LookAtTarget
        {
            get => targetTransform;
            set => targetTransform = value;
        }

        public bool SmoothLookAt
        {
            get => smoothLookAt;
            set => SmoothLookAt = value;
        }
    }
}

