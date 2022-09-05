/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Utilities;

namespace PylonSoftwareEngine.SceneManagement
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

            if (MySoftware.RendererEnabled == false)
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
