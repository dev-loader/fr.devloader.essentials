/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240716

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Devloader.Interaction
{
    public class InteractableColliderHandler : MonoBehaviour
    {
        [SerializeField] LayerMask layers;

        [Header("Events")]
        [Space]
        [SerializeField] private UnityEvent<InteractableColliderEventData> onTriggerEnter = new UnityEvent<InteractableColliderEventData>();
        [Space]

        [SerializeField] private UnityEvent<InteractableColliderEventData> onTriggerExit = new UnityEvent<InteractableColliderEventData>();

        private void Start()
        {
            if (TryGetComponent(out Collider collider))
                collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((layers & (1 << other.gameObject.layer)) != 0)
            {
                onTriggerEnter.Invoke(new InteractableColliderEventData(true, other, other.ClosestPoint(transform.position)));
                Debug.Log(other.gameObject.name + " entered");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((layers & (1 << other.gameObject.layer)) != 0)
                onTriggerExit.Invoke(new InteractableColliderEventData(false, other, other.ClosestPoint(transform.position)));
        }

        public UnityEvent<InteractableColliderEventData> OnEntered { get => onTriggerEnter; }

        public UnityEvent<InteractableColliderEventData> OnExit { get => onTriggerExit; }
    }
}