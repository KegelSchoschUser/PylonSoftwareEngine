namespace PylonGameEngine.Input
{
    public static class KeyCodes
    {
        public static KeyboardKey ConvertToKey(int VK)
        {
            return (KeyboardKey)VK;
        }

        public static char ToChar(KeyboardKey key, bool shift, bool ctrl, bool alt)
        {
            char character = '\0';
            switch (key)
            {
                case KeyboardKey.Backspace:
                    character = '\b';
                    break;
                case KeyboardKey.Tab:
                    character = '\t';
                    break;
                case KeyboardKey.Enter:
                    character = '\n';
                    break;
                case KeyboardKey.Space:
                    character = ' ';
                    break;
                case KeyboardKey.Num0:
                    character = '0';
                    break;
                case KeyboardKey.Num1:
                    character = '1';
                    break;
                case KeyboardKey.Num2:
                    character = '2';
                    break;
                case KeyboardKey.Num3:
                    character = '3';
                    break;
                case KeyboardKey.Num4:
                    character = '4';
                    break;
                case KeyboardKey.Num5:
                    character = '5';
                    break;
                case KeyboardKey.Num6:
                    character = '6';
                    break;
                case KeyboardKey.Num7:
                    character = '7';
                    break;
                case KeyboardKey.Num8:
                    character = '8';
                    break;
                case KeyboardKey.Num9:
                    character = '9';
                    break;
                case KeyboardKey.A:
                    character = 'a';
                    break;
                case KeyboardKey.B:
                    character = 'b';
                    break;
                case KeyboardKey.C:
                    character = 'c';
                    break;
                case KeyboardKey.D:
                    character = 'd';
                    break;
                case KeyboardKey.E:
                    character = 'e';
                    break;
                case KeyboardKey.F:
                    character = 'f';
                    break;
                case KeyboardKey.G:
                    character = 'g';
                    break;
                case KeyboardKey.H:
                    character = 'h';
                    break;
                case KeyboardKey.I:
                    character = 'i';
                    break;
                case KeyboardKey.J:
                    character = 'j';
                    break;
                case KeyboardKey.K:
                    character = 'k';
                    break;
                case KeyboardKey.L:
                    character = 'l';
                    break;
                case KeyboardKey.M:
                    character = 'm';
                    break;
                case KeyboardKey.N:
                    character = 'n';
                    break;
                case KeyboardKey.P:
                    character = 'p';
                    break;
                case KeyboardKey.Q:
                    character = 'q';
                    break;
                case KeyboardKey.R:
                    character = 'r';
                    break;
                case KeyboardKey.S:
                    character = 's';
                    break;
                case KeyboardKey.T:
                    character = 't';
                    break;
                case KeyboardKey.U:
                    character = 'u';
                    break;
                case KeyboardKey.V:
                    character = 'v';
                    break;
                case KeyboardKey.W:
                    character = 'w';
                    break;
                case KeyboardKey.X:
                    character = 'x';
                    break;
                case KeyboardKey.Y:
                    character = 'y';
                    break;
                case KeyboardKey.Z:
                    character = 'z';
                    break;
                case KeyboardKey.O:
                    character = 'o';
                    break;
                case KeyboardKey.Numpad0:
                    character = '0';
                    break;
                case KeyboardKey.Numpad1:
                    character = '1';
                    break;
                case KeyboardKey.Numpad2:
                    character = '2';
                    break;
                case KeyboardKey.Numpad3:
                    character = '3';
                    break;
                case KeyboardKey.Numpad4:
                    character = '4';
                    break;
                case KeyboardKey.Numpad5:
                    character = '5';
                    break;
                case KeyboardKey.Numpad6:
                    character = '6';
                    break;
                case KeyboardKey.Numpad7:
                    character = '7';
                    break;
                case KeyboardKey.Numpad8:
                    character = '8';
                    break;
                case KeyboardKey.Numpad9:
                    character = '9';
                    break;
                case KeyboardKey.NumpadPlus:
                    character = '+';
                    break;
                case KeyboardKey.NumpadMinus:
                    character = '-';
                    break;
                case KeyboardKey.Comma:
                    character = ',';
                    break;
                case KeyboardKey.Minus:
                    character = '-';
                    break;
                case KeyboardKey.Dot:
                    character = '.';
                    break;
                default:
                    character = '\0';
                    break;
            }

            if (shift)
            {
                character = char.ToUpper(character);
            }
            return character;
        }
    }

    public enum KeyboardKey
    {
        None = 0,
        Backspace = 8,
        Tab = 9,
        Enter = 13,
        Shift = 16,
        Ctrl = 17,
        Alt = 18,
        CapsLock = 20,
        Space = 32,
        Num0 = 48,
        Num1 = 49,
        Num2 = 50,
        Num3 = 51,
        Num4 = 52,
        Num5 = 53,
        Num6 = 54,
        Num7 = 55,
        Num8 = 56,
        Num9 = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        O = 79,
        Numpad0 = 96,
        Numpad1 = 97,
        Numpad2 = 98,
        Numpad3 = 99,
        Numpad4 = 100,
        Numpad5 = 101,
        Numpad6 = 102,
        Numpad7 = 103,
        Numpad8 = 104,
        Numpad9 = 105,
        NumpadPlus = 107,
        NumpadMinus = 109,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        NumLock = 144,
        Comma = 188,
        Minus = 189,
        Dot = 190,
        Degrees = 220,
        QuestionMark = 219,
        Accents = 221,
        Plus = 187,
        Hashtag = 191,
        Ü = 186,
        Ä = 222,
        Ö = 192,
        GreaterSmaller = 226,
        Escape = 27
    }
}
