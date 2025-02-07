/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using UnityEngine;

namespace Devloader.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);

            return value;
        }

        public static Vector3 Clamp01(this Vector3 value)
        {
            value.x = Mathf.Clamp01(value.x);
            value.y = Mathf.Clamp01(value.y);
            value.z = Mathf.Clamp01(value.z);

            return value;
        }

        public static bool IsInRange(this Vector3 value, Vector3 min, Vector3 max, bool includeMin = true, bool includeMax = true) => (includeMin ? (value.x >= min.x && value.y >= min.y && value.z >= min.z) : (value.x > min.x && value.y > min.y && value.z > min.z))
                && (includeMax ? (value.x <= max.x && value.y <= max.y && value.z <= max.z) : (value.x < max.x && value.y < max.y && value.z < max.z));


        public static Vector3 Random(Vector3 min, Vector3 max) => new Vector3(
            UnityEngine.Random.Range(min.x, max.x),
            UnityEngine.Random.Range(min.y, max.y),
            UnityEngine.Random.Range(min.z, max.z)
        );

        public static Vector3 RandomUniform(float min, float max)
        {
            float rand = UnityEngine.Random.Range(min, max);
            return new Vector3(rand, rand, rand);
        }
    }
}