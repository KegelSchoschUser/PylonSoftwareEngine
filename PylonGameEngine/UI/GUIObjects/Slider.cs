using PylonGameEngine.GameWorld;
using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.UI.Drawing;
using System;

namespace PylonGameEngine.GUI.GUIObjects
{
    public class Slider : GUIObject
    {
        public float _Value = 0f;
        public float _Minimum = 0f;
        public float _Maximum = 1f;

        public float Value
        {
            get { return _Value; }
            set
            {
                float Previous = _Value;
                _Value = Mathf.Clamp(value, Minimum, Maximum);
                OnValueChanged(this, Previous, _Value);
                QueueDraw();
            }
        }

        public float Minimum
        {
            get { return _Minimum; }
            set { _Minimum = value; QueueDraw(); }
        }

        public float Maximum
        {
            get { return _Maximum; }
            set { _Maximum = value; QueueDraw(); }
        }


        private RGBColor _SliderColor = RGBColor.From255Range(100, 100, 100);
        public RGBColor SliderColor
        {
            get { return _SliderColor; }
            set { _SliderColor = value; QueueDraw(); }
        }

        private RGBColor _KnobColor = RGBColor.PylonOrange;
        public RGBColor KnobColor
        {
            get { return _KnobColor; }
            set { _KnobColor = value; QueueDraw(); }
        }

        private RGBColor _KnobHoverColor = RGBColor.Red;
        public RGBColor KnobHoverColor
        {
            get { return _KnobHoverColor; }
            set { _KnobHoverColor = value; QueueDraw(); }
        }

        public delegate void ValueChanged(Slider sender, float Previous, float Current);
        public event ValueChanged OnValueChanged;

        public Slider()
        {
            OnValueChanged += (sender, p, c) => { };
        }

        public override void UpdateTick()
        {
            if (MouseEnter || MouseLeave || Focused || FocusedLost)
                QueueDraw();

            if (Focused && LeftMousePressed)
            {
                Value = Mathf.Lerp(Minimum, Maximum, MouseLocal.X / Transform.Size.X);
                Mathf.Clamp(Value, Minimum, Maximum);
            }
        }

        private Pen SliderPen;
        private Pen KnobPen;

        public override void OnDraw(Graphics g)
        {
            g.Clear(RGBColor.Transparent);

            if (SliderPen == null)
                SliderPen = new Pen(g, SliderColor, 3f);

            if (KnobPen == null)
                KnobPen = new Pen(g, KnobColor);

            if (MouseHover)
            {
                KnobPen.Color = KnobHoverColor;
            }
            else
            {
                KnobPen.Color = KnobColor;
            }

            float KnobRadius = 10f;
            float ActualWidth = g.Size.X - KnobRadius * 2f;

            Vector2 KnobPosition = new Vector2(ActualWidth / 100f * ((Value - Minimum) / (Maximum - Minimum) * 100f) + KnobRadius, g.Size.Y / 2f);

            g.DrawLine(SliderPen, new Vector2(KnobRadius, Transform.Size.Y / 2f), new Vector2(Transform.Size.X - KnobRadius, Transform.Size.Y / 2f));
            g.FillCircle(KnobPen.ToSolidBrush(), KnobPosition, KnobRadius);
            g.FillCircle(SliderPen.ToSolidBrush(), KnobPosition, KnobRadius / 1.5f);
            g.FillCircle(KnobPen.ToSolidBrush(), KnobPosition, KnobRadius / 4f);

            //if (Value <= Minimum)
            //    g.FillRectangle(b, new Mathematics.Vector2(0, 0), new Vector2(10f, Transform.Size.Y));
            //else if (Value >= Maximum)
            //    g.FillRectangle(b, new Mathematics.Vector2(Transform.Size.X - 10f, 0), new Vector2(10f, Transform.Size.Y));
            //else
            //    g.FillRectangle(b, new Mathematics.Vector2(Transform.Size.X / 100f * ((Value - Minimum) / (Maximum - Minimum) * 100f), 0), new Vector2(10f, Transform.Size.Y));
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
