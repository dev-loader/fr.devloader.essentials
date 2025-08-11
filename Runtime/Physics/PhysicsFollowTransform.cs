/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250811

using UnityEngine;

namespace Devloader
{
    /// <summary>
    /// Script permettant à un rigidbody de suivre le transform d'un autre objet
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsFollowTransform : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] Transform targetTransform;
        [Space]

        [SerializeField] bool useFixedUpdate;

        private void FixedUpdate()
        {
            if (!useFixedUpdate || !targetTransform)
                return;

            MoveRigidBodyToTransform(targetTransform);
        }

        private void Update()
        {
            if (useFixedUpdate || !targetTransform)
                return;

            MoveRigidBodyToTransform(targetTransform);
        }

        public void MoveRigidBodyToTransform(Transform transform)
        {
            if (transform)
                MoveRigidBodyToPosition(transform.position);
        }

        public void MoveRigidBodyToPosition(Vector3 position) => rb.MovePosition(position);
    }
}