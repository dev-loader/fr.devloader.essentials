/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using Devloader.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade LayerGroup Spacing")]
    public class FadeLayerGroupSpacing : AbstractEffect
    {
        [SerializeField] HorizontalOrVerticalLayoutGroup _layoutGroup;

        [SerializeField] int _initial;
        [SerializeField] int _final;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!_layoutGroup)
                _layoutGroup = this.ValidateComponent<HorizontalOrVerticalLayoutGroup>();
        }
#endif

        private void Awake() => ProcessAction = value => _layoutGroup.spacing = Mathf.Lerp(_initial, _final, value);
    }
}