/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

using UnityEngine;

namespace Devloader.Maths
{
    public abstract class AbstractRanged<T, TSelf> where T : struct where TSelf : AbstractRanged<T, TSelf>
    {
        [SerializeField] private T _aBound;
        [SerializeField] private T _bBound;
        [SerializeField] private T _currentValue;

        #region Constructeurs
        public AbstractRanged(T aBound, T bBound)
        {
            _aBound = aBound;
            _bBound = bBound;
        }

        public AbstractRanged(T aBound, T bBound, T currentValue) : this(aBound, bBound) => Clamp(currentValue);

        public AbstractRanged(TSelf self) : this(self._aBound, self._bBound, self._currentValue) { }

        public AbstractRanged(TSelf self, T value) : this(self._aBound, self._bBound) => Clamp(value);

        #endregion

        #region Méthodes publiques

        public abstract TSelf Clamp(T value);

        public abstract TSelf InverseLerp(T value);

        public abstract TSelf Lerp(float t);

        public abstract TSelf Random();

        public abstract TSelf Random(int seed);

        #endregion

        #region Opérateurs implicites

        public static implicit operator T(AbstractRanged<T, TSelf> ranged) => ranged._currentValue;

        #endregion

        #region Propriétés

        public T a { get => _aBound; set => _aBound = value; }
        public T b { get => _bBound; set => _bBound = value; }

        public T currentValue { get => _currentValue; protected set => _currentValue = value; }

        #endregion
    }
}