/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250603

using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Effects
{
    [AddComponentMenu("Dev'loader/Effects/Effect Behaviour")]
    public class EffectBehaviour : MonoBehaviour
    {
        public enum RunDirection
        {
            In = 1,
            None = 0,
            Out = -1
        }

        public bool runOnStart = true;
        public AbstractEffect effect;

        [Range(-1, 1)]
        public int runOnStartDirection = 1; 

        [Header("Effect In Events")]
        public UnityEvent onEffectInStarted = new UnityEvent();
        public UnityEvent onEffectInFinished = new UnityEvent();
        public UnityEvent<float> onEffectInProgressed = new UnityEvent<float>();

        private UnityAction inCallback = null;

        [Header("Effect Out Events")]
        public UnityEvent onEffectOutStarted = new UnityEvent();
        public UnityEvent onEffectOutFinished = new UnityEvent();
        public UnityEvent<float> onEffectOutProgressed = new UnityEvent<float>();

        private UnityAction outCallback = null;

        [Header("General Events")]
        public UnityEvent<AbstractEffect> onStarted = new UnityEvent<AbstractEffect>();
        public UnityEvent<AbstractEffect> onFinished = new UnityEvent<AbstractEffect>();
        public UnityEvent<AbstractEffect, float> onProgressed = new UnityEvent<AbstractEffect, float>();

        [Header("Error Event")]
        public UnityEvent<AbstractEffect> onError = new UnityEvent<AbstractEffect>();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!this.effect && TryGetComponent(out AbstractEffect effect))
                this.effect = effect;
        }
#endif

        private void Start()
        {
            if (runOnStart && effect)
                effect.Run(runOnStartDirection); 
        }

        public void RunIn(UnityAction callback = null)
        {
            if (effect)
            {
                effect.Run(1);
                inCallback = callback;
            }
        }

        public void RunOut(UnityAction callback = null)
        {
            if (effect)
            {
                effect.Run(-1);
                outCallback = callback;
            }
        }

        public void Run(RunDirection direction)
        {
            if (direction == RunDirection.Out)
                RunOut();
            else if (direction == RunDirection.In)
                RunIn();
            else if (effect)
                effect.Run(0);
        }

        public void OnEvent(AbstractEffect effect, EffectEvent.EventType eventType)
        {
            switch (eventType)
            {
                case EffectEvent.EventType.Started:
                    if (effect.Direction == AbstractEffect.EffectDirection.A2B)
                        onEffectInStarted.Invoke();
                    else if (effect.Direction == AbstractEffect.EffectDirection.B2A)
                        onEffectOutStarted.Invoke();

                    onStarted.Invoke(effect);
                    break;

                case EffectEvent.EventType.Completed:
                    if (effect.Direction == AbstractEffect.EffectDirection.A2B)
                    {
                        onEffectInFinished.Invoke();

                        if (inCallback != null)
                            inCallback.Invoke();
                    }
                    else if (effect.Direction == AbstractEffect.EffectDirection.B2A)
                    {
                        onEffectOutFinished.Invoke();

                        if (outCallback != null)
                            outCallback.Invoke();
                    }

                    onFinished.Invoke(effect);
                    break;

                case EffectEvent.EventType.Progress:
                    if (effect.Direction == AbstractEffect.EffectDirection.A2B)
                        onEffectInProgressed.Invoke(effect.Progress);
                    else if (effect.Direction == AbstractEffect.EffectDirection.B2A)
                        onEffectOutProgressed.Invoke(effect.Progress);

                    onProgressed.Invoke(effect, effect.Progress);
                    break;

                case EffectEvent.EventType.Error:
                    onError.Invoke(effect);
                    break;
            }
        }
    }
}