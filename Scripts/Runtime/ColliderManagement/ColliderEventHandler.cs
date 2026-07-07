/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using UnityEngine;
using UnityEngine.Events;

namespace Devloader.ColliderManagement
{
    public class ColliderEventHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent<Collision> _collisionEnterEvent = new UnityEvent<Collision>();
        [SerializeField] UnityEvent<Collision> _collisionStayEvent = new UnityEvent<Collision>();
        [SerializeField] UnityEvent<Collision> _collisionExitEvent = new UnityEvent<Collision>();

        [SerializeField] UnityEvent<Collider> _triggerEnterEvent = new UnityEvent<Collider>();
        [SerializeField] UnityEvent<Collider> _triggerStayEvent = new UnityEvent<Collider>();
        [SerializeField] UnityEvent<Collider> _triggerExitEvent = new UnityEvent<Collider>();

        public UnityEvent<Collision> collisionEnterEvent => _collisionEnterEvent;
        public UnityEvent<Collision> collisionExitEvent => _collisionExitEvent;
        public UnityEvent<Collision> collisionStayEvent => _collisionStayEvent;

        public UnityEvent<Collider> triggerEnterEvent => _triggerEnterEvent;
        public UnityEvent<Collider> triggerExitEvent => _triggerExitEvent;
        public UnityEvent<Collider> triggerStayEvent => _triggerStayEvent;

        private void OnTriggerEnter(Collider other) => triggerEnterEvent.Invoke(other);

        private void OnTriggerExit(Collider other) => triggerExitEvent.Invoke(other);

        private void OnTriggerStay(Collider other) => triggerStayEvent.Invoke(other);

        private void OnCollisionEnter(Collision collision) => collisionEnterEvent.Invoke(collision);

        private void OnCollisionExit(Collision collision) => collisionExitEvent.Invoke(collision);

        private void OnCollisionStay(Collision collision) => collisionStayEvent.Invoke(collision);
    }
}
