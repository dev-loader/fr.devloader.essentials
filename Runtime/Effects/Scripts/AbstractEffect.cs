/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240227

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Effects
{
    public abstract partial class AbstractEffect : MonoBehaviour
    {
        protected UnityAction<float> processAction;

        [Header("Aperçu de la courbe")]
        public AnimationCurve curve = null;
        private Keyframe[] keyframes = { };
        [Range(-1, 1)]
        public int direction = 1;

        [Space]

        [Header("Réglages généraux")]
        [Range(0.01f, 10)]
        public float duration = 1;

        [Space]

        [Header("Valeurs de la courbe")]
        public float firstValue = 0;
        public float finalValue = 1;

        [Space]

        [Header("Evenements")]
        public EffectEvent events = new EffectEvent();

        float t = 0;
        public float T
        {
            get { return t; }
            set
            {
                if (!isRunning)
                    t = value;
            }
        }

        private bool isRunning = false;
        Coroutine coroutine = null;

        [NonSerialized]
        public string error;
        public float progress;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            EditorUpdateCurve();
        }

        private void EditorUpdateCurve()
        {
            if (direction > 0)
                keyframes = new Keyframe[]
                {
                    new Keyframe(0, firstValue),
                    new Keyframe(duration, finalValue)
                };
            else if (direction < 0)
                keyframes = new Keyframe[]
                {
                    new Keyframe(0, finalValue),
                    new Keyframe(duration, firstValue)
                };
            else if (direction == 0)
                keyframes = new Keyframe[]
                {
                    new Keyframe(0, firstValue),
                    new Keyframe(duration, firstValue)
                };

            keyframes[0].outTangent = (keyframes[1].value - keyframes[0].value) / duration;
            keyframes[1].inTangent = (keyframes[1].value - keyframes[0].value) / duration;

            curve = new AnimationCurve(keyframes);
        }
#endif

        private float RuntimeUpdateCurve()
        {
            float value = 0;

            if (curve != null)
                value = curve.Evaluate(t);

            if (direction > 0)
                keyframes = new Keyframe[]
                {
                    new Keyframe(0, firstValue),
                    new Keyframe(duration, finalValue)
                };
            else if (direction < 0)
                keyframes = new Keyframe[]
                {
                    new Keyframe(0, finalValue),
                    new Keyframe(duration, firstValue)
                };
            else if (direction == 0)
                keyframes = new Keyframe[]
                {
                    new Keyframe(0, value),
                    new Keyframe(duration, value)
                };

            keyframes[0].outTangent = (keyframes[1].value - keyframes[0].value) / duration;
            keyframes[1].inTangent = (keyframes[1].value - keyframes[0].value) / duration;

            curve = new AnimationCurve(keyframes);
            return value;
        }

        public virtual void SetToBegin(int direction)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            this.direction = direction;
            t = 0;

            RuntimeUpdateCurve();
            processAction?.Invoke(curve.Evaluate(t));
        }

        public virtual void SetToEnd(int direction)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            this.direction = direction;
            t = duration;

            RuntimeUpdateCurve();
            processAction?.Invoke(curve.Evaluate(duration));
        }

        /// <summary>
        /// Start/Pause the fade: -1 to fade out, 0 to pause, 1 to fade in
        /// </summary>
        /// <param name="direction">-1 to fade out, 0 to pause, 1 to fade in</param>
        public virtual void Run(int direction)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            this.direction = direction;

            if (this.direction == 0)
                return;

            coroutine = StartCoroutine(ProcessCoroutine(RuntimeUpdateCurve()));
        }

        private IEnumerator ProcessCoroutine(float lastValue)
        {
            // Recherche du t correspondant à la valeur de la précédente courbe sur la nouvelle
            // Degré d'imprécision
            float treshold = 0.001f;

            bool tFound = false;

            // Tant que la valeur courante ne dépasse pas la valeur maximale de la courbe
            while (t < curve.keys[curve.length - 1].time && !tFound)
            {
                float currentValue = curve.Evaluate(t);

                if (Mathf.Abs(currentValue - lastValue) < treshold)
                    tFound = true;
                else
                    t += Time.fixedDeltaTime;
            }

            if (!tFound)
            {
                error = "can not found time corresponding to value";
                events.Invoke(this, EffectEvent.EventType.Error);
            }

            events.Invoke(this, EffectEvent.EventType.Started);

            while (t < duration)
            {
                t += Time.fixedDeltaTime;
                progress = curve.Evaluate(t);

                processAction?.Invoke(progress);
                events.Invoke(this, EffectEvent.EventType.Progress);

                yield return new WaitForFixedUpdate();
            }

            t = duration;
            events.Invoke(this, EffectEvent.EventType.Progress);

            events.Invoke(this, EffectEvent.EventType.Completed);
            yield break;
        }
    }
}