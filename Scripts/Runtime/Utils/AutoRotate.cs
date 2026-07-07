using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private bool useFixedUpdate = false;
    [Space]

    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float angularSpeed = 90;

    [Header("For debug purpose")]
    [SerializeField] private Vector3 initialEulerAngles;

    public Vector3 CurrentEulerAngles => transform.localEulerAngles;

    private void Start() => initialEulerAngles = transform.localEulerAngles;

    private void Update()
    {
        if (useFixedUpdate)
            return;

        transform.Rotate(rotationAxis * angularSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!useFixedUpdate)
            return;

        transform.Rotate(rotationAxis * angularSpeed * Time.fixedDeltaTime);
    }

    public float AngularSpeed { get => angularSpeed; set => angularSpeed = value; }
    public Vector3 RotationAxis { get => rotationAxis; set => rotationAxis = value; }
}
