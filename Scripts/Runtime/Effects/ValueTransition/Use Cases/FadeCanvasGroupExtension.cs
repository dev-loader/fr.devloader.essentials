/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using UnityEngine;
using UnityEngine.Events;

using Devloader.Extensions;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace Devloader.Effects.Extensions
{
    [AddComponentMenu("Devloader/Effects/Fade CanvasGroup Extension")]
    public class FadeCanvasGroupExtension : MonoBehaviour
    {
        [Header("Réglages de l'extension")]
        [SerializeField] bool _hideOnAwake = true;
        [SerializeField] bool _fadeHideOnAwake = false;

        [Header("Réglages de l'effet")]
        [Min(.1f), Tooltip("En seconde")]
        [SerializeField] float _duration = 1;

        [Header("Evénements")]
        [SerializeField] UnityEvent _onShow = new UnityEvent();
        [SerializeField] UnityEvent _onHide = new UnityEvent();

        [System.Obsolete("Use Display property instead")]
        public bool show { get => Display; private set => Display = value; }
        public bool Display { get; private set; }

#if UNITY_EDITOR
        private void Reset()
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();

            if (fadeCanvasGroup.Events.GetPersistentEventCount() <= 0)
                UnityEventTools.AddPersistentListener(fadeCanvasGroup.Events, OnFadeCanvasGroupEvent);

            if (fadeCanvasGroup.Duration != _duration)
                _duration = fadeCanvasGroup.Duration;
        }

        private void OnValidate()
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();

            if (_duration != fadeCanvasGroup.Duration)
                fadeCanvasGroup.Duration = _duration;
        }
    #endif

        private void Start()
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();

            if (_duration != fadeCanvasGroup.Duration)
                fadeCanvasGroup.Duration = _duration;

            if (_hideOnAwake)
            {
                if (_fadeHideOnAwake)
                    Show(false);
                else
                    ResetCanvasGroup(true);
            }
            else
                ResetCanvasGroup(false);
        }

        public void OnFadeCanvasGroupEvent(AbstractEffect effect, EffectEvent.EventType eventType)
        {
            if (!enabled)
                return;

            if (effect is not FadeCanvasGroup)
                return;

            FadeCanvasGroup fadeCanvasGroup = effect as FadeCanvasGroup;

            switch (eventType)
            {
                case EffectEvent.EventType.Started:
                    fadeCanvasGroup.CanvasGroup.interactable = false;
                    fadeCanvasGroup.CanvasGroup.blocksRaycasts = false;
                    break;

                case EffectEvent.EventType.Completed:
                    if (effect.Direction > 0)
                    {
                        _onShow.Invoke();
                        fadeCanvasGroup.SetToBegin(-1);

                        fadeCanvasGroup.CanvasGroup.interactable = true;
                        fadeCanvasGroup.CanvasGroup.blocksRaycasts = true;
                    }
                    else if (effect.Direction < 0)
                    {
                        _onHide.Invoke();
                        fadeCanvasGroup.SetToBegin(1);
                    }
                    break;
            }
        }

        public void Show(bool show)
        {
            if (!isActiveAndEnabled)
            {
                ResetCanvasGroup(!show);
                return;
            }

            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();
            fadeCanvasGroup.Run(show ? 1 : -1);

            Display = show;
        }

        public void ResetCanvasGroup(bool show)
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();
            fadeCanvasGroup.CanvasGroup.interactable = !show;
            fadeCanvasGroup.CanvasGroup.blocksRaycasts = !show;

            fadeCanvasGroup.SetToBegin(show ? 1 : -1);
            Display = !show;
        }
    }
}
