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

        #region Core
        public static void AddKey(KeyboardKey key)
        {
            if (!PressedKeys.Contains(key))
            {
                DownKeysBuffer.Add(key);
            }

            PressedKeys.Add(key);
        }

        public static void RemoveKey(KeyboardKey key)
        {
            UpKeysBuffer.Add(key);
            PressedKeys.Remove(key);
        }

        public static void Cycle()
        {

            DownKeys.Clear();
            DownKeys.UnionWith(DownKeysBuffer);
            DownKeysBuffer.Clear();

            UpKeys.Clear();
            UpKeys.UnionWith(UpKeysBuffer);
            UpKeysBuffer.Clear();
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
