/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250208

using UnityEngine;

namespace Devloader.Extensions
{
    public static class TransformExtension
    {
        public static Transform RandomPosition(this Transform transform, Vector3 min, Vector3 max)
        {
            transform.position = Vector3Extension.Random(min, max);
            return transform;
        }

        public static Transform RandomLocalPosition(this Transform transform, Vector3 min, Vector3 max)
        {
            transform.localPosition = Vector3Extension.Random(min, max);
            return transform;
        }

        public static Transform RandomEulerAngles(this Transform transform, Vector3 min, Vector3 max)
        {
            transform.eulerAngles = Vector3Extension.Random(min, max);
            return transform;
        }

        public static Transform RandomLocalEulerAngles(this Transform transform, Vector3 min, Vector3 max)
        {
            transform.localEulerAngles = Vector3Extension.Random(min, max);
            return transform;
        }

        public static Transform RandomScale(this Transform transform, Vector3 min, Vector3 max)
        {
            transform.localScale = Vector3Extension.Random(min, max);
            return transform;
        }

        public static Transform RandomUniformScale(this Transform transform, float min, float max)
        {
            transform.localScale = Vector3Extension.RandomUniform(min, max);
            return transform;
        }

        public static Transform ResetAll(this Transform transform, bool useLocalReference = true)
        {
            if (useLocalReference)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }

            transform.localScale = Vector3.one;
            return transform;
        }
    }
}
