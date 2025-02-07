/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20250206

namespace Devloader.Extensions
{
    public static class RealExtension
    {
        public static bool IsInRange(this float value, float min, float max, bool includeMin = true, bool includeMax = true) => (includeMin ? value >= min : value > min) && (includeMax ? value <= max : value < max);

        public static string SecondsToMinutes(this float value)
        {
            int seconds = (int)value;
            int minutes = seconds / 60;
            seconds %= 60;

            return minutes + ":" + ((seconds <= 9) ? "0" + seconds.ToString() : seconds.ToString());
        }

        public static string DistanceToString(this double distance, int decimalCount = 2)
        {
            string distanceStr = "";

            if (distance >= 1000)
            {
                distance /= 1000;
                distanceStr += distance.ToString("N" + decimalCount) + "km";
            }
            else
                distanceStr += distance.ToString("N" + decimalCount) + "m";

            return distanceStr;
        }
    }
}