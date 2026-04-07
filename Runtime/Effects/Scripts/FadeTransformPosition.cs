using UnityEngine;
using Devloader.Effects;
using Devloader.Maths;

public class FadeTransformPosition : AbstractEffect
{
    [Header("References")]
    [SerializeField] Transform _startPositionRef;
    [SerializeField] Transform _finalPositionRef;

    [SerializeField] ClampedVector3 _clampedPosition = new ClampedVector3(Vector3.zero, new Vector3(1,0,1), Vector3.zero);

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        if(_startPositionRef)
            _clampedPosition.min = _startPositionRef.position;

        if(_finalPositionRef)
            _clampedPosition.max = _finalPositionRef.position;
    }
#endif

    private void Awake() => processAction = value => transform.position = _clampedPosition.Lerp(value);

    public override void SetToBegin(int direction)
    {
        if (_startPositionRef)
            _clampedPosition.min = _startPositionRef.position;

        if (_finalPositionRef)
            _clampedPosition.max = _finalPositionRef.position;

        base.SetToBegin(direction);
    }
}
