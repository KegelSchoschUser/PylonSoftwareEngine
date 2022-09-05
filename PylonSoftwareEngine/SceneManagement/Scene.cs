using PylonSoftwareEngine.General;
using PylonSoftwareEngine.Input;
using PylonSoftwareEngine.Physics;
using PylonSoftwareEngine.SceneManagement.Objects;
using PylonSoftwareEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PylonSoftwareEngine.SceneManagement
{
    public class Scene : UniqueNameInterface
    {
        public bool Paused = false;

        public LockedList<SoftwareObject3D> Objects { get; private set; }
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

        public MyPhysics Physics;

        public Scene()
        {
            Objects = new LockedList<SoftwareObject3D>();
            Components = new LockedList<IComponent>();
            Cameras = new LockedList<Camera>();
            Properties = new SceneProperties(this, SkyBox.DefaultSkybox);
            Renderer = new SceneRenderer(this);
            InputManager = new InputManager(this, null);
            Gui = new GUI(this);

            Physics = new MyPhysics();
            Physics.SceneContext = this;
            Physics.Initialize();
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
            if(!Paused)
                Renderer.Render();
        }

        public void Add(SoftwareObject3D obj)
        {
            if (obj.SceneContext == null)
                obj.SceneContext = this;

            if(Objects.Contains(obj) == false)
                Objects.Add(obj);

            obj.OnAddScene();

            if (obj is Camera)
            {
                Cameras.Add((Camera)obj);
                if (Cameras.Count == 1)
                    MainCamera = (Camera)obj;
            }
        }

        public void Remove(SoftwareObject3D obj)
        {
            Objects.Remove(obj);
            obj.Destroy();
        }

        internal void UpdateFrame()
        {
            if (Paused)
                return;
            Gui.UpdateFrame();

            foreach (var obj in Gui.GetRenderOrder())
            {
                obj.UpdateFrameInternal();
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

            if (Paused)
                return;
            InputManager.UpdateTick();
            Gui.UpdateTick();
            foreach (var obj in Gui.GetRenderOrder())
            {
                obj.UpdateTickInternal();
                obj.UpdateTick();
            }

            foreach (var component in Components.ToList())
            {
                component.UpdateTick();
            }

            Physics.Update(MySoftware.SoftwareTickLoop.Tickrate);
        }

        internal List<SoftwareObject3D> GetRenderOrder3D()
        {
            var Output = new List<SoftwareObject3D>();
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
