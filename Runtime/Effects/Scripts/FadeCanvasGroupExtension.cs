/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using UnityEngine;
using UnityEngine.Events;

using Devloader.Extensions;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace Devloader.Effects.Extensions
{
    [AddComponentMenu("Devloader/Effects/FadeCanvasGroupExtension")]
    public class FadeCanvasGroupExtension : MonoBehaviour
    {
        [Header("Réglages de l'extension")]
        public bool hideOnAwake = true;
        public bool fadeHideOnAwake = false;

        [Header("Réglages de l'effet")]
        [Min(.1f), Tooltip("En seconde")]
        public float duration = 1;

        [Header("Evénements")]
        public UnityEvent OnShow = new UnityEvent();
        public UnityEvent OnHide = new UnityEvent();

        public bool show { get; private set; }

#if UNITY_EDITOR
        private void Reset()
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();

            if (fadeCanvasGroup.events.GetPersistentEventCount() <= 0)
                UnityEventTools.AddPersistentListener(fadeCanvasGroup.events, OnFadeCanvasGroupEvent);

            if (fadeCanvasGroup.duration != duration)
                duration = fadeCanvasGroup.duration;
        }

        private void OnValidate()
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();

            if (duration != fadeCanvasGroup.duration)
                fadeCanvasGroup.duration = duration;
        }
    #endif

        private void Start()
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();

            if (duration != fadeCanvasGroup.duration)
                fadeCanvasGroup.duration = duration;

            if (hideOnAwake)
            {
                if (fadeHideOnAwake)
                    Show(false);
                else
                    Reset(true);
            }
            else
                Reset(false);
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
                    fadeCanvasGroup.fadeCanvasGroup.interactable = false;
                    fadeCanvasGroup.fadeCanvasGroup.blocksRaycasts = false;

                    /*if (effect.direction > 0)
                    {
                        fadeCanvasGroup.fadeCanvasGroup.interactable = true;
                        fadeCanvasGroup.fadeCanvasGroup.blocksRaycasts = true;
                    }
                    else if(effect.direction < 0)
                    {
                        fadeCanvasGroup.fadeCanvasGroup.interactable = false;
                        fadeCanvasGroup.fadeCanvasGroup.blocksRaycasts = false;
                    }*/
                    break;

                case EffectEvent.EventType.Completed:
                    if (effect.direction > 0)
                    {
                        OnShow.Invoke();
                        fadeCanvasGroup.SetToBegin(-1);

                        fadeCanvasGroup.fadeCanvasGroup.interactable = true;
                        fadeCanvasGroup.fadeCanvasGroup.blocksRaycasts = true;
                    }
                    else if (effect.direction < 0)
                    {
                        OnHide.Invoke();
                        fadeCanvasGroup.SetToBegin(1);
                    }
                    break;
            }
        }

        public void Show(bool show)
        {
            if (!isActiveAndEnabled)
            {
                Reset(!show);
                return;
            }

            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();
            fadeCanvasGroup.Run(show ? 1 : -1);

            this.show = show;
        }

        public void Reset(bool show)
        {
            FadeCanvasGroup fadeCanvasGroup = this.ValidateComponent<FadeCanvasGroup>();
            fadeCanvasGroup.fadeCanvasGroup.interactable = !show;
            fadeCanvasGroup.fadeCanvasGroup.blocksRaycasts = !show;

            fadeCanvasGroup.SetToBegin(show ? 1 : -1);
            this.show = !show;
        }
    }
}

/// <summary>
/// Version 20230306
/// </summary>
