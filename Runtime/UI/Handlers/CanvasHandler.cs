/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

using Devloader.Extensions;

using System.Collections.Generic;

using UnityEngine;

namespace Devloader.UI.Handlers
{
    [AddComponentMenu("Devloader/UI/Handlers/CanvasHandler")]
    public class CanvasHandler : MonoBehaviour
    {
        public enum PositionMethod
        {
            ScreenSpaceOverlay = 0,
            ScreenCamera = 1,
            WorldSpaceEyeLevel = 2,
            WorldSpaceCameraDirection = 3
        }

        static CanvasHandler _instance;

        #region Inspector

        [SerializeField, Tooltip("Set the canvas at the same height that the camera")] PositionMethod _positionMethod = PositionMethod.ScreenSpaceOverlay;
        [SerializeField, Tooltip("Distance between the camera and the canvas for world space placement")] float _cameraDistance = 1;

        [Space, SerializeField] AudioClip _showSoundFX;
        [SerializeField] AudioClip _hideSoundFX;

        #endregion

        #region Privates

        Dictionary<string, CanvasGroupHandler> _groups = new Dictionary<string, CanvasGroupHandler>();
        AudioSource _audioSource = null;

        #endregion


        [System.Obsolete("Use instance instead")]
        public static CanvasHandler Instance { get => _instance ? _instance : ComponentExtension.FindFirst<CanvasHandler>(true, true); }
        public static CanvasHandler instance { get => _instance ? _instance : ComponentExtension.FindFirst<CanvasHandler>(true, true); }

        public static bool HasInstance { get => _instance; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_positionMethod == PositionMethod.ScreenSpaceOverlay || _positionMethod == PositionMethod.ScreenCamera)
                _cameraDistance = 0;
            else if(_cameraDistance < (Camera.main??Camera.current).nearClipPlane)
                _cameraDistance = Camera.main.nearClipPlane;

            ValidateAudioSource();
        }
#endif

        private void Awake()
        {
            if (!_instance)
                _instance = this;
            else if (_instance != this)
                this.HardDestroy();
        }

        private void OnEnable() => ValidateAudioSource();

        public void AddCanvas(string name, CanvasGroupHandler canvas)
        {
            if(!_groups.ContainsKey(name))
                _groups.Add(name, canvas);
        }

        public void Hide(string name)
        {
            if (_groups.ContainsKey(name))
                Hide(_groups[name]);
        }

        private void Hide(CanvasGroupHandler handler)
        {
            handler.Hide();

            if (_hideSoundFX)
            {
                _audioSource.clip = _hideSoundFX;
                _audioSource.Play();
            }
        }

        public void HideAll()
        {
            foreach (KeyValuePair<string, CanvasGroupHandler> pair in _groups)
                if(!pair.Value.Hidden)
                    pair.Value.Hide();
        }

        public void RemoveCanvas(string name)
        {
            if (_groups.ContainsKey(name))
                _groups.Remove(name);
        }

        public void Show(string name)
        {
            if (_groups.ContainsKey(name))
                Show(_groups[name]);
        }

        private void Show(CanvasGroupHandler handler)
        {
            Transform cameraTransform = Camera.main ? Camera.main.transform : Camera.current.transform;

            if (cameraTransform)
            {
                Vector3 position = cameraTransform.position + cameraTransform.forward * _cameraDistance;

                if (_positionMethod == PositionMethod.WorldSpaceEyeLevel)
                    position.y = cameraTransform.position.y;

                transform.position = position;

                Vector3 target = cameraTransform.position;

                if (_positionMethod == PositionMethod.WorldSpaceEyeLevel)
                    target.y = position.y;

                transform.LookAt(target);
                transform.Rotate(Vector3.up * 180);
            }

            handler.Show();

            if (_showSoundFX)
            {
                _audioSource.clip = _showSoundFX;
                _audioSource.Play();
            }
        }

        public void Toggle(string name)
        {
            if(_groups.ContainsKey(name))
            {
                if (_groups[name].Hidden)
                    Show(_groups[name]);
                else
                    Hide(_groups[name]);
            }
        }

        private void ValidateAudioSource()
        {
            if (!_audioSource && (_showSoundFX || _hideSoundFX) && !TryGetComponent(out _audioSource))
                _audioSource = this.ValidateComponent<AudioSource>();
            else if (!_showSoundFX && !_hideSoundFX && _audioSource)
                Destroy(_audioSource);
        }
    }
}