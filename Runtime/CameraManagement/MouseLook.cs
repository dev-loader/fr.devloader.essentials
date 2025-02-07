/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240620

using UnityEngine;

#if !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

[AddComponentMenu("Devloader/Camera/Mouse Look")]
public class MouseLook : MonoBehaviour
{
    public enum RotationAxis { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    [SerializeField] RotationAxis axis = RotationAxis.MouseX;

#if ENABLE_LEGACY_INPUT_MANAGER
    [SerializeField] private string xAxisInputName = "Mouse X";
    [SerializeField] private string yAxisInputName = "Mouse Y";
#else
    [SerializeField] private InputActionReference deltaInputValue;
    Vector2 delta;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
    [SerializeField] float sensitivityX = 15f;
    [SerializeField] float sensitivityY = 15f;
#else
    [SerializeField] float sensitivityX = .15f;
    [SerializeField] float sensitivityY = .15f;
#endif

    /*[SerializeField] float minimumX = -360f;
    [SerializeField] float maximumX = 360f;*/

    [SerializeField] float minimumY = -60f;
    [SerializeField] float maximumY = 60f;

	float rotationY = 0F;

    // From me
    [SerializeField]
    private static bool _isMouseLock = true;
    public static bool isMouseLock
    {
        get => _isMouseLock;

        set
        {
            _isMouseLock = value;

            if(value && Application.isPlaying)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

#if !ENABLE_LEGACY_INPUT_MANAGER
    protected virtual void OnEnable()
    {
        deltaInputValue.action.performed += UpdateDelta;
        deltaInputValue.action.canceled += ResetDelta;
    }
#endif

    protected virtual void Start()
    {
        if (TryGetComponent(out Rigidbody rb))
            rb.freezeRotation = true;
        
        if (_isMouseLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

#if !ENABLE_LEGACY_INPUT_MANAGER
    protected virtual void OnDisable()
    {
        deltaInputValue.action.performed -= UpdateDelta;
        deltaInputValue.action.canceled -= ResetDelta;
    }
#endif

    protected virtual void Update ()
    {
        if (_isMouseLock)
        {
            if (axis == RotationAxis.MouseXAndY)
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                float rotationX = transform.localEulerAngles.y + Input.GetAxis(xAxisInputName) * sensitivityX;
                rotationY += Input.GetAxis(yAxisInputName) * sensitivityY;
#else
                float rotationX = transform.localEulerAngles.y + delta.x * sensitivityX;
                rotationY += delta.y * sensitivityY;
#endif

                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axis == RotationAxis.MouseX)
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                transform.Rotate(0, Input.GetAxis(xAxisInputName) * sensitivityX, 0);
#else
                transform.Rotate(0, delta.x * sensitivityX, 0);
#endif
            }
            else
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                rotationY += Input.GetAxis(yAxisInputName) * sensitivityY;
#else
                rotationY += delta.y * sensitivityY;
#endif
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }
    }

#if !ENABLE_LEGACY_INPUT_MANAGER
    private void ResetDelta(InputAction.CallbackContext obj) => delta = Vector2.zero;
#endif

    public void SetCameraSensitivity(float bothAxis)
    {
        sensitivityX = bothAxis;
        sensitivityY = bothAxis;
    }

    public void SetCameraSensitivity(float horizontalAxis, float verticalAxis)
    {
        sensitivityX = horizontalAxis;
        sensitivityY = verticalAxis;
    }

#if !ENABLE_LEGACY_INPUT_MANAGER
    private void UpdateDelta(InputAction.CallbackContext obj) => delta = obj.ReadValue<Vector2>();
#endif

    public static void Lock() => isMouseLock = true;

    public static void Unlock() => isMouseLock = false;

    public float YRotation => rotationY;
}