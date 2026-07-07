/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 202502019

using Devloader.Utils;

using UnityEngine;

namespace Devloader.UI
{
    public class UIAutoDetectCamera : MonoBehaviour
    {
        Canvas _canvas;

        private void OnEnable()
        {
            if (TryGetComponent(out _canvas) && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                _canvas.worldCamera = CameraUtils.Active;
        }

        private void Update()
        {
            if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay || _canvas.worldCamera == CameraUtils.Active)
                return;
            else
                _canvas.worldCamera = CameraUtils.Active;
        }
    }
}
