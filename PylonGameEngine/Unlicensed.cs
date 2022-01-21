using PylonGameEngine.GUI.GUIObjects;
using PylonGameEngine.Mathematics;
using PylonGameEngine.UI;
using PylonGameEngine.UI.Drawing;

namespace PylonGameEngine
{
    internal class Unlicensed : RenderCanvas
    {
        internal bool Active = true;
        private Font f;

        public Unlicensed()
        {
            f = new Font(color: Mathematics.RGBColor.Red);
        }

        public override void OnDraw(Graphics g)
        {
            if (Active)
            {
               // g.DrawBitmap(@"CoreContent\Logo.png", new System.Drawing.RectangleF(0, 0, 100, 100));
#if DEBUG
                g.DrawText("DEV BUILD", f, new Vector2(0, 0), new Vector2(100, 100), Enums.TextAlignment.Center);
#else
                g.DrawText("DEMO BUILD", f, new Vector2(0, 0), new Vector2(100, 100), Enums.TextAlignment.Center);
#endif
            }
        }
    }
}
