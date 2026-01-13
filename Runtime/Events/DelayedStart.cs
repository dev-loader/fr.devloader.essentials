/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260113

using UnityEngine;

namespace Devloader.Events
{
    public class DelayedStart : MonoBehaviour
    {
        [SerializeField] DelayedAction _action = new DelayedAction();

        private void Start() => _action.Invoke();

        public DelayedAction action { get => _action; set => _action = value; }
    }
}