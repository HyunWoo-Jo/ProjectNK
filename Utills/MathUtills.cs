using UnityEngine;

namespace N.Utills
{
    public static class MathUtills
    {
        public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax) {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }

        public static float RemapClamp(float value, float fromMin, float fromMax, float toMin, float toMax) {
           return Remap(Mathf.Clamp(value, fromMin, fromMax),fromMin, fromMax, toMin, toMax);
        }
    }
}
