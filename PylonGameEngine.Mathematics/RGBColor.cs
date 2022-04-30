using System;
using System.Collections.Generic;
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

        public RGBColor(RGBColor BaseColor, float a)
        {
            R = BaseColor.R;
            G = BaseColor.G;
            B = BaseColor.B;
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
            if (Alpha)
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
            return new Vortice.Mathematics.Color((int)(R * 255), (int)(G * 255), (int)(B * 255), (int)(A * 255));
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

        public HSVColor ToHSV()
        {
            // https://github.com/rivy/OpenPDN

            float min;
            float max;
            float delta;

            float r = this.R;
            float g = this.G;
            float b = this.B;

            float h;
            float s;
            float v;

            min = Mathf.Min(Mathf.Min(r, g), b);
            max = Mathf.Max(Mathf.Max(r, g), b);
            v = max;
            delta = max - min;
            if (max == 0 || delta == 0)
            {
                // R, G, and B must be 0, or all the same.
                // In this case, S is 0, and H is undefined.
                // Using H = 0 is as good as any...
                s = 0;
                h = 0;
            }
            else
            {
                s = delta / max;
                if (r == max)
                {
                    // Between Yellow and Magenta
                    h = (g - b) / delta;
                }
                else if (g == max)
                {
                    // Between Cyan and Yellow
                    h = 2f + (b - r) / delta;
                }
                else
                {
                    // Between Magenta and Cyan
                    h = 4f + (r - g) / delta;
                }

            }
            // Scale h to be between 0 and 360. 
            // This may require adding 360, if the value
            // is negative.
            h *= 60f;

            if (h < 0)
            {
                h += 360f;
            }

            return new HSVColor(h, s * 100f, v * 100f);
        }

        public static List<RGBColor> ColorPalette = new List<RGBColor>()
        {
            new RGBColor(1f, 1f, 1f, 1f)          ,
            new RGBColor(0.99f, 0.96f, 0.02f, 1f) ,
            new RGBColor(1f, 0.39f, 0.02f, 1f)    ,
            new RGBColor(0.86f, 0.03f, 0.03f, 1f) ,
            new RGBColor(0.94f, 0.03f, 0.52f, 1f) ,
            new RGBColor(0.28f, 0f, 0.64f, 1f)    ,
            new RGBColor(0f, 0f, 0.83f, 1f)       ,
            new RGBColor(0f, 0.67f, 0.91f, 1f)    ,
            new RGBColor(0.13f, 0.72f, 0.08f, 1f) ,
            new RGBColor(0f, 0.39f, 0.06f, 1f)    ,
            new RGBColor(0.75f, 0.75f, 0.75f, 1f)
        };

        public static RGBColor GetColorFromPalette(int index)
        {
            return ColorPalette[index % ColorPalette.Count];
        }


        public static implicit operator Vortice.Mathematics.Color(RGBColor c) => new Vortice.Mathematics.Color(c.R, c.G, c.B, c.A);
        public static explicit operator RGBColor(Vortice.Mathematics.Color c) => new RGBColor(c.R, c.G, c.B, c.A);

        public static implicit operator HSVColor(RGBColor v) => v.ToHSV();
        public static implicit operator RGBColor(HSVColor v) => v.ToRGB();
    }
}
