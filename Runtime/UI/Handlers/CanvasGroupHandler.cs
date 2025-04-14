/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250414

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Devloader.Effects;
using Devloader.Extensions;

namespace Devloader.UI.Handlers
{
    [AddComponentMenu("Devloader/UI/Handlers/CanvasGroupHandler")]
    public class CanvasGroupHandler : MonoBehaviour
    {
        public enum TransitionEffect
        {
            None = 0,
            DragLeft = 1,
            DragRight = 2,
            DragForward = 3,
            DragBackward = 4,
            Fade = 5,
            ZoomIn = 6,
            ZoomOut = 7,
        }

        public enum VisibilityOnStart
        {
            LeaveItAsItIs = 0,
            Show = 1,
            Hide = 2
        }

        #region Inspector

        [Header("Main settings")]
        [SerializeField, Tooltip("Used to identify the group in the Dev_CanvasHandler")] string groupName = "";
        [SerializeField] VisibilityOnStart visibilityOnStart = VisibilityOnStart.Hide;
        [Space]

        [Header("Transition settings")]
        [SerializeField] TransitionEffect showTransition = TransitionEffect.Fade;
        [SerializeField] float showTransitionDuration = .2f;

        [SerializeField] TransitionEffect hideTransition = TransitionEffect.Fade;
        [SerializeField] float hideTransitionDuration = .2f;
        [Space]

        [Header("Events")]
        [SerializeField] UnityEvent OnShow = new UnityEvent();
        [SerializeField] UnityEvent OnHide = new UnityEvent();

        [Header("For debug purposes")]
        [SerializeField] CanvasGroup canvasGroup;

        #endregion

        AbstractEffect showEffect;
        AbstractEffect hideEffect;

        bool started;

        public bool Hidden { get; private set; } = true;

#if UNITY_EDITOR
        private void OnValidate() => InitComponent();

        private void Reset() => InitComponent();
#endif

        private void Awake()
        {
            InitComponent();
            InitEffects();

            CanvasHandler.Instance.AddCanvas(groupName, this);
        }

        private void OnEnable()
        {
            if (started)
                Show();

            showEffect.events.AddListener(OnEffectEvent);
            hideEffect.events.AddListener(OnEffectEvent);
        }

        private void Start()
        {
            if (visibilityOnStart == VisibilityOnStart.Show)
                Show();

            started = true;
        }

        private void OnDisable() => Hide(false);

        private void OnDestroy()
        {
            if(CanvasHandler.HasInstance)
                CanvasHandler.Instance.RemoveCanvas(groupName);
        }

        private bool GroupNameAlreadyExists(string groupName)
        {
            List<CanvasGroupHandler> groups = new List<CanvasGroupHandler>(FindObjectsOfType<CanvasGroupHandler>());
            CanvasGroupHandler group = groups.Find(group => group.groupName == groupName);

            return group;
        }

        public void Hide(bool effect = true)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            hideEffect?.SetToBegin(-1);

            if(effect)
                hideEffect?.Run(-1);
        }

        private void InitComponent()
        {
            if (!canvasGroup)
                canvasGroup = this.ValidateComponent<CanvasGroup>();

            if(groupName.Trim().Length <= 0)
            {
                int instanceIndex = (new List<CanvasGroupHandler>(FindObjectsOfType<CanvasGroupHandler>())).IndexOf(this);

                while (GroupNameAlreadyExists("Group " + (++instanceIndex))) ;
                groupName = "Group " + instanceIndex;
            }

#if UNITY_EDITOR
            if(Application.isPlaying)
            {
                canvasGroup.interactable = !Application.IsPlaying(gameObject);
                canvasGroup.blocksRaycasts = !Application.IsPlaying(gameObject);
            }
#else
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
#endif
        }

        private void InitEffects()
        {
            showEffect = InitEffect(showTransition, showTransitionDuration);
            hideEffect = InitEffect(hideTransition, hideTransitionDuration);
        }

        private AbstractEffect InitEffect(TransitionEffect transition, float duration)
        {
            AbstractEffect effect = null;

            switch (transition)
            {
                case TransitionEffect.Fade:
                    effect = this.ValidateComponent<FadeCanvasGroup>();

                    effect.firstValue = 0;
                    effect.finalValue = 1;
                    break;

                case TransitionEffect.DragLeft:
                case TransitionEffect.DragRight:
                case TransitionEffect.DragForward:
                case TransitionEffect.DragBackward:

                case TransitionEffect.ZoomIn:
                    effect = this.ValidateComponent<UniformRescaleEffect>();

                    effect.firstValue = 1;
                    effect.finalValue = 0;
                    break;

                case TransitionEffect.ZoomOut:
                    effect = this.ValidateComponent<UniformRescaleEffect>();

                    effect.firstValue = 0;
                    effect.finalValue = 1;
                    break;
            }

            effect.duration = duration;

            if(visibilityOnStart != VisibilityOnStart.LeaveItAsItIs)
                effect.SetToBegin(1);

            return effect;
        }

        public void OnEffectEvent(AbstractEffect effect, EffectEvent.EventType eventType)
        {
            if (!enabled)
                return;

            switch (eventType)
            {
                case EffectEvent.EventType.Started:
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    break;

                case EffectEvent.EventType.Completed:
                    if (effect.direction > 0)
                    {
                        OnShow.Invoke();

                        canvasGroup.interactable = true;
                        canvasGroup.blocksRaycasts = true;

                        Hidden = false;
                    }
                    else if (effect.direction < 0)
                    {
                        OnHide.Invoke();
                        Hidden = true;
                    }

                    break;
            }
        }

        public void Show(bool show = true)
        {
            if (!show)
                Hide();
            else
            {
                hideEffect.SetToBegin(-1);

                showEffect.SetToBegin(1);
                showEffect.Run(1);
            }
        }
    }

}
