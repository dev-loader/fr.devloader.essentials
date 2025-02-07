/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using System;

using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Startup
{
    [Serializable]
    public class StartupReadyEvent : UnityEvent
    { }

    [AddComponentMenu("Devloader/Startup/StartupStep")]
    public class StartupStep : MonoBehaviour
    {
        public StartupReadyEvent onStartupReady = new StartupReadyEvent();

        private void OnDestroy()
        { StartupSequence.RemoveStep(this); }
    }
}