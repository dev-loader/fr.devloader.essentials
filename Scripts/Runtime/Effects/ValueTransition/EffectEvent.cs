/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using UnityEngine.Events;

namespace Devloader.Effects
{
    [System.Serializable]
    public class EffectProgressEvent : UnityEvent<float>
    { }

    [System.Serializable]
    public class EffectEvent : UnityEvent<AbstractEffect, EffectEvent.EventType>
    {
        public enum EventType
        {
            Started,
            Progress,
            Completed,
            Error
        }
    }
}