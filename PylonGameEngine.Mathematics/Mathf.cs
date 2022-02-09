using System;

namespace PylonGameEngine
{
    public static class Mathf
    {
        public const float Deg2Rad = 0.0174532925f;
        public const float Rad2Deg = 1f / Deg2Rad;
        public const float PI = 3.1415926535897931f;

        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        public static float LerpArray(float[] values, int index, float by)
        {
            if (values.Length < 2)
                throw new ArgumentOutOfRangeException("values");

            float first = values[index];
            float second;
            if(index + 1 == values.Length)
                second = values[0];
            else
                second = values[index + 1];

            return Mathf.Lerp(first, second, by);
        }

        public static float Clamp(float Value, float Minumum, float Maximum)
        {
            if (Value <= Minumum)
                return Minumum;
            else if (Value >= Maximum)
                return Maximum;
            else
                return Value;
        }

        public static float Max(float Value1, float Value2)
        {
            if (Value1 >= Value2)
                return Value1;
            else
                return Value2;
        }

        public static float Min(float Value1, float Value2)
        {
            if (Value1 <= Value2)
                return Value1;
            else
                return Value2;
        }


        public static (int, float) Max(float[] Values)
        {
            if (Values.Length < 2)
                throw new ArgumentOutOfRangeException();
            int index = -1;
            float value = float.MinValue;

            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i] > value)
                {
                    value = Values[i];
                    index = i;
                }
            }
            
            return (index, value);
        }

        public static (int, float) Min(float[] Values)
        {
            if (Values.Length < 2)
                throw new ArgumentOutOfRangeException();
            int index = -1;
            float value = float.MaxValue;

            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i] < value)
                {
                    value = Values[i];
                    index = i;
                }
            }

            return (index, value);
        }

        public static float Sin(float value) => (float)System.Math.Sin(value);
        public static float Cos(float value) => (float)System.Math.Cos(value);
        public static float Tan(float value) => (float)System.Math.Tan(value);
        public static float Abs(float value) => (float)System.Math.Abs(value);
        public static float Floor(float value) => (float)System.Math.Floor(value);
        public static float Pow(float x, float y) => (float)System.Math.Pow(x, y);
        public static int Truncate(float value) => (int)System.Math.Truncate(value);
    }
}
