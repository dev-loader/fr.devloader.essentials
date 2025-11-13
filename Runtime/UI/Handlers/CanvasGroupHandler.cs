/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

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
        [SerializeField, Tooltip("Used to identify the group in the Dev_CanvasHandler")] string _groupName = "";
        [SerializeField] VisibilityOnStart _visibilityOnStart = VisibilityOnStart.Hide;
        [Space]

        [Header("Transition settings")]
        [SerializeField] TransitionEffect _showTransition = TransitionEffect.Fade;
        [SerializeField] float _showTransitionDuration = .2f;

        [SerializeField] TransitionEffect _hideTransition = TransitionEffect.Fade;
        [SerializeField] float _hideTransitionDuration = .2f;
        [Space]

        [Header("Events")]
        [SerializeField] UnityEvent _onShow = new UnityEvent();
        [SerializeField] UnityEvent _onHide = new UnityEvent();

        [Header("For debug purposes")]
        [SerializeField] CanvasGroup _canvasGroup;

        #endregion

        AbstractEffect _showEffect;
        AbstractEffect _hideEffect;

        bool _started;

        [System.Obsolete("Use hidden instead")]
        public bool Hidden { get; private set; } = true;
        public bool hidden { get; private set; } = true;

#if UNITY_EDITOR
        private void OnValidate() => InitComponent();

        private void Reset() => InitComponent();
#endif

        private void Awake()
        {
            InitComponent();
            InitEffects();

            CanvasHandler.instance.AddCanvas(_groupName, this);
        }

        private void OnEnable()
        {
            if (_started)
                Show();

            _showEffect.events.AddListener(OnEffectEvent);
            _hideEffect.events.AddListener(OnEffectEvent);
        }

        private void Start()
        {
            if (_visibilityOnStart == VisibilityOnStart.Show)
                Show();

            _started = true;
        }

        private void OnDisable() => Hide(false);

        private void OnDestroy()
        {
            if(CanvasHandler.HasInstance)
                CanvasHandler.instance.RemoveCanvas(_groupName);
        }

        private bool GroupNameAlreadyExists(string groupName)
        {
            List<CanvasGroupHandler> groups = new List<CanvasGroupHandler>(ComponentExtension.FindAll<CanvasGroupHandler>());
            CanvasGroupHandler group = groups.Find(group => group._groupName == groupName);

            return group;
        }

        public void Hide(bool effect = true)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _hideEffect?.SetToBegin(-1);

            if(effect)
                _hideEffect?.Run(-1);
        }

        private void InitComponent()
        {
            if (!_canvasGroup)
                _canvasGroup = this.ValidateComponent<CanvasGroup>();

            if(_groupName.Trim().Length <= 0)
            {
                int instanceIndex = (new List<CanvasGroupHandler>(ComponentExtension.FindAll<CanvasGroupHandler>())).IndexOf(this);

                while (GroupNameAlreadyExists("Group " + (++instanceIndex))) ;
                _groupName = "Group " + instanceIndex;
            }

#if UNITY_EDITOR
            if(Application.isPlaying)
            {
                _canvasGroup.interactable = !Application.IsPlaying(gameObject);
                _canvasGroup.blocksRaycasts = !Application.IsPlaying(gameObject);
            }
#else
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
#endif
        }

        private void InitEffects()
        {
            _showEffect = InitEffect(_showTransition, _showTransitionDuration);
            _hideEffect = InitEffect(_hideTransition, _hideTransitionDuration);
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
                    effect = this.ValidateComponent<FadeTransformScale>();

                    effect.firstValue = 1;
                    effect.finalValue = 0;
                    break;

                case TransitionEffect.ZoomOut:
                    effect = this.ValidateComponent<FadeTransformScale>();

                    effect.firstValue = 0;
                    effect.finalValue = 1;
                    break;
            }

            effect.duration = duration;

            if(_visibilityOnStart != VisibilityOnStart.LeaveItAsItIs)
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
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = false;
                    break;

                case EffectEvent.EventType.Completed:
                    if (effect.direction > 0)
                    {
                        _onShow.Invoke();

                        _canvasGroup.interactable = true;
                        _canvasGroup.blocksRaycasts = true;

                        hidden = false;
                    }
                    else if (effect.direction < 0)
                    {
                        _onHide.Invoke();
                        hidden = true;
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
                _hideEffect.SetToBegin(-1);

                _showEffect.SetToBegin(1);
                _showEffect.Run(1);
            }
        }
    }

}
