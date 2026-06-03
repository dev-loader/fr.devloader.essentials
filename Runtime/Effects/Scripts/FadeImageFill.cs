/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using UnityEngine;
using UnityEngine.UI;

using Devloader.Extensions;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade Image Fill")]
    public class FadeImageFill : AbstractEffect
    {
        [SerializeField] Image _fillImage;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if(!_fillImage)
                this.ValidateComponent(out _fillImage);
        }
#endif

        private void Awake() => ProcessAction = value=> _fillImage.fillAmount = value;

        [System.Obsolete("Use ImageComponent property instead")]
        public Image fillImage => ImageComponent;

        public Image ImageComponent => _fillImage;
    }
}