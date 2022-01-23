using System;
using System.Drawing;

using System.Runtime.InteropServices;

namespace PylonGameEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HSVColor
    {

        public float H;
        public float S;
        public float V;
        public float A;


        public static Random r = new Random();

        public HSVColor(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
            A = 1f;
        }


        public HSVColor(float h, float s, float v, float a)
        {
            H = h;
            S = s;
            V = v;
            A = a;
        }

        public static HSVColor GetRandomColor()
        {
            return new HSVColor(r.Next(0, 360) / 360f, r.Next(0, 1000) / 10f, r.Next(0, 1000) / 10f);
        }

        public static bool operator ==(HSVColor left, HSVColor right)
        {
            bool same = true;
            if (left.H != right.H)
            {
                same = false;
            }

            if (left.S != right.S)
            {
                same = false;
            }

            if (left.V != right.V)
            {
                same = false;
            }

            if (left.A != right.A)
            {
                same = false;
            }

            return same;
        }

        public static bool operator !=(HSVColor left, HSVColor right)
        {
            bool same = !(left == right);
            return same;
        }

        public override string ToString()
        {
            return "{" + $"{H.ToString("0.000")}, {S.ToString("0.000")}, {V.ToString("0.000")}, {A.ToString("0.000")}" + "}";
        }

        public RGBColor ToRGB()
        {            
            // https://github.com/rivy/OpenPDN

            // HsvColor contains values scaled as in the color wheel:

            float h;
            float s;
            float v;

            float r = 0f;
            float g = 0f;
            float b = 0f;

            // Scale Hue to be between 0 and 360. Saturation
            // and value scale to be between 0 and 1.
            h = H % 360f;
            s = S / 100f;
            v = V / 100f;

            if (s == 0)
            {
                // If s is 0, all colors are the same.
                // This is some flavor of gray.
                r = v;
                g = v;
                b = v;
            }
            else
            {
                float p;
                float q;
                float t;

                float fractionalSector;
                int sectorNumber;
                float sectorPos;
                // The color wheel consists of 6 sectors.
                // Figure out which sector you're in.
                sectorPos = h / 60f;
                sectorNumber = (int)(Mathf.Floor(sectorPos));

                // get the fractional part of the sector.
                // That is, how many degrees into the sector
                // are you?
                fractionalSector = sectorPos - sectorNumber;

                // Calculate values for the three axes
                // of the color. 
                p = v * (1 - s);
                q = v * (1 - (s * fractionalSector));
                t = v * (1 - (s * (1 - fractionalSector)));

                // Assign the fractional colors to r, g, and b
                // based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }
            // return an RgbColor structure, with values scaled
            // to be between 0 and 255.
            return new RGBColor(r, g ,b);
        }        

        
    }
}
