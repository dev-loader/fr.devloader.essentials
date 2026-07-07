using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TransformReset : MonoBehaviour
{
    [SerializeField, Tooltip("If set to false, values will be reset with 0")] bool useStartValues = true;
    [SerializeField, Tooltip("If set to true, local values will be used for position and rotation")] bool useLocalValues = true;
    [Space]

    [SerializeField] bool resetPosition = true;
    [SerializeField] bool resetRotation = true;
    [SerializeField] bool resetScale = true;

    Vector3 initialPosition = Vector3.zero;
    Vector3 initialEulerAngles = Vector3.zero;
    Vector3 initialScale = Vector3.zero;

    void Start()
    {
        if(useStartValues)
        {
            initialPosition = useLocalValues ? transform.localPosition : transform.position;
            initialEulerAngles = useLocalValues ? transform.localEulerAngles : transform.eulerAngles;
            initialScale = transform.localScale;
        }
    }

    public void ResetTransform()
    {
        if(useLocalValues)
        {
            if(resetPosition)
                transform.localPosition = initialPosition;

            if(resetRotation)
                transform.localEulerAngles = initialEulerAngles;
        }
        else
        {
            if (resetPosition)
                transform.position = initialPosition;

            if (resetRotation)
                transform.eulerAngles = initialEulerAngles;
        }

        transform.localScale = initialScale;
    }

    public void ResetPosition()
    {
        if (!resetPosition)
            return;

        if (useLocalValues)
            transform.localPosition = initialPosition;
        else
            transform.position = initialPosition;
    }

    public void ResetRotation()
    {
        if (!resetRotation)
            return;

        if (useLocalValues)
            transform.localEulerAngles = initialEulerAngles;
        else
            transform.eulerAngles = initialEulerAngles;
    }

    public void ResetScale()
    {
        if(resetScale)
            transform.localScale = initialScale;
    }
}
