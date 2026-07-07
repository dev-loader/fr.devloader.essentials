/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260113

using UnityEngine;

namespace Devloader.Events
{
    public class DelayedOnDestroy : MonoBehaviour
    {
        [SerializeField] DelayedAction _action = new DelayedAction();

        private void OnDestroy() => _action.Invoke();

        public DelayedAction action { get => _action; set => _action = value; }
    }
}