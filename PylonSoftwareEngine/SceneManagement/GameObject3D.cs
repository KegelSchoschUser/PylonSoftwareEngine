using PylonSoftwareEngine.General;
using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.SceneManagement.Objects;
using PylonSoftwareEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;


namespace PylonSoftwareEngine.SceneManagement
{
    public class SoftwareObject3D : UniqueNameInterface, ISoftwareObject
    {
        public List<Component3D> Components;
        public Transform Transform = new Transform();
        public SoftwareObject3D Parent { get; private set; }
        public List<SoftwareObject3D> Children { get; private set; }

        public List<string> Tags = new List<string>();
        public bool Visible = true;

        internal Scene SceneContext = null;


        public SoftwareObject3D()
        {
            Children = new List<SoftwareObject3D>();
            Components = new List<Component3D>();

            OnCreate();
        }
            
        public void AddComponent(Component3D component)
        {
            component.Parent = this;
            component.SceneContext = SceneContext;
            Components.Add(component);
            SceneContext.Components.Add(component);
            component.Initialize();
        }

        public void AddObject(SoftwareObject3D SoftwareObject)
        {
            if (SoftwareObject == this)
            {
                throw new ArgumentException("Cannot set myself as child!", "SoftwareObject3D");
            }

            SoftwareObject.Parent = this;
            SoftwareObject.SceneContext = SceneContext;
            SoftwareObject.Transform.SetParent(this.Transform);
            Children.Add(SoftwareObject);

            SoftwareObject.OnAddScene();

            if (SoftwareObject is Camera)
            {
                SceneContext.Cameras.Add((Camera)SoftwareObject);
                if (SceneContext.Cameras.Count == 1)
                    SceneContext.MainCamera = (Camera)SoftwareObject;
            }
        }

        public virtual void OnAddScene()
        {

        }

        public void Destroy()
        {
            OnDestroy();

            foreach (var Component in Components)
            {
                Component.OnDestroy();
            }

            if (Parent != null)
            {
                Parent.Children.Remove(this);
                foreach (SoftwareObject3D child in Children)
                {
                    child.Parent = Parent;
                    Parent.Children.Add(child);
                }

            }
            else
            {
                foreach (SoftwareObject3D child in Children)
                {
                    child.Parent = null;
                }
            }
        }

        public virtual void OnCreate()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public List<SoftwareObject3D> GetChildrenRecursive()
        {
            var objects = new List<SoftwareObject3D>();
            foreach (var item in Children)
            {
                objects.Add(item);
                objects.AddRange(item.GetChildrenRecursive());
            }
            return objects;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Name != null)
                sb.AppendLine("Name: " + Name.ToString());
            else
                sb.AppendLine("Name: " + "NULL");
            sb.AppendLine("Position: " + Transform.Position.ToString());
            sb.AppendLine("Scale: " + Transform.Scale.ToString());
            sb.Append("Rotation: " + Transform.Rotation.ToString());

            return sb.ToString();
        }
    }
}
