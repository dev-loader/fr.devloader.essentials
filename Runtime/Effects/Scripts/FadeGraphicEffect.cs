/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using UnityEngine;
using UnityEngine.UI;

using Devloader.Extensions;

namespace Devloader.Effects
{
	[AddComponentMenu("Devloader/Effects/FadeGraphicEffect")]
	public class FadeGraphicEffect : AbstractEffect
	{
		[Header("Graphic où appliquer le fondu en alpha")]
		public Graphic graphic;
		public Color mask = Color.white;

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			if (!graphic)
				graphic = this.ValidateComponent<Graphic>();

			base.OnValidate();
		}
#endif

		private void Awake()
		{
			if (!graphic)
				graphic = this.ValidateComponent<Graphic>();

			processAction = delegate (float value)
			{
				Color color = mask * value;
				graphic.color = color;
			};
		}
	}
}

/// <summary>
/// Version 20230212
/// </summary>