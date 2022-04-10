using System.Collections.Generic;

namespace PylonGameEngine.Input
{
    public class Keyboard
    {
        internal InputManager InputManager { get; private set; }

        public HashSet<KeyboardKey> DownKeys = new HashSet<KeyboardKey>();
        private HashSet<KeyboardKey> DownKeysBuffer = new HashSet<KeyboardKey>();
        public HashSet<KeyboardKey> PressedKeys = new HashSet<KeyboardKey>();
        public HashSet<KeyboardKey> UpKeys = new HashSet<KeyboardKey>();
        private HashSet<KeyboardKey> UpKeysBuffer = new HashSet<KeyboardKey>();
        public HashSet<char> CharacterKeys = new HashSet<char>();
        private HashSet<char> CharacterKeysBuffer = new HashSet<char>();

        public delegate void KeyDownEvent(KeyboardKey key);
        public event KeyDownEvent OnKeyDown;

        public delegate void KeyUpEvent(KeyboardKey key);
        public event KeyUpEvent OnKeyUp;
        public Keyboard(InputManager manager)
        {
            InputManager = manager;
            OnKeyDown += (k) =>{ };
            OnKeyUp += (k) => { };
        }


        #region Core
        internal void AddKey(KeyboardKey key)
        {
            if (!PressedKeys.Contains(key))
            {
                DownKeysBuffer.Add(key);
            }

            PressedKeys.Add(key);
            OnKeyDown(key);
        }

        internal void AddCharKey(char c)
        {
            CharacterKeysBuffer.Add(c);
        }
        internal void RemoveKey(KeyboardKey key)
        {
            UpKeysBuffer.Add(key);
            PressedKeys.Remove(key);
            OnKeyUp(key);
        }

        internal void Update()
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

        public bool KeyDown(KeyboardKey key)
        {
            return DownKeys.Contains(key);
        }

        public bool KeyPressed(KeyboardKey key)
        {
            return PressedKeys.Contains(key);
        }

        public bool KeyUp(KeyboardKey key)
        {
            return UpKeys.Contains(key);
        }
    }
}
