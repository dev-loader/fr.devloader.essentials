/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260113

using UnityEngine;

namespace Devloader.Events
{
    public class DelayedActions : MonoBehaviour
    {
        DelayedAction _onEnableAction = new DelayedAction();
        DelayedAction _startAction = new DelayedAction();

        [Space]
        DelayedAction _onDisableAction = new DelayedAction();
        DelayedAction _onDestroyAction = new DelayedAction();

        private void Awake()
        {
            if(_onEnableAction.count > 0)
                gameObject.AddComponent<DelayedOnEnable>().action = _onEnableAction;

            if (_startAction.count > 0)
                gameObject.AddComponent<DelayedStart>().action = _startAction;

            if (_onDisableAction.count > 0)
                gameObject.AddComponent<DelayedOnDestroy>().action = _onDisableAction;

            if (_onDestroyAction.count > 0)
                gameObject.AddComponent<DelayedOnDestroy>().action = _onDestroyAction;
        }

        public DelayedAction onEnableAction => _onEnableAction;
        public DelayedAction startAction => _startAction;

        public DelayedAction onDisableAction => _onDisableAction;
        public DelayedAction onDestroyAction => _onDestroyAction;
    }
}