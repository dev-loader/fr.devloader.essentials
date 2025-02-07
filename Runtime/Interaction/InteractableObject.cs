/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240621

using Devloader.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

namespace Devloader.Interaction
{
    [AddComponentMenu("Devloader/Interaction/InteractableObject")]
    public class InteractableObject : MonoBehaviour
    {
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

        [SerializeField] private InteractionMethod interactionMethod = InteractionMethod.ByCollider;
        [SerializeField] private InteractionTrigger interactionAction = InteractionTrigger.KeyOrInputAction;
        [Space]

        [SerializeField] private float triggerDistance = 1;
        [SerializeField] private GameObject overrideDistanceGameObjectSource = null;
        [Space]

        [SerializeField] private GameObject overridePointer = null;

#if ENABLE_LEGACY_INPUT_MANAGER
        [Space, SerializeField] private KeyCode interactionKey = KeyCode.Mouse0;
#else
        [Space, SerializeField] private InputActionReference clickActionReference;
        [Space, SerializeField] private InputActionReference positionActionReference;

        [SerializeField] private ActionState inputActionState = ActionState.Canceled;
#endif

        [Space, SerializeField] private UnityEvent<bool> onConditionFilled = new UnityEvent<bool>();
        [Space, SerializeField] private UnityEvent onInteractionPerformed = new UnityEvent();

        private bool isEntered = false;

        private Vector2 pointerPosition;
        private float distance;
        private RaycastHit pointerHit;

        private List<InteractableColliderHandler> colliderHandlers = new List<InteractableColliderHandler>();

#if !ENABLE_LEGACY_INPUT_MANAGER
        private void OnEnable()
        {
            if(clickActionReference)
                clickActionReference.action.Enable();

            if(positionActionReference)
                positionActionReference.action.Enable();
        }

        private void OnDisable()
        {
            if (clickActionReference)
                clickActionReference.action.Disable();

            if (positionActionReference)
                positionActionReference.action.Disable();
        }

        private void Start()
        {
            if(interactionMethod == InteractionMethod.ByCollider)
            {
                Collider collider;

                if (!TryGetComponent(out collider))
                {
                    if (this.TryFindComponents(out Collider[] colliders, true))
                        foreach (Collider c in colliders)
                        {
                            InteractableColliderHandler handler = c.ValidateComponent<InteractableColliderHandler>();

                            if (interactionAction == InteractionTrigger.OnEnter)
                                handler.OnEntered.AddListener(isEntered => onInteractionPerformed.Invoke());
                            else if(interactionAction == InteractionTrigger.OnExit)
                                handler.OnExit.AddListener(isEntered => onInteractionPerformed.Invoke());
                            else if(interactionAction == InteractionTrigger.KeyOrInputAction)
                            {
                                handler.OnEntered.AddListener(ColliderTriggerHandler);
                            }

                            colliderHandlers.Add(handler);
                        }
                }
                else
                    colliderHandlers = new() { this.ValidateComponent<InteractableColliderHandler>() };
            }

            if(interactionAction == InteractionTrigger.KeyOrInputAction)
                switch (inputActionState)
                {
                    case ActionState.Started:
                        clickActionReference.action.started += InputActionHandler;
                        break;

                    case ActionState.Performed:
                        clickActionReference.action.performed += InputActionHandler;
                        break;

                    case ActionState.Canceled:
                        clickActionReference.action.canceled += InputActionHandler;
                        break;
                }

            if(positionActionReference)
                positionActionReference.action.performed += context => pointerPosition = positionActionReference.action.ReadValue<Vector2>();
        }
#endif

        private void FixedUpdate()
        {
            switch(interactionMethod)
            {
                case InteractionMethod.ByPointer:
                    if (overridePointer)
                        pointerPosition = Camera.WorldToScreenPoint(overridePointer.transform.position);
#if ENABLE_LEGACY_INPUT_MANAGER
                    else
                        pointerPosition = Input.mousePosition;
#endif
                    break;

                case InteractionMethod.ByDistance:
                    Transform source = overrideDistanceGameObjectSource ? overrideDistanceGameObjectSource.transform : Camera.transform;
                    distance = Vector3.Distance(source.position, transform.position);
                    break;
            }
        }

        private void Update()
        {
            switch (interactionMethod)
            {
                case InteractionMethod.ByCollider:
#if ENABLE_LEGACY_INPUT_MANAGER
                    if(interactionAction == InteractionTrigger.KeyOrInputAction && isEntered && Input.GetKeyUp(interactionKey))
                        onInteractionPerformed.Invoke();
#endif
                    break;

                case InteractionMethod.ByDistance:
                    if (distance <= triggerDistance)
                    {
                        onConditionFilled.Invoke(true);

                        if (interactionAction == InteractionTrigger.OnEnter && !isEntered)
                            onInteractionPerformed.Invoke();
#if ENABLE_LEGACY_INPUT_MANAGER
                        else if (interactionAction == InteractionTrigger.KeyOrInputAction && Input.GetKeyUp(interactionKey))
                            onInteractionPerformed.Invoke();
#endif

                        isEntered = true;
                    }
                    else
                    {
                        onConditionFilled.Invoke(false);

                        if (interactionAction == InteractionTrigger.OnExit && isEntered)
                            onInteractionPerformed.Invoke();

                        isEntered = false;
                    }

                    break;

                case InteractionMethod.ByPointer:
#if ENABLE_LEGACY_INPUT_MANAGER
#elif !UNITY_ANDROID || !UNITY_IOS
                    if (Mouse.current is null)
                    {
                        isEntered = false;
                        return;
                    }
#else
                    if (Touchscreen.current is null || Touchscreen.current.touches.Count <= 0)
                    {
                        if (interactionAction == InteractionTrigger.OnExit && isEntered)
                            onInteractionPerformed.Invoke();

                        isEntered = false;
                        return;
                    }
#endif

                    if (
                        Physics.Raycast(
                            Camera.ScreenPointToRay(pointerPosition),
                            out pointerHit,
                            Camera.farClipPlane
                        ) &&
                        (pointerHit.collider.gameObject == gameObject || pointerHit.collider.transform.IsChildOf(transform))
                    )
                    {
                        onConditionFilled.Invoke(true);

                        if (interactionAction == InteractionTrigger.OnEnter && !isEntered)
                            onInteractionPerformed.Invoke();
#if ENABLE_LEGACY_INPUT_MANAGER
                        else if (interactionAction == InteractionTrigger.KeyOrInputAction && Input.GetKeyUp(interactionKey))
                            onInteractionPerformed.Invoke();
#endif
                        isEntered = true;
                    }
                    else
                    {
                        onConditionFilled.Invoke(false);

                        if (interactionAction == InteractionTrigger.OnExit && isEntered)
                            onInteractionPerformed.Invoke();

                        isEntered = false;
                    }

                    break;
            }
        }

        private void ColliderTriggerHandler(InteractableColliderEventData eventData) => isEntered = eventData.isInCollider && isActiveAndEnabled;

#if !ENABLE_LEGACY_INPUT_MANAGER
        private void InputActionHandler(InputAction.CallbackContext context)
        {
            if(isActiveAndEnabled && isEntered)
                onInteractionPerformed.Invoke();
        }
#endif

        private Camera Camera { get => Camera.main ?? Camera.current; }
    }
}