﻿using PylonGameEngine.Mathematics;
using Vortice.Direct2D1;
using Vortice.Mathematics;

namespace PylonGameEngine.UI.Drawing
{
    public class SolidBrush
    {
        internal ID2D1SolidColorBrush br;
        private RGBColor _Color;
        public RGBColor Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                br.Color = new Color4(value.R, value.G, value.B, value.A);
            }
        }

        internal SolidBrush()
        {

        }

        public SolidBrush(Graphics g, RGBColor color)
        {
            _Color = color;
            br = g.RenderTarget.CreateSolidColorBrush(color.ToMSColor());
        }
    }
}
