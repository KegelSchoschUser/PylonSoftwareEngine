/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using Vortice.Direct2D1;
using Vortice.Mathematics;

namespace PylonSoftwareEngine.UI.Drawing
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
            br = g.RenderTarget.CreateSolidColorBrush(color.ToVorticeColor());
        }
    }
}
