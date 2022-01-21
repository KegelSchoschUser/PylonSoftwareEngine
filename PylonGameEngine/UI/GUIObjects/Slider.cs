using PylonGameEngine.GameWorld;
using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.UI.Drawing;
using System;

namespace PylonGameEngine.GUI.GUIObjects
{
    public class Slider : GUIObject
    {
        public float Value = 0f;
        public float Minimum = 0f;
        public float Maximum = 1f;


        public override void UpdateTick()
        {
            if (MouseEnter || MouseLeave || Focused || FocusedLost)
                QueueDraw();
            if (Focused && Mouse.LeftButtonPressed())
            {
                Value = Mathf.Lerp(Minimum, Maximum, MouseLocal.X / Transform.Size.X);
                Mathf.Clamp(Value, Minimum, Maximum);
            }
        }

        public override void OnDraw(Graphics g)
        {
            g.Clear(RGBColor.Transparent);

            var p = g.CreatePen(RGBColor.White, 5f);
            var b = g.CreateSolidBrush(RGBColor.Black);


            g.DrawLine(p, new Vector2(0, Transform.Size.Y / 2f), new Vector2(Transform.Size.X, Transform.Size.Y / 2f));

            if (MouseHover)
            {
                b.Color = new RGBColor(1, 0, 0);
            }

            if (Focused)
            {
                b.Color = new RGBColor(0, 1, 1);
            }

            if (Value <= Minimum)
                g.FillRectangle(b, new Mathematics.Vector2(0, 0), new Vector2(10f, Transform.Size.Y));
            else if (Value >= Maximum)
                g.FillRectangle(b, new Mathematics.Vector2(Transform.Size.X - 10f, 0), new Vector2(10f, Transform.Size.Y));
            else
                g.FillRectangle(b, new Mathematics.Vector2(Transform.Size.X / 100f * ((Value - Minimum) / (Maximum - Minimum) * 100f), 0), new Vector2(10f, Transform.Size.Y));
        }

        private static Random random = new Random(2216);
        public void SetRandomValue()
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (Maximum - Minimum) + Minimum);
            Value = (float)val;
        }
    }
}
