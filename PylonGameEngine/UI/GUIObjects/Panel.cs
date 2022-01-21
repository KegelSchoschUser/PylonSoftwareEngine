using PylonGameEngine.GameWorld;
using PylonGameEngine.Mathematics;
using PylonGameEngine.UI.Drawing;

namespace PylonGameEngine.GUI.GUIObjects
{
    public class Panel : GUIObject
    {
        private string _Text = "";
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                QueueDraw();
            }
        }

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

        public override void OnDraw(Graphics g)
        {
            g.Clear(RGBColor.Transparent);
            var p = g.CreatePen(RGBColor.White, 3f);
            var b = g.CreateSolidBrush(RGBColor.From255Range(40, 40, 40));

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

            p.Color = new RGBColor(1, 1, 1);

            g.DrawRoundedRectangle(p, new Vector2(EdgeX, EdgeY));
        }
    }
}
