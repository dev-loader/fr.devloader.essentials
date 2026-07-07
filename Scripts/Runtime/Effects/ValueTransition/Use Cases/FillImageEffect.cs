/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

using UnityEngine;
using UnityEngine.UI;

using Devloader.Extensions;

namespace Devloader.Effects
{
    [System.Obsolete("Use FadeImageFill instead")]
    [AddComponentMenu("Devloader/Effects/FillImageEffect")]
    public class FillImageEffect : AbstractEffect
    {
        [Header("Image où appliquer le fondu en alpha")]
        public Image fillImage;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!fillImage)
                fillImage = this.ValidateComponent<Image>();

            base.OnValidate();
        }
#endif

        private void Awake()
        {
            if (!fillImage)
                fillImage = this.ValidateComponent<Image>();

            processAction = delegate (float value)
            { fillImage.fillAmount = value; };
        }
    }
}