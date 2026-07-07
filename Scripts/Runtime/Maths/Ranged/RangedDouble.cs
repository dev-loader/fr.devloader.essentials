/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

namespace Devloader.Maths
{
    [System.Serializable]
    public class RangedDouble : AbstractRanged<double, RangedDouble>
    {
        public RangedDouble(double aBound, double bBound) : base(aBound, bBound) { }

        public RangedDouble(double aBound, double bBound, double currentValue) : base(aBound, bBound, currentValue) { }

        public RangedDouble(RangedDouble self) : base(self) { }

        public RangedDouble(RangedDouble self, double value) : base(self, value) { }

        public override RangedDouble Clamp(double value)
        {
            currentValue = System.Math.Clamp(value, a, b);
            return this;
        }

        public override RangedDouble InverseLerp(double value)
        {
            currentValue = (value - a) / (b - a);
            return this;
        }

        public override RangedDouble Lerp(float t)
        {
            currentValue = (b - a) * t + a;
            return this;
        }

        public override RangedDouble Random()
        {
            System.Random rand = new System.Random();
            currentValue = rand.NextDouble() * (b - a) + a;

            return this;
        }

        public override RangedDouble Random(int seed)
        {
            System.Random rand = new System.Random(seed);
            currentValue = rand.NextDouble() * (b - a) + a;

            return this;
        }

        public static RangedDouble Clamp(double value, double a, double b) => new RangedDouble(a, b, value);

        public static RangedDouble InverseLerp(double a, double b, double value) => new RangedDouble(a, b).InverseLerp(value);

        public static RangedDouble Lerp(double a, double b, float t) => new RangedDouble(a, b).Lerp(t);

        public static RangedDouble Random(double a, double b) => new RangedDouble(b, a).Random();

        public static RangedDouble Random(double a, double b, int seed) => new RangedDouble(b, a).Random(seed);
    }
}