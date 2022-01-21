using PylonGameEngine.Mathematics;
using PylonGameEngine.UI.Drawing;

namespace PylonGameEngine.UI
{
    public class Font
    {
        public string FontFamilyName;
        public float FontSize;
        public Enums.FontWeight FontWeight;
        public Enums.FontStyle FontStyle;
        public RGBColor Color = RGBColor.White;


        public Font(string fontFamilyName = "Calibri", float fontSize = 24f, RGBColor? color = null, Enums.FontWeight fontWeight = Enums.FontWeight.Medium, Enums.FontStyle fontStyle = Enums.FontStyle.Normal)
        {
            FontFamilyName = fontFamilyName;
            FontSize = fontSize;
            FontWeight = fontWeight;
            FontStyle = fontStyle;
            if (color != null)
                Color = (RGBColor)color;
        }
    }
}
