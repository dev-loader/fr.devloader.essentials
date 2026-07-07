/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240621

using UnityEngine;

namespace Devloader.Utils
{
    public class LookAtTransform : MonoBehaviour
    {
        [SerializeField, Tooltip("Invert y rotation to face the camera")] private bool uiMode = false;
        [SerializeField] private Transform transformToLookAt;
        [Space]
        [SerializeField] private bool useFixedUpdate = false;

        protected virtual void FixedUpdate()
        {
            if (!useFixedUpdate || !transformToLookAt) return;

            if (uiMode)
                transform.rotation = Quaternion.LookRotation((transform.position - transformToLookAt.position).normalized, Vector3.up);
            else
                transform.LookAt(transformToLookAt);
        }

        protected virtual void Update()
        {
            if (useFixedUpdate || !transformToLookAt) return;

            if (uiMode)
                transform.rotation = Quaternion.LookRotation((transform.position - transformToLookAt.position).normalized, Vector3.up);
            else
                transform.LookAt(transformToLookAt);
        }

        public virtual bool UIMode
        {
            get => uiMode;
            set => uiMode = value;
        }

        public virtual Transform TransformToLookAt
        {
            get => transformToLookAt;
            set => transformToLookAt = value;
        }

        public virtual bool UseFixedUpdate
        {
            get => useFixedUpdate;
            set => useFixedUpdate = value;
        }
    }
}