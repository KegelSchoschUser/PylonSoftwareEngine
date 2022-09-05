namespace PylonSoftwareEngine.UI.Drawing
{
    public static class Enums
    {
        public enum InterpolationMode
        {
            Nearest,
            Linear
        }

        public enum FontWeight
        {
            Thin = 100,
            ExtraLight = 200,
            UltraLight = 200,
            Light = 300,
            SemiLight = 350,
            Normal = 400,
            Regular = 400,
            Medium = 500,
            DemiBold = 600,
            SemiBold = 600,
            Bold = 700,
            ExtraBold = 800,
            UltraBold = 800,
            Black = 900,
            Heavy = 900,
            ExtraBlack = 950,
            UltraBlack = 950
        }

        public enum FontStyle
        {
            Normal = 0,
            Oblique = 1,
            Italic = 2
        }

        public enum TextAlignment
        {
            Leading = 0,
            Trailing = 1,
            Center = 2,
            Justified = 3
        }

        public enum ParagraphAlignment
        {
            Near = 0,
            Far = 1,
            Center = 2
        }
        public enum ReadingDirection
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }

        public enum WordWrapping
        {
            Wrap = 0,
            NoWrap = 1,
            EmergencyBreak = 2,
            WholeWord = 3,
            Character = 4
        }

        public enum CapStyle
        {
            Flat = 0,
            Square = 1,
            Round = 2,
            Triangle = 3
        }

        public enum LineJoin
        {
            Miter = 0,
            Bevel = 1,
            Round = 2,
            MiterOrBevel = 3
        }

        public enum DashStyle
        {
            Solid = 0,
            Dash = 1,
            Dot = 2,
            DashDot = 3,
            DashDotDot = 4,
            Custom = 5
        }


    }
}
