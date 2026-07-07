/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

namespace Devloader.Maths
{
    [System.Serializable]
    public class RangedInt : AbstractRanged<int, RangedInt>
    {
        public RangedInt(int aBound, int bBound) : base(aBound, bBound) { }

        public RangedInt(int aBound, int bBound, int currentValue) : base(aBound, bBound, currentValue) { }

        public RangedInt(RangedInt self) : base(self) { }

        public RangedInt(RangedInt self, int value) : base(self, value) { }

        public override RangedInt Clamp(int value)
        {
            currentValue = System.Math.Clamp(value, a, b);
            return this;
        }

        public override RangedInt InverseLerp(int value)
        {
            currentValue = (value - a) / (b - a);
            return this;
        }

        public override RangedInt Lerp(float t)
        {
            currentValue = (int)((b - a) * t) + a;
            return this;
        }

        public override RangedInt Random()
        {
            System.Random rand = new System.Random();
            currentValue = rand.Next() * (b - a) + a;

            return this;
        }

        public override RangedInt Random(int seed)
        {
            System.Random rand = new System.Random(seed);
            currentValue = rand.Next() * (b - a) + a;

            return this;
        }

        public static RangedInt Clamp(int value, int a, int b) => new RangedInt(a, b, value);

        public static RangedInt InverseLerp(int a, int b, int value) => new RangedInt(a, b).InverseLerp(value);

        public static RangedInt Lerp(int a, int b, float t) => new RangedInt(a, b).Lerp(t);

        public static RangedInt Random(int a, int b) => new RangedInt(b, a).Random();

        public static RangedInt Random(int a, int b, int seed) => new RangedInt(b, a).Random(seed);
    }
}