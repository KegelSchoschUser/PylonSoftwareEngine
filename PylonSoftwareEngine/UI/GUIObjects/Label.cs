using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.UI;
using PylonSoftwareEngine.UI.Drawing;

namespace PylonSoftwareEngine.UI.GUIObjects
{
    public class Label : GUIObject
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
                if (AutoSize)
                    Transform.Size = Graphics.MeasureText(value, _Font, _XAlign, _YAlign, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap).LayoutSize;
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
                if (AutoSize)
                    Transform.Size = Graphics.MeasureText(_Text, _Font, _XAlign, _YAlign, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap).LayoutSize;
                QueueDraw();
            }
        }

        private Enums.TextAlignment _XAlign;
        public Enums.TextAlignment XAlign
        {
            get
            {
                return _XAlign;
            }
            set
            {
                _XAlign = value;
                if (AutoSize)
                    Transform.Size = Graphics.MeasureText(_Text, _Font, _XAlign, _YAlign, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap).LayoutSize;
                QueueDraw();
            }
        }

        private Enums.ParagraphAlignment _YAlign;
        public Enums.ParagraphAlignment YAlign
        {
            get
            {
                return _YAlign;
            }
            set
            {
                _YAlign = value;
                if (AutoSize)
                    Transform.Size = Graphics.MeasureText(_Text, _Font, _XAlign, _YAlign, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap).LayoutSize;
                QueueDraw();
            }
        }


        private bool _AutoSize = true;
        public bool AutoSize
        {
            get
            {
                return _AutoSize;
            }
            set
            {
                _AutoSize = value;
                QueueDraw();
            }
        }


        public Label(string text, Font font, Enums.TextAlignment xAlign = Enums.TextAlignment.Leading, Enums.ParagraphAlignment yAlign = Enums.ParagraphAlignment.Near)
        {
            Text = text;
            Font = font;
            XAlign = xAlign;
            YAlign = yAlign;
        }

        public Label(string text, Enums.TextAlignment xAlign = Enums.TextAlignment.Leading, Enums.ParagraphAlignment yAlign = Enums.ParagraphAlignment.Near)
        {
            Text = text;
            Font = new Font();
            XAlign = xAlign;
            YAlign = yAlign;
        }

        public override void OnDraw(Graphics g)
        {
            g.Clear(RGBColor.Transparent);

            g.DrawText(Text, Font, XAlign, YAlign, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap);
        }
    }
}
