using System;

namespace Utils.FloatOperations
{
    public static class Comparison
    {
        public const float Tolerance = 1e-6f;

        public static bool Equal(this float value, float other)
        {
            return Math.Abs(value - other) < Tolerance;
        }
    }
}