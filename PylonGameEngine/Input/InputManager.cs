using PylonGameEngine.SceneManagement;

namespace PylonGameEngine.Input
{
    public class InputManager
    {
        internal Scene SceneContext { get; private set; }
        public Window Window { get; private set; }

        public Mouse Mouse { get; private set; }
        public Keyboard Keyboard { get; private set; }

        internal InputManager(Scene sceneContext, Window window)
        {
            SceneContext = sceneContext;
            Window = window;

            Mouse = new Mouse(this);
            Keyboard = new Keyboard(this);
        }

        internal void SetWindow(Window window)
        {
            if (Window != null)
                if (Window.InputManagers.Contains(this))
                    Window.InputManagers.Remove(this);

            window.InputManagers.Add(this);
            this.Window = window;
        }
        internal void UpdateTick()
        {
            Mouse.Update();
            Keyboard.Update();
        }

        internal void MouseMove(int x, int y)
        {
            Mouse.MouseMoveEvent(x, y);
        }

        internal void MouseButtonEvent(Mouse.MouseButton Button, bool down)
        {
            Mouse.MouseButtonEvent(Button, down);
        }
        public void MouseScrollEvent(int y)
        {
            Mouse.MouseScrollEvent(y);
        }

        internal void KeyDown(KeyboardKey key)
        {
            Keyboard.AddKey(key);
        }

        internal void KeyUp(KeyboardKey key)
        {
            Keyboard.RemoveKey(key);
        }
        internal void CharKey(char c)
        {
            Keyboard.AddCharKey(c);
        }
    }
}
