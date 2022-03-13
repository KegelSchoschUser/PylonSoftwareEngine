using PylonGameEngine.General;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;


namespace PylonGameEngine.SceneManagement
{
    public class GameObject3D : UniqueNameInterface, IGameObject
    {
        public List<Component3D> Components;
        public Transform Transform = new Transform();
        public GameObject3D Parent { get; private set; }
        public List<GameObject3D> Children { get; private set; }

        public List<string> Tags = new List<string>();

        internal Scene SceneContext = null;


        public GameObject3D()
        {
            Children = new List<GameObject3D>();
            Components = new List<Component3D>();

            OnCreate();
        }

        public void AddComponent(Component3D component)
        {
            component.Parent = this;
            component.Scene = SceneContext;
            Components.Add(component);
            SceneContext.Components.Add(component);
            component.Initialize();
        }

        public void AddObject(GameObject3D gameObject)
        {
            if (gameObject == this)
            {
                throw new ArgumentException("Cannot set myself as child!", "GameObject3D");
            }

            gameObject.Parent = this;
            gameObject.SceneContext = SceneContext;
            gameObject.Transform.SetParent(this.Transform);
            Children.Add(gameObject);
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
                foreach (GameObject3D child in Children)
                {
                    child.Parent = Parent;
                    Parent.Children.Add(child);
                }

            }
            else
            {
                foreach (GameObject3D child in Children)
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

        public List<GameObject3D> GetChildrenRecursive()
        {
            var objects = new List<GameObject3D>();
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
