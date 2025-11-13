/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

using UnityEngine;

namespace Devloader.Effects
{
    public class FadeTransformScale : AbstractEffect
    {
        [SerializeField] Transform _transform;

        [SerializeField] Vector3 _firstScale;
        [SerializeField] Vector3 _finalScale;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!_transform)
                _transform = transform;
        }
#endif

        private void Awake()
        {
            if (!_transform)
                _transform = transform;

            processAction = delegate (float value)
            { _transform.localScale = Vector3.Lerp(_firstScale, _finalScale, value); };
        }
    }
}