/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

using UnityEngine;

namespace Devloader.Maths
{
    [System.Serializable]
    public class RangedVector2 : AbstractRanged<Vector2, RangedVector2>
    {
        public RangedVector2(Vector2 aBound, Vector2 bBound) : base(aBound, bBound) { }

        public RangedVector2(Vector2 aBound, Vector2 bBound, Vector2 currentValue) : base(aBound, bBound, currentValue) { }

        public RangedVector2(RangedVector2 self) : base(self) { }

        public RangedVector2(RangedVector2 self, Vector2 value) : base(self, value) { }

        public override RangedVector2 Clamp(Vector2 value)
        {
            currentValue = new Vector2()
            {
                x = new RangedFloat(a.x, b.x, value.x),
                y = new RangedFloat(a.y, b.y, value.y),
            };

            return this;
        }

        public override RangedVector2 InverseLerp(Vector2 value)
        {
            currentValue = new Vector2()
            {
                x = new RangedFloat(a.x, b.x).InverseLerp(value.x),
                y = new RangedFloat(a.y, b.y).InverseLerp(value.y),
            };

            return this;
        }

        public override RangedVector2 Lerp(float t)
        {
            currentValue = new Vector2()
            {
                x = new RangedFloat(a.x, b.x).Lerp(t),
                y = new RangedFloat(a.y, b.y).Lerp(t),
            };

            return this;
        }

        public override RangedVector2 Random()
        {
            currentValue = new Vector2()
            {
                x = new RangedFloat(a.x, b.x).Random(),
                y = new RangedFloat(a.y, b.y).Random(),
            };

            return this;
        }

        public override RangedVector2 Random(int seed)
        {
            currentValue = new Vector2()
            {
                x = new RangedFloat(a.x, b.x).Random(seed),
                y = new RangedFloat(a.y, b.y).Random(seed),
            };

            return this;
        }

        public static RangedVector2 Clamp(Vector2 value, Vector2 a, Vector2 b) => new RangedVector2(a, b, value);

        public static RangedVector2 InverseLerp(Vector2 a, Vector2 b, Vector2 value) => new RangedVector2(a, b).InverseLerp(value);

        public static RangedVector2 Lerp(Vector2 a, Vector2 b, float t) => new RangedVector2(a, b).Lerp(t);

        public static RangedVector2 Random(Vector2 a, Vector2 b) => new RangedVector2(b, a).Random();

        public static RangedVector2 Random(Vector2 a, Vector2 b, int seed) => new RangedVector2(b, a).Random(seed);
    }
}