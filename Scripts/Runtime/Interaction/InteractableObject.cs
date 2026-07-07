/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240821

using Devloader.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Devloader.Interaction
{
    [AddComponentMenu("Devloader/Interaction/InteractableObject")]
    public class InteractableObject : MonoBehaviour
    {
        public enum EventCondition
        {
            ByColliderCollision,
            ByColliderTrigger,
            ByDistance,
            ByPointer,
        }

        public enum EventTrigger
        {
            KeyOrInputAction,
            OnEnter,
            OnExit,
        }


        public enum InteractionMethod
        {
            ByCollider,
            ByDistance,
            ByPointer,
        }

        public enum InteractionTrigger
        {
            KeyOrInputAction,
            OnEnter,
            OnExit,
        }

        public enum ActionState
        {
            Started,
            Performed,
            Canceled
        }

        [Header("Main settings")]
        [SerializeField] private InteractionMethod _interactionMethod = InteractionMethod.ByCollider;
        [SerializeField] private InteractionTrigger _interactionTrigger = InteractionTrigger.KeyOrInputAction;
        [Space]

        [SerializeField] private ActionState _stateOnConditionFulfilled = ActionState.Canceled;
#if ENABLE_INPUT_SYSTEM
        [SerializeField] private InputActionReference _pressActionReference;
#else
        [SerializeField] private KeyCode _interactionKey = KeyCode.Mouse0;
#endif

        [Header("Collider method settings")]
        [SerializeField] LayerMask _colliderLayerMask;

        [Header("Distance method settings")]
        [SerializeField] private float _triggerDistance = 1;
        [SerializeField] private GameObject _overrideDistanceGameObjectSource = null;
        [Space]

        [Header("Pointer method settings")]
        [SerializeField] LayerMask _raycastLayerMask;
        [SerializeField] private GameObject _overridePointer = null;
        [Space]

#if ENABLE_INPUT_SYSTEM
        [SerializeField] private InputActionReference _pressPositionActionReference;
#else
        [SerializeField] private KeyCode _interactionKey = KeyCode.Mouse0;
#endif

        [Header("Event settings")]
        [Space, SerializeField] private UnityEvent<bool> _onConditionFulfilled = new UnityEvent<bool>();

        private UnityEvent _onPressStarted = new UnityEvent();
        [Space, SerializeField] private UnityEvent _onPressPerformed = new UnityEvent();
        private UnityEvent _onPressCanceled = new UnityEvent();

        private bool _isEntered = false;

        private Vector2 _pointerPosition;
        private float _distance;
        private RaycastHit _pointerHit;

        private List<InteractableColliderHandler> _colliderHandlers = new List<InteractableColliderHandler>();

#if ENABLE_INPUT_SYSTEM
        private void OnEnable()
        {
            if(_pressActionReference)
                _pressActionReference.action.Enable();

            if(_pressPositionActionReference)
                _pressPositionActionReference.action.Enable();
        }

        private void OnDisable()
        {
            if (_pressActionReference)
                _pressActionReference.action.Disable();

            if (_pressPositionActionReference)
                _pressPositionActionReference.action.Disable();
        }

        private void Start()
        {
            if(_interactionMethod == InteractionMethod.ByCollider)
            {
                Collider collider;

                if (!TryGetComponent(out collider))
                {
                    if (this.TryFindComponents(out Collider[] colliders, true))
                        foreach (Collider c in colliders)
                        {
                            InteractableColliderHandler handler = c.ValidateComponent<InteractableColliderHandler>();

                            if (_interactionTrigger == InteractionTrigger.OnEnter)
                                handler.OnEntered.AddListener(isEntered => _onPressPerformed.Invoke());
                            else if(_interactionTrigger == InteractionTrigger.OnExit)
                                handler.OnExit.AddListener(isEntered => _onPressPerformed.Invoke());
                            else if(_interactionTrigger == InteractionTrigger.KeyOrInputAction)
                            {
                                handler.OnEntered.AddListener(ColliderTriggerHandler);
                            }

                            _colliderHandlers.Add(handler);
                        }
                }
                else
                    _colliderHandlers = new() { this.ValidateComponent<InteractableColliderHandler>() };
            }

            if(_pressActionReference && _interactionTrigger == InteractionTrigger.KeyOrInputAction)
                switch (_stateOnConditionFulfilled)
                {
                    case ActionState.Started:
                        _pressActionReference.action.started += HandlePressAction;
                        break;

                    case ActionState.Performed:
                        _pressActionReference.action.performed += HandlePressAction;
                        break;

                    case ActionState.Canceled:
                        _pressActionReference.action.canceled += HandlePressAction;
                        break;
                }

            if(_pressPositionActionReference)
                _pressPositionActionReference.action.performed += HandlePositionAction;
        }
#endif

        private void FixedUpdate()
        {
            switch(_interactionMethod)
            {
                case InteractionMethod.ByPointer:
                    if (_overridePointer)
                        _pointerPosition = Camera.WorldToScreenPoint(_overridePointer.transform.position);
#if !ENABLE_INPUT_SYSTEM
                    else
                        _pointerPosition = Input.mousePosition;
#endif
                    break;

                case InteractionMethod.ByDistance:
                    Transform source = _overrideDistanceGameObjectSource ? _overrideDistanceGameObjectSource.transform : Camera.transform;
                    _distance = Vector3.Distance(source.position, transform.position);
                    break;
            }
        }

        private void Update()
        {
            switch (_interactionMethod)
            {
                case InteractionMethod.ByCollider:
#if !ENABLE_INPUT_SYSTEM
                    if(_interactionTrigger == InteractionTrigger.KeyOrInputAction && _isEntered && Input.GetKeyUp(_interactionKey))
                        _onInteractionPerformed.Invoke();
#endif
                    break;

                case InteractionMethod.ByDistance:
                    if (_distance <= _triggerDistance)
                    {
                        _onConditionFulfilled.Invoke(true);

                        if (_interactionTrigger == InteractionTrigger.OnEnter && !_isEntered)
                            _onPressPerformed.Invoke();
#if !ENABLE_INPUT_SYSTEM
                        else if (_interactionTrigger == InteractionTrigger.KeyOrInputAction && Input.GetKeyUp(_interactionKey))
                            _onInteractionPerformed.Invoke();
#endif

                        _isEntered = true;
                    }
                    else
                    {
                        _onConditionFulfilled.Invoke(false);

                        if (_interactionTrigger == InteractionTrigger.OnExit && _isEntered)
                            _onPressPerformed.Invoke();

                        _isEntered = false;
                    }

                    break;

                case InteractionMethod.ByPointer:
#if ENABLE_INPUT_SYSTEM
#elif !UNITY_ANDROID || !UNITY_IOS
                    if (Mouse.current is null)
                    {
                        _isEntered = false;
                        return;
                    }
#else
                    if (Touchscreen.current is null || Touchscreen.current.touches.Count <= 0)
                    {
                        if (_interactionAction == InteractionTrigger.OnExit && _isEntered)
                            _onInteractionPerformed.Invoke();

                        _isEntered = false;
                        return;
                    }
#endif

                    if (
                        Physics.Raycast(
                            Camera.ScreenPointToRay(_pointerPosition),
                            out _pointerHit,
                            Camera.farClipPlane,
                            _raycastLayerMask
                        ) &&
                        (_pointerHit.collider.gameObject == gameObject || _pointerHit.collider.transform.IsChildOf(transform))
                    )
                    {
                        _onConditionFulfilled.Invoke(true);

                        if (_interactionTrigger == InteractionTrigger.OnEnter && !_isEntered)
                            _onPressPerformed.Invoke();
#if !ENABLE_INPUT_SYSTEM
                        else if (_interactionTrigger == InteractionTrigger.KeyOrInputAction && Input.GetKeyUp(_interactionKey))
                            _onInteractionPerformed.Invoke();
#endif
                        _isEntered = true;
                    }
                    else
                    {
                        _onConditionFulfilled.Invoke(false);

                        if (_interactionTrigger == InteractionTrigger.OnExit && _isEntered)
                            _onPressPerformed.Invoke();

                        _isEntered = false;
                    }

                    break;
            }
        }

        private void ColliderTriggerHandler(InteractableColliderEventData eventData) => _isEntered = eventData.isInCollider && _colliderLayerMask.Includes(eventData.other.gameObject.layer) && isActiveAndEnabled;

#if ENABLE_INPUT_SYSTEM
        private void HandlePressAction(InputAction.CallbackContext context)
        {
            if(isActiveAndEnabled && _isEntered)
                _onPressPerformed.Invoke();
        }

        private void HandlePositionAction(InputAction.CallbackContext context) => _pointerPosition = context.ReadValue<Vector2>();
#endif

        private Camera Camera { get => Camera.main ?? Camera.current; }
    }
}