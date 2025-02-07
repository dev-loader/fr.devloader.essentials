/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using UnityEngine;
using Devloader.Extensions;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/FadeCanvasGroup")]
    public class FadeCanvasGroup : AbstractEffect
    {
        public EffectProgressEvent progressEvent = new EffectProgressEvent();

        [Space]
        [Header("CanvasGroup o� appliquer le fondu en alpha")]
        public CanvasGroup fadeCanvasGroup;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!fadeCanvasGroup)
                fadeCanvasGroup = this.ValidateComponent<CanvasGroup>();

            base.OnValidate();
        }
#endif

        protected virtual void Awake()
        {
            if (!fadeCanvasGroup)
                fadeCanvasGroup = this.ValidateComponent<CanvasGroup>();

            processAction = delegate (float value)
            { fadeCanvasGroup.alpha = value; };
        }
    }
}