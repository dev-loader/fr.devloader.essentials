/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using Devloader.Extensions;
using Devloader.Utils;
using UnityEngine;

namespace Devloader.UI
{
    public class UIFollowGameObject : MonoBehaviour
    {
        [SerializeField] GameObject _gameObject;
        Canvas _canvas;

        public GameObject gameobject
        {
            get => _gameObject;
            set => _gameObject = value;
        }

        private void Awake()
        {
            _canvas = this.FindComponentInParent<Canvas>();
        }

        private void FixedUpdate()
        {
            if (_gameObject)
            {
                switch(_canvas.renderMode)
                {
                    case RenderMode.ScreenSpaceOverlay:
                        transform.Translate(GetOverlayScreenspacePosition(_gameObject) - transform.position);
                        break;

                    default:
                        transform.position = GetCameraScreenspacePosition(_gameObject, _canvas.transform.position, CameraUtils.Active.transform.position);
                        break;
                }
            }
        }

        Vector3 GetOverlayScreenspacePosition(GameObject gameObject) => CameraUtils.Active.WorldToScreenPoint(_gameObject.transform.position);

        Vector3 GetCameraScreenspacePosition(GameObject gameObject, Vector3 canvasPosition, Vector3 cameraPosition) => GetWorldScreenspacePosition(gameObject, canvasPosition, cameraPosition);

        Vector3 GetWorldScreenspacePosition(GameObject gameObject, Vector3 canvasPosition, Vector3 cameraPosition)
        {
            Vector3 objectPositionInScreenSpace = CameraUtils.Active.WorldToScreenPoint(_gameObject.transform.position);
            objectPositionInScreenSpace.z = (canvasPosition - cameraPosition).magnitude;

            return CameraUtils.Active.ScreenToWorldPoint(objectPositionInScreenSpace);
        }
    }
}
