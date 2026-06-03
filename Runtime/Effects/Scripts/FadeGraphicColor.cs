/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using UnityEngine;
using UnityEngine.UI;

namespace Devloader.Effects
{
	[AddComponentMenu("Devloader/Effects/Fade Graphic Color")]
	public class FadeGraphicColor : AbstractEffect
	{
		[SerializeField] Graphic _graphic;
		
		[Space]
        [SerializeField] Color _firstColor = Color.white;
        [SerializeField] Color _finalColor = Color.white;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_graphic && _graphic.color != _firstColor)
				_graphic.color = _firstColor;
        }
#endif

		private void Awake() => ProcessAction = value => _graphic.color = Color.Lerp(_firstColor, _finalColor, value);

		[System.Obsolete("Use Color property instead")]
		public Color color => Color;
		public Color Color => _graphic.color;

        [System.Obsolete("Use Graphic property instead")]
        public Graphic graphic => Graphic;
        public Graphic Graphic => _graphic;
    }
}