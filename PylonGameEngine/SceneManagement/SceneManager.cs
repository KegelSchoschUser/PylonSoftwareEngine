using PylonGameEngine.Utilities;

namespace PylonGameEngine.SceneManagement
{
    public static class SceneManager
    {
        public static LockedList<Scene> Scenes { get; private set; }
        public static Scene ActiveScene { get; private set; }
        private static object Lock = new object();

        static SceneManager()
        {
            Scenes = new LockedList<Scene>();
        }

        internal static void UpdateFrame()
        {
            lock (Lock)
            {
                foreach (var scene in Scenes)
                {
                    scene.UpdateFrame();
                }
            }

            if (MyGame.RendererEnabled == false)
                return;

            lock (Lock)
            {
                foreach (var scene in Scenes)
                {
                    if (ActiveScene != scene)
                        scene.Render();
                }
                ActiveScene.Render();
            }
        }

        internal static void UpdateTick()
        {
            lock (Lock)
            {
                // C# Bug, foreach loop doesn't work somehow (Error: collection was modified)
                for (int i = 0; i < Scenes.Count; i++)
                {
                    Scenes[i].UpdateTick();
                }
            }
        }

        public static void AddScene(Scene Scene)
        {
            lock (Lock)
            {
                Scene.Initialize();
                Scenes.Add(Scene);
                if (Scenes.Count == 1)
                    ActiveScene = Scene;
            }
        }

        public static void RemoveScene(Scene Scene)
        {
            lock (Lock)
            {
                if (Scenes.Contains(Scene))
                {
                    Scene.Destroy();
                    Scenes.Remove(Scene);
                }
            }
        }
    }
}
