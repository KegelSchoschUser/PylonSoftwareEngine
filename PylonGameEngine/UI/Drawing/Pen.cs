﻿using PylonGameEngine.Mathematics;
using Vortice.Direct2D1;
using Vortice.Mathematics;

namespace PylonGameEngine.UI.Drawing
{
    public class Pen
    {
        internal ID2D1SolidColorBrush br;
        internal ID2D1StrokeStyle StrokeStyle;
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

        public float Width = 1f;

        public Pen(Graphics g, RGBColor color)
        {
            Initialize(g, color, 1f, new StrokeStyle());
        }

        public Pen(Graphics g, RGBColor color, float width)
        {
            Initialize(g, color, width, new StrokeStyle());
        }

        public Pen(Graphics g, RGBColor color, float width, StrokeStyle style)
        {
            Initialize(g, color, width, style);
        }

        public Pen(Graphics g, RGBColor color, StrokeStyle style)
        {
            Initialize(g, color, 1f, style);
        }


        private void Initialize(Graphics g, RGBColor color, float width, StrokeStyle style)
        {
            _Color = color;
            Width = width;
            br = g.RenderTarget.CreateSolidColorBrush(color.ToMSColor());

            var strokestyleproperties = new StrokeStyleProperties();
            strokestyleproperties.StartCap = style.StartCap;
            strokestyleproperties.EndCap = style.EndCap;
            strokestyleproperties.DashCap = style.DashCap;
            strokestyleproperties.LineJoin = style.LineJoin;
            strokestyleproperties.MiterLimit = style.MiterLimit;
            strokestyleproperties.DashStyle = style.DashStyle;
            strokestyleproperties.DashOffset = style.DashOffset;
            StrokeStyle = g.RenderTarget.Factory.CreateStrokeStyle(strokestyleproperties);
        }

        public SolidBrush ToSolidBrush()
        {
            return new SolidBrush() { br = this.br, Color = Color };
        }
    }

    public class StrokeStyle
    {
        public CapStyle StartCap;
        public CapStyle EndCap;
        public CapStyle DashCap;
        public LineJoin LineJoin;
        public float MiterLimit;
        public DashStyle DashStyle;
        public float DashOffset;

        public StrokeStyle(Enums.CapStyle startCap = Enums.CapStyle.Flat,
                           Enums.CapStyle endCap = Enums.CapStyle.Flat,
                           Enums.CapStyle dashCap = Enums.CapStyle.Flat,
                           Enums.LineJoin lineJoin = Enums.LineJoin.Miter,
                           float miterLimit = 0f,
                           Enums.DashStyle dashStyle = Enums.DashStyle.Solid,
                           float dashOffset = 0f)
        {
            StartCap = (CapStyle)startCap;
            EndCap = (CapStyle)endCap;
            DashCap = (CapStyle)dashCap;
            LineJoin = (LineJoin)lineJoin;
            MiterLimit = miterLimit;
            DashStyle = (DashStyle)dashStyle;
            DashOffset = dashOffset;
        }
    }
}
