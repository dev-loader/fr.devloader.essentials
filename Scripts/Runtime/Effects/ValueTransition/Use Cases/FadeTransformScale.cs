/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using Devloader.Maths;
using UnityEngine;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade Transform Scale")]
    public class FadeTransformScale : AbstractEffect
    {
        [SerializeField] Transform _transform;
        [SerializeField] RangedVector3 _scaleInterval = new RangedVector3(Vector3.zero, Vector3.one);

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!_transform)
                _transform = transform;
        }
#endif

        private void Awake() => ProcessAction = value => _transform.localScale = Vector3.Lerp(_scaleInterval.a, _scaleInterval.b, value);
    }
}