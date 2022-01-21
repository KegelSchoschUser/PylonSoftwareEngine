using PylonGameEngine.General;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;


namespace PylonGameEngine.GameWorld
{
    public class GameObject3D : UniqueNameInterface, IGameObject
    {
        public List<IComponent> Components;
        public Transform Transform = new Transform();
        public GameObject3D Parent { get; private set; }
        public List<GameObject3D> Children { get; private set; }







        public GameObject3D()
        {
            WorldManager.Objects.Add(this);
            Children = new List<GameObject3D>();
            Components = new List<IComponent>();

            OnCreate();
        }

        public void AddComponent(Component3D component)
        {
            component.Parent = this;
            Components.Add(component);
            component.Initialize();
        }

        public void AddComponent(IComponent component)
        {
            Components.Add(component);
            component.Initialize();
        }

        public void AddObject(GameObject3D gameObject)
        {
            if (gameObject == this)
            {
                throw new ArgumentException("Cannot set myself as child!", "GameObject3D");
            }

            gameObject.Parent = this;
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

            WorldManager.Objects.Remove(this);
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
                MyGameWorld.Objects.Remove(this);
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
