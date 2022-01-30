using System.Collections.Generic;

namespace PylonGameEngine.Input
{
    public static class Keyboard
    {
        public static HashSet<KeyboardKey> DownKeys = new HashSet<KeyboardKey>();
        private static HashSet<KeyboardKey> DownKeysBuffer = new HashSet<KeyboardKey>();
        public static HashSet<KeyboardKey> PressedKeys = new HashSet<KeyboardKey>();
        public static HashSet<KeyboardKey> UpKeys = new HashSet<KeyboardKey>();
        private static HashSet<KeyboardKey> UpKeysBuffer = new HashSet<KeyboardKey>();
        public static HashSet<char> CharacterKeys = new HashSet<char>();
        private static HashSet<char> CharacterKeysBuffer = new HashSet<char>();


        #region Core
        internal static void AddKey(KeyboardKey key)
        {
            if (!PressedKeys.Contains(key))
            {
                DownKeysBuffer.Add(key);
            }

            PressedKeys.Add(key);
        }

        internal static void AddCharKey(char c)
        {
            CharacterKeysBuffer.Add(c);
        }

        internal static void RemoveKey(KeyboardKey key)
        {
            UpKeysBuffer.Add(key);
            PressedKeys.Remove(key);
        }

        internal static void Cycle()
        {
            DownKeys.Clear();
            DownKeys.UnionWith(DownKeysBuffer);
            DownKeysBuffer.Clear();

            UpKeys.Clear();
            UpKeys.UnionWith(UpKeysBuffer);
            UpKeysBuffer.Clear();

            CharacterKeys.Clear();
            CharacterKeys.UnionWith(CharacterKeysBuffer);
            CharacterKeysBuffer.Clear();
        }
        #endregion Core

        public static bool KeyDown(KeyboardKey key)
        {
            return DownKeys.Contains(key);
        }

        public static bool KeyPressed(KeyboardKey key)
        {
            return PressedKeys.Contains(key);
        }

        public static bool KeyUp(KeyboardKey key)
        {
            return UpKeys.Contains(key);
        }
    }
}
