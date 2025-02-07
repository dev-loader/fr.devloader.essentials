using Devloader.Extensions;
using UnityEngine;

/// <summary>
/// Script permettant à un rigidbody de suivre le transform d'un autre objet
/// </summary>
public class PhysicsFollowTransform : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform targetTransform;
    [Space]

    [SerializeField] bool useFixedUpdate;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(!rb)
            rb = this.ValidateComponent<Rigidbody>();
    }

#endif

    private void Start()
    {
        if (!rb)
            rb = this.ValidateComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!useFixedUpdate)
            return;

        MoveRigidBodyToTransform(targetTransform);
    }

    private void Update()
    {
        if (useFixedUpdate)
            return;

        MoveRigidBodyToTransform(targetTransform);
    }

    private void MoveRigidBodyToTransform(Transform transform)
    {
        if (transform)
            MoveRigidBodyToPosition(transform.position);
    }

    private void MoveRigidBodyToPosition(Vector3 position) => rb.MovePosition(position);
}
