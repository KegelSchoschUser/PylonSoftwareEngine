/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;

namespace PylonSoftwareEngine.Mathematics
{
    public static class Mathf
    {
        public const float Deg2Rad = 0.0174532925f;
        public const float Rad2Deg = 1f / Deg2Rad;
        public const float PI = 3.1415926535897931f;

        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            by = Clamp(by, 0, 1);
            return firstFloat * (1 - by) + secondFloat * by;
        }

        public static float LerpArray(float[] values, int index, float by)
        {
            if (values.Length < 2)
                throw new ArgumentOutOfRangeException("values");

            float first = values[index];
            float second;
            if (index + 1 == values.Length)
                second = values[index];
            else
                second = values[index + 1];

            return Lerp(first, second, by);
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

        public static float Sin(float value) => (float)Math.Sin(value);
        public static float Cos(float value) => (float)Math.Cos(value);
        public static float Tan(float value) => (float)Math.Tan(value);
        public static float Abs(float value) => (float)Math.Abs(value);
        public static int Round(float value) => (int)Math.Round(value);
        public static int Floor(float value) => (int)Math.Floor(value);
        public static int Ceiling(float value) => (int)Math.Ceiling(value);
        public static float Pow(float x, float y) => (float)Math.Pow(x, y);
        public static int Truncate(float value) => (int)Math.Truncate(value);
    }
}
