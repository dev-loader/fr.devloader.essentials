/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

using UnityEngine;

namespace Devloader.Maths
{
    [System.Serializable]
    public class RangedVector3 : AbstractRanged<Vector3, RangedVector3>
    {
        public RangedVector3(Vector3 aBound, Vector3 bBound) : base(aBound, bBound) { }

        public RangedVector3(Vector3 aBound, Vector3 bBound, Vector3 currentValue) : base(aBound, bBound, currentValue) { }

        public RangedVector3(RangedVector3 self) : base(self) { }

        public RangedVector3(RangedVector3 self, Vector3 value) : base(self, value) { }

        public override RangedVector3 Clamp(Vector3 value)
        {
            currentValue = new Vector3()
            {
                x = new RangedFloat(a.x, b.x, value.x),
                y = new RangedFloat(a.y, b.y, value.y),
                z = new RangedFloat(a.z, b.z, value.z),
            };

            return this;
        }

        public override RangedVector3 InverseLerp(Vector3 value)
        {
            currentValue = new Vector3()
            {
                x = new RangedFloat(a.x, b.x).InverseLerp(value.x),
                y = new RangedFloat(a.y, b.y).InverseLerp(value.y),
                z = new RangedFloat(a.z, b.z).InverseLerp(value.z),
            };

            return this;
        }

        public override RangedVector3 Lerp(float t)
        {
            currentValue = new Vector3()
            {
                x = new RangedFloat(a.x, b.x).Lerp(t),
                y = new RangedFloat(a.y, b.y).Lerp(t),
                z = new RangedFloat(a.z, b.z).Lerp(t),
            };

            return this;
        }

        public override RangedVector3 Random()
        {
            currentValue = new Vector3()
            {
                x = new RangedFloat(a.x, b.x).Random(),
                y = new RangedFloat(a.y, b.y).Random(),
                z = new RangedFloat(a.z, b.z).Random(),
            };

            return this;
        }

        public override RangedVector3 Random(int seed)
        {
            currentValue = new Vector3()
            {
                x = new RangedFloat(a.x, b.x).Random(seed),
                y = new RangedFloat(a.y, b.y).Random(seed),
                z = new RangedFloat(a.z, b.z).Random(seed),
            };

            return this;
        }

        public static RangedVector3 Clamp(Vector3 value, Vector3 a, Vector3 b) => new RangedVector3(a, b, value);

        public static RangedVector3 InverseLerp(Vector3 a, Vector3 b, Vector3 value) => new RangedVector3(a, b).InverseLerp(value);

        public static RangedVector3 Lerp(Vector3 a, Vector3 b, float t) => new RangedVector3(a, b).Lerp(t);

        public static RangedVector3 Random(Vector3 a, Vector3 b) => new RangedVector3(b, a).Random();

        public static RangedVector3 Random(Vector3 a, Vector3 b, int seed) => new RangedVector3(b, a).Random(seed);
    }
}