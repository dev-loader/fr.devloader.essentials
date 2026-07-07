/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using Devloader.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade LayerGroup Padding")]
    public class FadeLayerGroupPadding : AbstractEffect
    {
        [SerializeField] LayoutGroup _layoutGroup;

        [SerializeField] RectOffset _initial;
        [SerializeField] RectOffset _final;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!_layoutGroup)
                _layoutGroup = this.ValidateComponent<LayoutGroup>();
        }
#endif

        private void Awake() => ProcessAction = value => _layoutGroup.padding = new RectOffset(
            (int)Mathf.Lerp(_initial.left, _final.left, value),
            (int)Mathf.Lerp(_initial.right, _final.right, value),
            (int)Mathf.Lerp(_initial.top, _final.top, value),
            (int)Mathf.Lerp(_initial.bottom, _final.bottom, value)
        );
    }
}