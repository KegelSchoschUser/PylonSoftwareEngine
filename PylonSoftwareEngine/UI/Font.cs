/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.UI.Drawing;

namespace PylonSoftwareEngine.UI
{
    public class Font
    {
        public string FontFamilyName;
        public float FontSize;
        public Enums.FontWeight FontWeight;
        public Enums.FontStyle FontStyle;
        public RGBColor Color = RGBColor.White;
        public bool EnableColorFont = true;


        public Font(string fontFamilyName = "Calibri", float fontSize = 24f, RGBColor? color = null, bool enableColorFont = true, Enums.FontWeight fontWeight = Enums.FontWeight.Medium, Enums.FontStyle fontStyle = Enums.FontStyle.Normal)
        {
            FontFamilyName = fontFamilyName;
            FontSize = fontSize;
            FontWeight = fontWeight;
            FontStyle = fontStyle;
            EnableColorFont = enableColorFont;
            if (color != null)
                Color = (RGBColor)color;
        }

        public Font Clone()
        {
            return new Font(FontFamilyName, FontSize, Color, EnableColorFont, FontWeight, FontStyle);
        }
    }
}
