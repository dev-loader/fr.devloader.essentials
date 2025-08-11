/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250811

using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Lifecycle
{
    public class OnDestroyHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent _onDestroy = new UnityEvent();

        protected virtual void OnDestroy() => _onDestroy.Invoke();

        public virtual UnityEvent onDestroy => _onDestroy;
    }
}
