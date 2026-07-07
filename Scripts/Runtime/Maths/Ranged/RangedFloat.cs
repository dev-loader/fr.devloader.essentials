/// <summary>
/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260319
/// </summary>

namespace Devloader.Maths
{
    [System.Serializable]
    public class RangedFloat : AbstractRanged<float, RangedFloat>
    {
        public RangedFloat(float aBound, float bBound) : base(aBound, bBound) { }

        public RangedFloat(float aBound, float bBound, float currentValue) : base(aBound, bBound, currentValue) { }

        public RangedFloat(RangedFloat self) : base(self) { }

        public RangedFloat(RangedFloat self, float value) : base(self, value) { }

        public override RangedFloat Clamp(float value)
        {
            currentValue = UnityEngine.Mathf.Clamp(value, a, b);
            return this;
        }

        public override RangedFloat InverseLerp(float value)
        {
            currentValue = UnityEngine.Mathf.InverseLerp(a, b, value);
            return this;
        }

        public override RangedFloat Lerp(float t)
        {
            currentValue = UnityEngine.Mathf.Lerp(a, b, t);
            return this;
        }

        public override RangedFloat Random()
        {
            currentValue = UnityEngine.Random.Range(a, b);
            return this;
        }

        public override RangedFloat Random(int seed)
        {
            UnityEngine.Random.InitState(seed);
            currentValue = UnityEngine.Random.Range(a, b);

            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
            return this;
        }

        public static RangedFloat Clamp(float value, float a, float b) => new RangedFloat(a, b, value);

        public static RangedFloat InverseLerp(float a, float b, float value) => new RangedFloat(a, b).InverseLerp(value);

        public static RangedFloat Lerp(float a, float b, float t) => new RangedFloat(a, b).Lerp(t);

        public static RangedFloat Random(float a, float b) => new RangedFloat(b, a).Random();

        public static RangedFloat Random(float a, float b, int seed) => new RangedFloat(b, a).Random(seed);
    }
}