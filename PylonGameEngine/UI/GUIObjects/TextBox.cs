using PylonGameEngine.GameWorld;
using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.UI;
using PylonGameEngine.UI.Drawing;

namespace PylonGameEngine.GUI.GUIObjects
{
    public class TextBox : GUIObject
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

        public override void OnDraw(Graphics g)
        {
            g.Clear(RGBColor.Transparent);
            var p = g.CreatePen(RGBColor.White, 3f);
            var b = g.CreateSolidBrush(RGBColor.From255Range(40, 40, 40));


            g.FillRectangle(b);
            if (MouseHover)
            {
                p.Color = new RGBColor(1, 0, 0);
            }

            if (Focused)
            {
                p.Color = new RGBColor(0, 1, 1);
            }

            g.DrawRectangle(p);

            var measure = g.MeasureText(Text, Font, Transform.Size - new Vector2(4), Enums.TextAlignment.Leading, Enums.ParagraphAlignment.Near, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap);
            g.DrawText(Text, Font, new Vector2(4), Transform.Size - new Vector2(8), Enums.TextAlignment.Leading, Enums.ParagraphAlignment.Near, Enums.ReadingDirection.LeftToRight, Enums.WordWrapping.Wrap);

            p.Color = Font.Color;
            g.FillRectangle(p.ToSolidBrush(), measure.CursorPosition, new Vector2(3, measure.CursorSizeY));
        }

        public override void UpdateTick()
        {
            if (MouseEnter || MouseLeave || Focused || FocusedLost)
                QueueDraw();

            if (!Focused)
                return;

            foreach (var character in Keyboard.CharacterKeys)
            {
                //char character = KeyCodes.ToChar(item, Keyboard.KeyPressed(KeyboardKey.Shift), false, false);
                System.Console.WriteLine(character);
                if (character == '\b')
                {
                    if (Text.Length > 0)
                        Text = Text.Remove(Text.Length - 1);
                }
                else if (character != '\0')
                {
                    Text += character;
                }
            }
        }
    }
}
