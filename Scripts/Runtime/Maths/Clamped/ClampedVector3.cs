/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250412

using UnityEngine;

namespace Devloader.Maths
{
    [System.Serializable]
    public struct ClampedVector3
    {
        [SerializeField] private Vector3 _min;
        [SerializeField] private Vector3 _max;

        [SerializeField] private Vector3 _current;

        /// <summary>
        /// Crée une structure permettant de gérer une plage de vecteurs
        /// </summary>
        /// <param name="min">Valeur minimale de la plage</param>
        /// <param name="max">Valeur maximale de la plage</param>
        /// <param name="current">Valeur actuelle de la plage</param>
        public ClampedVector3(Vector3 min, Vector3 max, Vector3 current)
        {
            _min = Vector3.Min(min, max);
            _max = Vector3.Max(min, max);

            _current = new Vector3(
                Mathf.Clamp(current.x, min.x, max.x),
                Mathf.Clamp(current.y, min.y, max.y),
                Mathf.Clamp(current.z, min.z, max.z)
            );
        }

        /// <summary>
        /// Contraint la valeur de value dans la plage délimitée par min et max. Met à jour la position actuelle
        /// </summary>
        /// <param name="value">Valeur qui sera limitée par min et max</param>
        /// <returns>Retourne l'instance sur laquelle est effectuée l'opération</returns>
        public ClampedVector3 Clamp(Vector3 value)
        {
            current = value;
            return this;
        }

        /// <summary>
        /// Retourne une valeur entre 0 et 1 correspondant à la position de la valeur actuelle dans la plage délimitée par min et max
        /// </summary>
        /// <returns>Le pourcentage correspondant à la position de la valeur actuelle (compris entre 0 et 1)</returns>
        public Vector3 InverseLerp()
        {
            Vector3 result = new Vector3(
                (current.x - min.x) / (max.x - min.x),
                (current.y - min.y) / (max.y - min.y),
                (current.z - min.z) / (max.z - min.z)
            );

            return result;
        }

        /// <summary>
        /// Retourne une valeur entre 0 et 1 correspondant à la position du paramètre value dans la plage délimitée par min et max. Met à jour la position actuelle.
        /// </summary>
        /// <param name="value">Valeur utilisée pour déterminer le pourcentage</param>
        /// <returns>Le pourcentage correspondant à la position de value (compris entre 0 et 1)</returns>
        public Vector3 InverseLerp(Vector3 value)
        {
            current = value;
            return InverseLerp();
        }

        /// <summary>
        /// Détermine la valeur entre min et max correspondant à percent. Met à jour la position actuelle
        /// </summary>
        /// <param name="percent">Pourcentage permettant de déterminer la valeur entre min et max correspondante (doit être compris entre 0 et 1)</param>
        /// <returns>La valeur correpondant à percentage (e.g. Min = 0, Max = 20, percent = 0.5, retourne 10)</returns>
        public ClampedVector3 Lerp(float percent)
        {
            current = Vector3.Lerp(min, max, percent);
            return this;
        }

        public static implicit operator Vector3(ClampedVector3 clampedValue) => clampedValue._current;

        /// <summary>
        /// Valeur actuelle de l'étendue comprise entre min et max
        /// </summary>
        public Vector3 current
        {
            get => _current;
            set => _current = new Vector3(
                Mathf.Clamp(value.x, min.x, max.x),
                Mathf.Clamp(value.y, min.y, max.y),
                Mathf.Clamp(value.z, min.z, max.z)
            );
        }

        /// <summary>
        /// Etendue de la plage
        /// </summary>
        public float length => Vector3.Distance(_min, _max);

        /// <summary>
        /// Valeur maximale de la plage
        /// </summary>
        public Vector3 max
        {
            get => _max;
            set => _max = value;
        }

        /// <summary>
        /// Valeur minimale de la plage
        /// </summary>
        public Vector3 min
        {
            get => _min;
            set => _min = value;
        }

        public float x => _current.x;
        public float y => _current.y;
        public float z => _current.z;
    }
}