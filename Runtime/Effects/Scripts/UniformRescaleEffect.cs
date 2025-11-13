/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

using UnityEngine;
using Devloader.Extensions;

namespace Devloader.Effects
{
    [System.Obsolete("Use FadeTransformScale instead")]
    public class UniformRescaleEffect : AbstractEffect
    {
        [Header("Transform où appliquer le rescale")]
        public Transform rescaleTransform;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!rescaleTransform)
                rescaleTransform = this.ValidateComponent<Transform>();

            base.OnValidate();
        }
#endif

        private void Awake()
        {
            if (!rescaleTransform)
                rescaleTransform = this.ValidateComponent<Transform>();

            processAction = delegate (float value)
            { rescaleTransform.localScale = Vector3.one * value; };
        }
    }
}