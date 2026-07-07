/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260609

using Devloader.InspectorProperty;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Effects
{
    public abstract partial class AbstractEffect : MonoBehaviour
    {
        public static class AnimationCurveUtils
        {
            /// <summary> Généré par ClaudeAI (Sonnet 4.6).
            /// Transition linéaire classique.
            /// f(t) = t
            /// </summary>
            public static AnimationCurve Linear => AnimationCurve.Linear(0f, 0f, 1f, 1f);

            /// <summary> Généré par ClaudeAI (Sonnet 4.6).
            /// Démarre doucement, se termine rapidement.
            /// f(t) = t²  →  pente départ = 0, pente arrivée = 2
            /// </summary>
            public static AnimationCurve EaseIn => new AnimationCurve(
                new Keyframe(0f, 0f, 0f, 0f),
                new Keyframe(1f, 1f, 2f, 2f)
            );

            /// <summary> Généré par ClaudeAI (Sonnet 4.6).
            /// Démarre rapidement, se termine doucement.
            /// f(t) = 2t − t²  →  pente départ = 2, pente arrivée = 0
            /// </summary>
            public static AnimationCurve EaseOut => new AnimationCurve(
                new Keyframe(0f, 0f, 0f, 2f),
                new Keyframe(1f, 1f, 0f, 0f)
            );

            /// <summary> Généré par ClaudeAI (Sonnet 4.6).
            /// Démarre et se termine doucement (SmoothStep).
            /// f(t) = 3t² − 2t³  →  pente départ = 0, pente arrivée = 0
            /// </summary>
            public static AnimationCurve EaseInOut => new AnimationCurve(
                new Keyframe(0f, 0f, 0f, 0f),
                new Keyframe(1f, 1f, 0f, 0f)
            );

            /// <summary>
            /// Courbe constante avec y = la valeur envoyée en paramètre
            /// </summary>
            public static AnimationCurve Constant(float value = 0) => AnimationCurve.Constant(0, 1, value);

            /// <summary> Généré par ClaudeAI (Sonnet 4.6).
            /// Trouver le temps correspondant à la valeur
            /// </summary>
            public static float FindTimeForValue(AnimationCurve curve, float targetValue, int iterations = 16)
            {
                float lo = 0f, hi = 1f;
                for (int i = 0; i < iterations; i++)
                {
                    float mid = (lo + hi) * 0.5f;
                    if (curve.Evaluate(mid) < targetValue)
                        lo = mid;
                    else
                        hi = mid;
                }
                return (lo + hi) * 0.5f;
            }
        }

        public enum EffectCurveShapePreset
        {
            Linear = 0,
            EaseIn = 1,
            EaseOut = 2,
            EaseInOut = 3,
            Custom = 4,
        }

        public enum EffectDirection
        {
            B2A = -1,
            Pause = 0,
            A2B = 1
        }

        public enum EffectLoopMethod
        {
            None = 0,
            Cycle = 1,
            PingPong = 2,
        }

        [Header("Curve settings")]
        [SerializeField] private EffectCurveShapePreset _curveShapePreset;
        [SerializeField] private AnimationCurve _curveShape = AnimationCurveUtils.Linear;

        [Header("Effect settings")]
        [SerializeField] private float _duration = 1;
        [SerializeField] private bool _useFixedUpdate;

        [Space]
        [SerializeField] private EffectDirection _direction = EffectDirection.A2B;
        [SerializeField] private EffectLoopMethod _loopMethod = EffectLoopMethod.None;

        [Header("Evenements")]
        [SerializeField] private EffectEvent _events = new EffectEvent();

        [Header("Debug")]
        [SerializeField, ReadOnly] private float _t = 0;

        Coroutine _coroutine = null;
        string _error;

        float _progress;
        UnityAction<float> _processAction;

        bool _loop;
        System.Predicate<float> _processPredicate;

#if UNITY_EDITOR
        protected virtual void OnValidate() => UpdateCurve();
#endif

        /// <summary>
        /// Met à jour la courbe et retourne la valeur à l'instant t de la courbe précédente.
        /// </summary>
        /// <param name="customCurve">Courbe utilisé par l'effet si le preset est réglé sur Custom.</param>
        /// <returns>Retourne la valeur à l'instant t de la courbe précédente</returns>
        float UpdateCurve(AnimationCurve customCurve = null)
        {
            float value = _curveShape.Evaluate(_t);

            switch (_curveShapePreset)
            {
                case EffectCurveShapePreset.Linear:
                    _curveShape = AnimationCurveUtils.Linear;
                    break;

                case EffectCurveShapePreset.EaseIn:
                    _curveShape = AnimationCurveUtils.EaseIn;
                    break;

                case EffectCurveShapePreset.EaseOut:
                    _curveShape = AnimationCurveUtils.EaseOut;
                    break;

                case EffectCurveShapePreset.EaseInOut:
                    _curveShape = AnimationCurveUtils.EaseInOut;
                    break;

                default:
                    _curveShape = customCurve ?? AnimationCurveUtils.Constant();
                    break;
            }

            return value;
        }

        public static EffectDirection IntDirectionToEffectDirection(int direction) => direction > 0 ? EffectDirection.A2B : (direction < 0 ? EffectDirection.B2A : EffectDirection.Pause);

        public virtual AbstractEffect SetToBegin() => SetToBegin(_direction);

        public virtual AbstractEffect SetToBegin(int direction) => SetToBegin(IntDirectionToEffectDirection(direction));

        public virtual AbstractEffect SetToBegin(EffectDirection direction)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _direction = direction;

            _t = _direction == EffectDirection.A2B ? 0 : _duration;
            _progress = _direction == EffectDirection.B2A ? 1 : 0;

            _processAction?.Invoke(_curveShape.Evaluate(_t));
            return this;
        }

        public virtual AbstractEffect SetToBegin(int direction, EffectCurveShapePreset preset, AnimationCurve customCurve = null) => SetToBegin(IntDirectionToEffectDirection(direction), preset, customCurve);

        public virtual AbstractEffect SetToBegin(EffectDirection direction, EffectCurveShapePreset preset, AnimationCurve customCurve = null)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _direction = direction;
            _curveShapePreset = preset;

            UpdateCurve(customCurve);

            _t = _direction == EffectDirection.A2B ? 0 : _duration;
            _progress = _direction == EffectDirection.B2A ? 1 : 0;

            _processAction?.Invoke(_curveShape.Evaluate(_t));
            return this;
        }

        public virtual AbstractEffect SetToEnd() => SetToEnd(_direction);

        public virtual AbstractEffect SetToEnd(int direction) => SetToEnd(IntDirectionToEffectDirection(direction));

        public virtual AbstractEffect SetToEnd(EffectDirection direction)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _direction = direction;

            _t = _direction == EffectDirection.A2B ? _duration : 0;
            _progress = _direction == EffectDirection.B2A ? 0 : 1;

            _processAction?.Invoke(_curveShape.Evaluate(_duration));
            return this;
        }

        public virtual AbstractEffect SetToEnd(int direction, EffectCurveShapePreset preset, AnimationCurve customCurve = null) => SetToEnd(IntDirectionToEffectDirection(direction), preset, customCurve);

        public virtual AbstractEffect SetToEnd(EffectDirection direction, EffectCurveShapePreset preset, AnimationCurve customCurve = null)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _direction = direction;
            _curveShapePreset = preset;

            UpdateCurve(customCurve);

            _t = _direction == EffectDirection.A2B ? _duration : 0;
            _progress = _direction == EffectDirection.B2A ? 0 : 1;

            _processAction?.Invoke(_curveShape.Evaluate(_duration));
            return this;
        }

        public EffectDirection ToggleDirection()
        {
            _direction = (EffectDirection)(-(int)_direction);
            return _direction;
        }

        public virtual void Run() => Run(_direction);

        /// <summary>
        /// Start/Pause the fade: -1 to fade out, 0 to pause, 1 to fade in
        /// </summary>
        /// <param name="direction">-1 to fade out, 0 to pause, 1 to fade in</param>
        public virtual void Run(int direction) => Run(IntDirectionToEffectDirection(direction));

        public virtual void Run(EffectDirection direction)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _direction = direction;

            if (_direction == 0)
                return;

            _loop = _loopMethod != EffectLoopMethod.None;
            _coroutine = StartCoroutine(ProcessCoroutine());
        }


        /// <summary>
        /// Start/Pause the fade: -1 to fade out, 0 to pause, 1 to fade in
        /// </summary>
        /// <param name="direction">-1 to fade out, 0 to pause, 1 to fade in</param>
        public virtual void Run(int direction, EffectCurveShapePreset preset, AnimationCurve customCurve = null) => Run(IntDirectionToEffectDirection(direction), preset, customCurve);

        public virtual void Run(EffectDirection direction, EffectCurveShapePreset preset, AnimationCurve customCurve = null)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _direction = direction;
            _curveShapePreset = preset;

            if (_direction == 0)
                return;

            _loop = _loopMethod != EffectLoopMethod.None;
            _coroutine = StartCoroutine(ProcessCoroutine(true, UpdateCurve(customCurve)));
        }

        IEnumerator ProcessCoroutine(bool curveHasChanged = false, float lastValue = 0)
        {
            if (curveHasChanged)
                _t = AnimationCurveUtils.FindTimeForValue(_curveShape, lastValue);

            if (_duration <= 0)
            {
                _progress = 1;
                _events.Invoke(this, EffectEvent.EventType.Completed);

                yield break;
            }

            _progress = _t / _duration;
            _events.Invoke(this, EffectEvent.EventType.Started);

            _processPredicate = LoopControl;

            while (_processPredicate(_t))
            {
                if (_useFixedUpdate)
                    _t += Time.fixedDeltaTime * (float)_direction;
                else
                    _t += Time.deltaTime * (float)_direction;

                if (_loop && !_processPredicate(_t))
                {
                    if (_loopMethod == EffectLoopMethod.Cycle)
                    {
                        _t = _direction == EffectDirection.A2B ? 0 : _duration;
                        _progress = _direction == EffectDirection.B2A ? 1 : 0;
                    }
                    else if (_loopMethod == EffectLoopMethod.PingPong)
                    {
                        ToggleDirection();
                        _processPredicate = LoopControl;
                    }
                }

                _progress = _t / _duration;

                _processAction?.Invoke(_curveShape.Evaluate(_progress));
                _events.Invoke(this, EffectEvent.EventType.Progress);

                if (_useFixedUpdate)
                    yield return new WaitForFixedUpdate();
                else
                    yield return null;
            }

            _events.Invoke(this, EffectEvent.EventType.Completed);
            yield break;
        }

        public EffectCurveShapePreset CurveShapePreset => _curveShapePreset;

        [System.Obsolete("Use Direction property instead")]
        public int direction { get => (int)_direction; set => _direction = IntDirectionToEffectDirection(value); }

        public EffectDirection Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                _processPredicate = LoopControl;
            }
        }

        [System.Obsolete("Use Duration property instead")]
        public float duration { get => _duration; set => _duration = value; }

        public float Duration { get => _duration; set => _duration = value; }

        [System.Obsolete("Use Error property instead")]
        public string error { get => _error; }

        public string Error => _error;

        [System.Obsolete("Use Events property instead")]
        public EffectEvent events => _events;

        public EffectEvent Events => _events;

        [System.Obsolete("Use FirstValue property instead")]
        public float firstValue => FirstValue;
        public float FirstValue => _curveShape.keys.Length > 0 ? _curveShape.keys[0].value : 0;

        [System.Obsolete("Use FinalValue property instead")]
        public float finalValue => FinalValue;
        public float FinalValue => _curveShape.keys.Length > 0 ? _curveShape.keys[_curveShape.keys.Length - 1].value : 1;

        protected virtual System.Predicate<float> LoopControl => _direction == EffectDirection.A2B ? ((t) => t < _duration) : (_direction == EffectDirection.B2A ? ((t) => t > 0) : ((t) => false));

        public EffectLoopMethod LoopMethod
        {
            get => _loopMethod;
            set
            {
                _loopMethod = value;
                _loop = _loopMethod != EffectLoopMethod.None;
            }
        }

        [System.Obsolete("Use ProcessAction property instead")]
        protected UnityAction<float> processAction { get => _processAction; set => _processAction = value; }

        protected UnityAction<float> ProcessAction { get => _processAction; set => _processAction = value; }

        [System.Obsolete("Use Progress property instead")]
        public float progress => _progress;

        public float Progress => _progress;

        [System.Obsolete("Use T property instead")]
        public float t => _t;

        public float T => _t;
    }
}