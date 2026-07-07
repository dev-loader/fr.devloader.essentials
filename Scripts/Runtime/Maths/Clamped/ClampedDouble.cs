/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250412

using System;
using UnityEngine;

namespace Devloader.Maths
{
    [Serializable]
    public struct ClampedDouble
    {
        [SerializeField] private double _min;
        [SerializeField] private double _max;

        [SerializeField] private double _current;

        /// <summary>
        /// Crée une structure permettant de gérer une plage de valeurs réelles à double précision
        /// </summary>
        /// <param name="min">Valeur minimale de la plage</param>
        /// <param name="max">Valeur maximale de la plage</param>
        /// <param name="current">Valeur actuelle de la plage</param>
        public ClampedDouble(double min, double max, double current)
        {
            _min = Math.Min(min, max);
            _max = Math.Max(min, max);

            _current = Math.Clamp(current, _min, _max);
        }

        /// <summary>
        /// Contraint la valeur de value dans la plage délimitée par min et max. Met à jour la position actuelle
        /// </summary>
        /// <param name="value">Valeur qui sera limitée par min et max</param>
        /// <returns>Retourne l'instance sur laquelle est effectuée l'opération</returns>
        public ClampedDouble Clamp(double value)
        {
            current = value;
            return this;
        }

        /// <summary>
        /// Retourne une valeur entre 0 et 1 correspondant à la position de la valeur actuelle dans la plage délimitée par min et max
        /// </summary>
        /// <returns>Le pourcentage correspondant à la position de la valeur actuelle (compris entre 0 et 1)</returns>
        public double InverseLerp() => (current - min) / (max - min);

        /// <summary>
        /// Retourne une valeur entre 0 et 1 correspondant à la position du paramètre value dans la plage délimitée par min et max. Met à jour la position actuelle.
        /// </summary>
        /// <param name="value">Valeur utilisée pour déterminer le pourcentage</param>
        /// <returns>Le pourcentage correspondant à la position de value (compris entre 0 et 1)</returns>
        public double InverseLerp(double value)
        {
            current = value;
            return InverseLerp();
        }

        /// <summary>
        /// Détermine la valeur entre min et max correspondant à percent. Met à jour la position actuelle
        /// </summary>
        /// <param name="percent">Pourcentage permettant de déterminer la valeur entre min et max correspondante (doit être compris entre 0 et 1)</param>
        /// <returns>La valeur correpondant à percentage (e.g. Min = 0, Max = 20, percent = 0.5, retourne 10)</returns>
        public ClampedDouble Lerp(double percent)
        {
            current = (max - min) * percent + min;
            return this;
        }

        public static implicit operator double(ClampedDouble clampedValue) => clampedValue._current;

        /// <summary>
        /// Valeur actuelle de l'étendue comprise entre min et max
        /// </summary>
        public double current
        {
            get => _current;
            set => _current = Math.Clamp(value, min, max);
        }

        /// <summary>
        /// Etendue de la plage
        /// </summary>
        public double length => _max - _min;

        /// <summary>
        /// Valeur maximale de la plage
        /// </summary>
        public double max
        {
            get => _max;
            set => _max = value;
        }

        /// <summary>
        /// Valeur minimale de la plage
        /// </summary>
        public double min
        {
            get => _min;
            set => _min = value;
        }
    }
}