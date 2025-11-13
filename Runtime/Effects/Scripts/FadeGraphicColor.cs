/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

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

		private void Awake()
		{
			if (!_graphic)
				return;

			processAction = delegate (float value)
			{
				Color color = Color.Lerp(_firstColor, _finalColor, value);
				_graphic.color = color;
			};
        }

		public Color color => _graphic.color;

        public Graphic graphic => _graphic;
    }
}

/// <summary>
/// Version 20230212
/// </summary>