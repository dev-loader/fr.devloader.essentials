/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260609

using Devloader.Maths;
using UnityEngine;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade Transform Position")]
    public class FadeTransformPosition : AbstractEffect
    {
        [Header("References")]
        [SerializeField] Transform _startPositionRef;
        [SerializeField] Transform _finalPositionRef;

        [SerializeField] RangedVector3 _clampedPosition = new RangedVector3(Vector3.zero, new Vector3(1, 0, 1), Vector3.zero);

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_startPositionRef)
                _clampedPosition.a = _startPositionRef.position;

            if (_finalPositionRef)
                _clampedPosition.b = _finalPositionRef.position;
        }
#endif

        private void Awake() => ProcessAction = value => transform.position = _clampedPosition.Lerp(value);

        [System.Obsolete("Use UpdatePositionsFromReferences then call SetToBegin on the returned reference instead")]
        public override AbstractEffect SetToBegin(int direction)
        {
            if (_startPositionRef)
                _clampedPosition.a = _startPositionRef.position;

            if (_finalPositionRef)
                _clampedPosition.b = _finalPositionRef.position;

            return base.SetToBegin(direction);
        }

        public AbstractEffect UpdatePositionsFromReferences()
        {
            if (_startPositionRef)
                _clampedPosition.a = _startPositionRef.position;

            if (_finalPositionRef)
                _clampedPosition.b = _finalPositionRef.position;

            return this;
        }
    }
}