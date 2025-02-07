/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240525

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

        static CanvasHandler instance;

        #region Inspector

        [SerializeField, Tooltip("Set the canvas at the same height that the camera")] PositionMethod positionMethod = PositionMethod.ScreenSpaceOverlay;
        [SerializeField, Tooltip("Distance between the camera and the canvas for world space placement")] float cameraDistance = 1;

        [Space, SerializeField] AudioClip showSoundFX;
        [SerializeField] AudioClip hideSoundFX;

        #endregion

        #region Privates

        Dictionary<string, CanvasGroupHandler> groups = new Dictionary<string, CanvasGroupHandler>();
        AudioSource audioSource = null;

        #endregion

        public static CanvasHandler Instance { get => instance ? instance : ComponentExtension.FindObject<CanvasHandler>(true, true); }

        public static bool HasInstance { get => instance; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (positionMethod == PositionMethod.ScreenSpaceOverlay || positionMethod == PositionMethod.ScreenCamera)
                cameraDistance = 0;
            else if(cameraDistance < (Camera.main??Camera.current).nearClipPlane)
                cameraDistance = Camera.main.nearClipPlane;

            ValidateAudioSource();
        }
#endif

        private void Awake()
        {
            if (!instance)
                instance = this;
            else if (instance != this)
                this.HardDestroy();
        }

        private void OnEnable() => ValidateAudioSource();

        public void AddCanvas(string name, CanvasGroupHandler canvas)
        {
            if(!groups.ContainsKey(name))
                groups.Add(name, canvas);
        }

        public void Hide(string name)
        {
            if (groups.ContainsKey(name))
                Hide(groups[name]);
        }

        private void Hide(CanvasGroupHandler handler)
        {
            handler.Hide();

            if (hideSoundFX)
            {
                audioSource.clip = hideSoundFX;
                audioSource.Play();
            }
        }

        public void HideAll()
        {
            foreach (KeyValuePair<string, CanvasGroupHandler> pair in groups)
                if(!pair.Value.Hidden)
                    pair.Value.Hide();
        }

        public void RemoveCanvas(string name)
        {
            if (groups.ContainsKey(name))
                groups.Remove(name);
        }

        public void Show(string name)
        {
            if (groups.ContainsKey(name))
                Show(groups[name]);
        }

        private void Show(CanvasGroupHandler handler)
        {
            Transform cameraTransform = Camera.main ? Camera.main.transform : Camera.current.transform;

            if (cameraTransform)
            {
                Vector3 position = cameraTransform.position + cameraTransform.forward * cameraDistance;

                if (positionMethod == PositionMethod.WorldSpaceEyeLevel)
                    position.y = cameraTransform.position.y;

                transform.position = position;

                Vector3 target = cameraTransform.position;

                if (positionMethod == PositionMethod.WorldSpaceEyeLevel)
                    target.y = position.y;

                transform.LookAt(target);
                transform.Rotate(Vector3.up * 180);
            }

            handler.Show();

            if (showSoundFX)
            {
                audioSource.clip = showSoundFX;
                audioSource.Play();
            }
        }

        public void Toggle(string name)
        {
            if(groups.ContainsKey(name))
            {
                if (groups[name].Hidden)
                    Show(groups[name]);
                else
                    Hide(groups[name]);
            }
        }

        private void ValidateAudioSource()
        {
            if (!audioSource && (showSoundFX || hideSoundFX) && !TryGetComponent(out audioSource))
                audioSource = this.ValidateComponent<AudioSource>();
            else if (!showSoundFX && !hideSoundFX && audioSource)
                Destroy(audioSource);
        }
    }
}