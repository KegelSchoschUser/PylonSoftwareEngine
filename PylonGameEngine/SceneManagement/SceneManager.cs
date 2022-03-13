using PylonGameEngine.Utilities;

namespace PylonGameEngine.SceneManagement
{
    public static class SceneManager
    {
        public static LockedList<Scene> Scenes { get; private set; }
        public static Scene ActiveScene { get; private set; }

        static SceneManager()
        {
            Scenes = new LockedList<Scene>();
        }

        internal static void UpdateFrame()
        {
            foreach (var scene in Scenes)
            {
                scene.UpdateFrame();
            }

            if (MyGame.RendererEnabled == false)
                return;

            foreach (var scene in Scenes)
            {
                if (ActiveScene != scene)
                    scene.Render();
            }

            ActiveScene.Render();
        }

        internal static void UpdateTick()
        {
            foreach (var scene in Scenes)
            {
                scene.UpdateTick();
            }
        }

        public static void AddScene(Scene Scene)
        {
            Scene.Initialize();
            Scenes.Add(Scene);
            if (Scenes.Count == 1)
                ActiveScene = Scene;
        }

        public static void RemoveScene(Scene Scene)
        {
            if (Scenes.Contains(Scene))
            {
                Scene.Destroy();
                Scenes.Remove(Scene);
            }
        }
    }
}
