/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

using UnityEngine;
using UnityEngine.UI;

using Devloader.Extensions;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/FillImageEffect")]
    public class FadeImageFill : AbstractEffect
    {
        [SerializeField] Image _fillImage;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            this.ValidateComponent(out _fillImage);
        }
#endif

        private void Awake()
        {
            this.ValidateComponent(out _fillImage);

            processAction = delegate (float value)
            { _fillImage.fillAmount = value; };
        }

        public Image fillImage => _fillImage;
    }
}