/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Input;
using PylonSoftwareEngine.UI;
using PylonSoftwareEngine.UI.GUIObjects;
using System;
using System.Globalization;
using System.Linq;

namespace PylonSoftwareEngine.Extensions
{
    internal class FloatInputBox : GUIObject
    {
        private float _Value = 0;
        public float Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
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
        public override void OnDraw(UI.Drawing.Graphics g)
        {
            base.OnDraw(g);
        }

        public override void UpdateTick()
        {
            if (MouseEnter || MouseLeave || Focused || FocusedLost)
                QueueDraw();

            if (!Focused)
                return;

            string ValueText = Value.ToString(CultureInfo.CurrentCulture);

            foreach (var character in SceneContext.InputManager.Keyboard.CharacterKeys)
            {
                if (character == '\b')
                {
                    if (ValueText.Length > 0)
                        ValueText = ValueText.Remove(ValueText.Length - 1);
                }
                else if (character != '\0')
                {
                    char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ',' };
                    if (numbers.Contains(character))
                        ValueText += character;
                }
            }

            try
            {
                Value = float.Parse(ValueText);
            }
            catch (Exception)
            {

            }
        }
    }
}
