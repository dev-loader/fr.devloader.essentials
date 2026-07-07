/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250412

using UnityEngine;

namespace Devloader.Maths
{
    [System.Serializable]
    public struct ClampedFloat
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        [SerializeField] private float _current;

        /// <summary>
        /// Crée une structure permettant de gérer une plage de valeurs réelles
        /// </summary>
        /// <param name="min">Valeur minimale de la plage</param>
        /// <param name="max">Valeur maximale de la plage</param>
        /// <param name="current">Valeur actuelle de la plage</param>
        public ClampedFloat(float min, float max, float current)
        {
            _min = Mathf.Min(min, max);
            _max = Mathf.Max(min, max);

            _current = Mathf.Clamp(current, _min, _max);
        }

        /// <summary>
        /// Contraint la valeur de value dans la plage délimitée par min et max
        /// </summary>
        /// <param name="value">Valeur qui sera limitée par min et max</param>
        /// <returns>Retourne l'instance sur laquelle est effectuée l'opération</returns>
        public ClampedFloat Clamp(float value)
        {
            current = value;
            return this;
        }

        /// <summary>
        /// Retourne une valeur entre 0 et 1 correspondant à la position de la valeur actuelle dans la plage délimitée par min et max
        /// </summary>
        /// <returns>Le pourcentage correspondant à la position de la valeur actuelle (compris entre 0 et 1)</returns>
        public float InverseLerp() => Mathf.InverseLerp(min, max, current);

        /// <summary>
        /// Retourne une valeur entre 0 et 1 correspondant à la position du paramètre value dans la plage délimitée par min et max. Met à jour la position actuelle.
        /// </summary>
        /// <param name="value">Valeur utilisée pour déterminer le pourcentage</param>
        /// <returns>Le pourcentage correspondant à la position de value (compris entre 0 et 1)</returns>
        public float InverseLerp(float value)
        {
            current = value;
            return InverseLerp();
        }

        /// <summary>
        /// Détermine la valeur entre min et max correspondant à percent
        /// </summary>
        /// <param name="percent">Pourcentage permettant de déterminer la valeur entre min et max correspondante (doit être compris entre 0 et 1)</param>
        /// <returns>La valeur correpondant à percentage (e.g. Min = 0, Max = 20, percent = 0.5, retourne 10)</returns>
        public ClampedFloat Lerp(float percent)
        {
            current = Mathf.Lerp(min, max, percent);
            return this;
        }

        public static implicit operator float(ClampedFloat clampedFloat) => clampedFloat._current;

        /// <summary>
        /// Valeur actuelle de l'étendue comprise entre min et max
        /// </summary>
        public float current
        {
            get => _current;
            set => _current = Mathf.Clamp(value, min, max);
        }

        /// <summary>
        /// Etendue de la plage
        /// </summary>
        public float length => _max - _min;

        /// <summary>
        /// Valeur maximale de la plage
        /// </summary>
        public float max
        {
            get => _max;
            set => _max = value;
        }

        /// <summary>
        /// Valeur minimale de la plage
        /// </summary>
        public float min
        {
            get => _min;
            set => _min = value;
        }
    }
}