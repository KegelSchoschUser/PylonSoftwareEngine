/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.UI;
using PylonSoftwareEngine.UI.Drawing;

namespace PylonSoftwareEngine.UI.GUIObjects
{
    public class Button : GUIObject
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

        private Font _Font = new Font();
        public Font Font
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
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

        private RGBColor _EdgeColorHover = RGBColor.Green;
        public RGBColor EdgeColorHover
        {
            get
            {
                return _EdgeColorHover;
            }
            set
            {
                _EdgeColorHover = value;
                QueueDraw();
            }
        }

        private RGBColor _EdgeColorClicked = RGBColor.Blue;
        public RGBColor EdgeColorClicked
        {
            get
            {
                return _EdgeColorClicked;
            }
            set
            {
                _EdgeColorClicked = value;
                QueueDraw();
            }
        }

        public delegate void Click(Button sender);
        public event Click OnClick;

        public Button()
        {
            OnClick += (sender) => { };
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

            if (MouseHover == false)
            {
                p.Color = EdgeColor;
            }
            else
            {
                if (LeftMousePressed)
                {
                    p.Color = EdgeColorClicked;
                }
                else
                {
                    p.Color = EdgeColorHover;
                }
            }

            g.DrawRoundedRectangle(p, new Vector2(EdgeX, EdgeY));
            g.DrawText(Text, Font, Enums.TextAlignment.Center, Enums.ParagraphAlignment.Center, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap);
        }

        public override void UpdateTick()
        {
            if (MouseEnter || MouseLeave || Focused || FocusedLost || LeftMouseClicked || LeftMousePressed || RightMouseClicked || RightMousePressed || MiddleMouseClicked || MiddleMouseClicked)
                QueueDraw();

            if (LeftMouseClicked)
                OnClick(this);
        }
    }
}
