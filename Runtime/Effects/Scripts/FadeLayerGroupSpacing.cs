using Devloader.Effects;
using UnityEngine;
using UnityEngine.UI;

public class FadeLayerGroupSpacing : AbstractEffect
{
    [SerializeField] HorizontalOrVerticalLayoutGroup _layoutGroup;

    [SerializeField] int _initial;
    [SerializeField] int _final;

    private void Awake() => processAction = value => _layoutGroup.spacing = Mathf.Lerp(_initial, _final, value);
}
