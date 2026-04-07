using Devloader.Effects;
using UnityEngine;
using UnityEngine.UI;

public class FadeLayerGroupPadding : AbstractEffect
{
    [SerializeField] LayoutGroup _layoutGroup;

    [SerializeField] RectOffset _initial;
    [SerializeField] RectOffset _final;

    private void Awake() => processAction = value => _layoutGroup.padding = new RectOffset(
        (int) Mathf.Lerp(_initial.left, _final.left, value),
        (int)Mathf.Lerp(_initial.right, _final.right, value),
        (int)Mathf.Lerp(_initial.top, _final.top, value),
        (int)Mathf.Lerp(_initial.bottom, _final.bottom, value)
    );
}
