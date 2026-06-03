/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using UnityEngine;
using Devloader.Extensions;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade CanvasGroup Alpha")]
    public class FadeCanvasGroup : AbstractEffect
    {
        [Header("CanvasGroup où appliquer le fondu en alpha")]
        [SerializeField] CanvasGroup _fadeCanvasGroup;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!_fadeCanvasGroup)
                _fadeCanvasGroup = this.ValidateComponent<CanvasGroup>();
        }
#endif

        protected virtual void Awake() => ProcessAction = value => _fadeCanvasGroup.alpha = value;

        [System.Obsolete("Use CanvasGroup property instead")]
        public CanvasGroup fadeCanvasGroup => _fadeCanvasGroup;

        public CanvasGroup CanvasGroup => _fadeCanvasGroup;
    }
}