using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.UI.Drawing;

namespace PylonSoftwareEngine.UI.GUIObjects
{
    public class Panel : GUIObject
    {
        private float _EdgeSize = 0f;
        public float EdgeSize
        {
            get
            {
                return _EdgeSize;
            }
            set
            {
                _EdgeSize = value;
                QueueDraw();
            }
        }

        public float EdgeSizePixel
        {
            get
            {
                return EdgeSize * (Transform.Size.Y / 2f);
            }
            set
            {
                EdgeSize = value / (Transform.Size.Y / 2f);
            }
        }

        private float _EdgeThickness = 1f;
        public float EdgeThickness
        {
            get
            {
                return _EdgeThickness;
            }
            set
            {
                _EdgeThickness = value;
                QueueDraw();
            }
        }

        private RGBColor _Color = RGBColor.Black;
        public RGBColor Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                QueueDraw();
            }
        }

        private RGBColor _EdgeColor = RGBColor.Red;
        public RGBColor EdgeColor
        {
            get
            {
                return _EdgeColor;
            }
            set
            {
                _EdgeColor = value;
                QueueDraw();
            }
        }


        public override void OnDraw(Graphics g)
        {
            g.Clear(RGBColor.Transparent);
            var b = g.CreateSolidBrush(Color);
            var p = g.CreatePen(EdgeColor, EdgeThickness);

            float EdgeX;
            float EdgeY;
            if (Transform.Size.X >= Transform.Size.Y)
            {
                EdgeX = EdgeSize * (Transform.Size.Y / 2f);
                EdgeY = EdgeSize * (Transform.Size.Y / 2f);
            }
            else
            {
                EdgeX = EdgeSize * (Transform.Size.X / 2f);
                EdgeY = EdgeSize * (Transform.Size.X / 2f);
            }


            g.FillRoundedRectangle(b, new Vector2(EdgeX, EdgeY));

            g.DrawRoundedRectangle(p, new Vector2(EdgeX, EdgeY));
        }
    }
}
