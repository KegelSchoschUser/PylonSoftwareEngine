using PylonGameEngine.General;
using PylonGameEngine.Input;
using PylonGameEngine.SceneManagement.Objects;
using PylonGameEngine.Utilities;
using System.Collections.Generic;

namespace PylonGameEngine.SceneManagement
{
    public class Scene : UniqueNameInterface
    {
        public LockedList<GameObject3D> Objects { get; private set; }
        public LockedList<Camera> Cameras { get; private set; }
        private Camera _MainCamera;
        public Camera MainCamera
        {
            get { return _MainCamera; }
            set { _MainCamera = value; if (Cameras.Count == 0) Cameras.Add(value); }
        }
        internal LockedList<IComponent> Components { get; private set; }

        public SceneRenderer Renderer { get; private set; }

        public SceneProperties Properties;
        public GUI Gui { get; private set; }

        public InputManager InputManager { get; private set; }

        public Scene()
        {
            Objects = new LockedList<GameObject3D>();
            Components = new LockedList<IComponent>();
            Cameras = new LockedList<Camera>();
            Properties = new SceneProperties(this, SkyBox.DefaultSkybox);
            Renderer = new SceneRenderer(this);
            InputManager = new InputManager(this, null);
            Gui = new GUI(this);

        }

        internal void Initialize()
        {
            OnInitialize();
        }
        public virtual void OnInitialize()
        {

        }

        internal void Destroy()
        {
            OnDestroy();
        }
        public virtual void OnDestroy()
        {

        }

        internal void Render()
        {
            Renderer.Render();
        }

        public void Add(GameObject3D obj)
        {
            if (obj.SceneContext == null)
                obj.SceneContext = this;
            Objects.Add(obj);

            if (obj is Camera)
            {
                Cameras.Add((Camera)obj);
                if (Cameras.Count == 1)
                    MainCamera = (Camera)obj;
            }
        }

        public void Remove(GameObject3D obj)
        {
            Objects.Remove(obj);
            obj.Destroy();
        }

        internal void UpdateFrame()
        {
            Gui.UpdateFrame();

            foreach (var obj in Gui.GetRenderOrder())
            {
                obj.UpdateFrame();
            }

            foreach (var component in Components)
            {
                component.UpdateFrame();
            }

            foreach (var obj in Gui.GetRenderOrder())
            {
                obj.OnDrawInternal();
            }
        }

        internal void UpdateTick()
        {
            InputManager.UpdateTick();
            Gui.UpdateTick();

            foreach (var obj in Gui.GetRenderOrder())
            {
                obj.UpdateTickInternal();
                obj.UpdateTick();
            }

            foreach (var component in Components)
            {
                component.UpdateTick();
            }

        }

        internal List<GameObject3D> GetRenderOrder3D()
        {
            var Output = new List<GameObject3D>();
            foreach (var obj in Objects)
            {
                Output.Add(obj);
                Output.AddRange(obj.GetChildrenRecursive());
            }
            return Output;
        }

        public void SetInputWindow(Window window)
        {
            InputManager.SetWindow(window);
        }
    }
}
