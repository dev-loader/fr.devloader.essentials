using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRescaleEffect : MonoBehaviour
{
    [SerializeField] private bool useFixedUpdate = false;
    [Space]

    [SerializeField] private Vector3 minScale = Vector3.one;
    [SerializeField] private Vector3 maxScale = Vector3.one * 2;

    [SerializeField] private float rescaleSpeed = 1;

    [Header("For debug purpose")]
    [SerializeField] private Vector3 initialScale;

    public Vector3 InitialScale => initialScale;

    Vector3 targetScale;

    public Vector3 Scale
    {
        get => transform.localScale;
        set => targetScale = value;
    }

    public float ScaleValue
    {
        get => transform.localScale.magnitude;
        set => targetScale = Vector3.Lerp(minScale, maxScale, value);
    }

    private void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    private void Update()
    {
        if (useFixedUpdate)
            return;

        if(transform.localScale != targetScale)
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, rescaleSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!useFixedUpdate)
            return;

        if (transform.localScale != targetScale)
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, rescaleSpeed * Time.fixedDeltaTime);
    }
}
