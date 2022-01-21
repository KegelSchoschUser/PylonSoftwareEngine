using System;
using System.Drawing;

using System.Runtime.InteropServices;

namespace PylonGameEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RGBColor
    {

        public float R;
        public float G;
        public float B;
        public float A;


        public static Random r = new Random();

        public static readonly RGBColor Transparent = new RGBColor(0, 0, 0, 0);
        public static readonly RGBColor White = new RGBColor(1, 1, 1, 1);
        public static readonly RGBColor Black = new RGBColor(0, 0, 0, 1);
        public static readonly RGBColor LightGray = new RGBColor(0.8f, 0.8f, 0.8f, 1);
        public static readonly RGBColor Gray = new RGBColor(0.5f, 0.5f, 0.5f, 1);
        public static readonly RGBColor DarkGray = new RGBColor(0.3f, 0.3f, 0.3f, 1);
        public static readonly RGBColor Red = new RGBColor(1, 0, 0, 1);
        public static readonly RGBColor Green = new RGBColor(0, 1, 0, 1);
        public static readonly RGBColor Blue = new RGBColor(0, 0, 1, 1);
        public static readonly RGBColor PylonOrange = new RGBColor(1, 0.537254902f, 0, 1);



        public RGBColor(float brightness)
        {
            R = brightness;
            G = brightness;
            B = brightness;
            A = 1f;
        }

        public RGBColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
            A = 1f;
        }


        public RGBColor(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static RGBColor From255Range(int r, int g, int b)
        {
            return new RGBColor((r / 255f), (g / 255f), (b / 255f), 1f);
        }

        public static RGBColor From255Range(int r, int g, int b, int a)
        {
            return new RGBColor((r / 255f), (g / 255f), (b / 255f), (a / 255f));
        }

        public static RGBColor GetRandomColor()
        {
            return new RGBColor(r.Next(0, 1000) / 1000f, r.Next(0, 1000) / 1000f, r.Next(0, 1000) / 1000f);
        }

        public static RGBColor GetGrayScale(float lum)
        {
            return new RGBColor((byte)(lum * 255f), (byte)(lum * 255f), (byte)(lum * 255f));
        }

        //public Vector4 ToVector4()
        //{
        //    return new Vector4(R, G, B, A);
        //}

        public string ToHexString(bool Alpha = false)
        {
            byte a = (byte)(A * 255f);
            byte r = (byte)(R * 255f);
            byte g = (byte)(G * 255f);
            byte b = (byte)(B * 255f);

            string Output = "#";
            if(Alpha)
                Output += a.ToString("X2");
            Output += r.ToString("X2");
            Output += g.ToString("X2");
            Output += b.ToString("X2");

            return Output;
        }

        public static RGBColor FromHexString(string HexString)
        {
            RGBColor Output = new RGBColor(0, 0, 0);
            if (HexString.StartsWith('#') && HexString.Length >= 7 && HexString.Length <= 9)
            {
                if (HexString.Length == 9)
                {
                    Output.A = byte.Parse(HexString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                    Output.R = byte.Parse(HexString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                    Output.G = byte.Parse(HexString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                    Output.B = byte.Parse(HexString.Substring(7, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                }
                else
                {
                    Output.R = byte.Parse(HexString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                    Output.G = byte.Parse(HexString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                    Output.B = byte.Parse(HexString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                }
            }
            return Output;
        }

        public static bool operator ==(RGBColor left, RGBColor right)
        {
            bool same = true;
            if (left.R != right.R)
            {
                same = false;
            }

            if (left.G != right.G)
            {
                same = false;
            }

            if (left.B != right.B)
            {
                same = false;
            }

            if (left.A != right.A)
            {
                same = false;
            }

            return same;
        }

        public Color ToMSColor()
        {
            return System.Drawing.Color.FromArgb((int)(A * 255f), (int)(R * 255f), (int)(G * 255f), (int)(B * 255f));
        }

        public Vortice.Mathematics.Color ToVorticeColor()
        {
            return new Vortice.Mathematics.Color(R, G, B, A);
        }

        public static bool operator !=(RGBColor left, RGBColor right)
        {
            bool same = !(left == right);
            return same;
        }

        public override string ToString()
        {
            return "{" + $"{R.ToString("0.000")}, {G.ToString("0.000")}, {B.ToString("0.000")}, {A.ToString("0.000")}" + "}";
        }

        public float GetBrightness()
        {
            float r = R;
            float g = G;
            float b = B;

            float max, min;

            max = r; min = r;

            if (g > max)
            {
                max = g;
            }

            if (b > max)
            {
                max = b;
            }

            if (g < min)
            {
                min = g;
            }

            if (b < min)
            {
                min = b;
            }

            return (max + min) / 2;
        }

        /// <summary>
        ///       Returns the Hue-Saturation-Lightness (HSL) hue
        ///       value, in degrees, for this <see cref='System.Drawing.Color'/> .  
        ///       If R == G == B, the hue is meaningless, and the return value is 0.
        /// </summary>
        public float GetHue()
        {
            if (R == G && G == B)
            {
                return 0; // 0 makes as good an UNDEFINED value as any
            }

            float r = R;
            float g = G;
            float b = B;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max)
            {
                max = g;
            }

            if (b > max)
            {
                max = b;
            }

            if (g < min)
            {
                min = g;
            }

            if (b < min)
            {
                min = b;
            }

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue;
        }

        public float GetSaturation()
        {
            float r = R;
            float g = G;
            float b = B;

            float max, min;
            float l, s = 0;

            max = r; min = r;

            if (g > max)
            {
                max = g;
            }

            if (b > max)
            {
                max = b;
            }

            if (g < min)
            {
                min = g;
            }

            if (b < min)
            {
                min = b;
            }

            // if max == min, then there is no color and
            // the saturation is zero.
            //
            if (max != min)
            {
                l = (max + min) / 2;

                if (l <= .5)
                {
                    s = (max - min) / (max + min);
                }
                else
                {
                    s = (max - min) / (2 - max - min);
                }
            }
            return s;
        }





        public static implicit operator Vortice.Mathematics.Color(RGBColor c) => new Vortice.Mathematics.Color(c.R, c.G, c.B, c.A);
        public static explicit operator RGBColor(Vortice.Mathematics.Color c) => new RGBColor(c.R, c.G, c.B, c.A);
    }
}
