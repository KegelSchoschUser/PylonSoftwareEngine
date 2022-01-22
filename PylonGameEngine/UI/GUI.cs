using PylonGameEngine.GameWorld;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;

namespace PylonGameEngine.UI
{
    public class GUI
    {
        public static LockedList<GUIObject> GUIObjects { get; private set; }

        public GUIObject FocusedObject { get; private set; }
        internal GUIObject FocusedLostObject { get; private set; }
        private GUIObject LastFocusedObject;

        internal GUIObject MouseHoverObject { get; private set; }
        private GUIObject LastMouseHoverObject;

        internal GUIObject MouseEnterObject { get; private set; }
        internal GUIObject MouseLeaveObject { get; private set; }

        private GUIObject PlaceHolder;

        public GUI()
        {
            GUIObjects = new LockedList<GUIObject>(ref MyGame.RenderLock);
            PlaceHolder = new GUIObject() { Transform = new Mathematics.Transform2D() { Size = new Mathematics.Vector2(MyGame.MainWindow.Size.X, MyGame.MainWindow.Size.Y) } };
        }


        internal void UpdateTick()
        {
            LastMouseHoverObject = MouseHoverObject;
            LastFocusedObject = FocusedObject;

            LockedList<GUIObject> Objects = new LockedList<GUIObject>(ref MyGame.RenderLock);
            Objects.AddRange(GUIObjects);
            Objects.Reverse();
            Objects.Add(PlaceHolder);

            foreach (var obj in Objects)
            {

                if (obj.MouseInBounds())
                {
                    var objs = obj.CheckChildrenMouseBound();
                    MouseHoverObject = objs.Item1;
                    if (objs.Item2 != null)
                    {
                        FocusedObject = objs.Item2;
                        break;
                    }
                    
                }
                //else
                //{
                //    MouseHoverObject = null;
                //}
            }

            Objects.Remove(PlaceHolder);
            Objects.Clear();

            if (FocusedObject == PlaceHolder)
            {
                FocusedObject = null;
            }

            if (LastMouseHoverObject != MouseHoverObject)
                MouseEnterObject = MouseHoverObject;
            else
            {
                MouseEnterObject = null;
            }

            if (LastMouseHoverObject != MouseHoverObject)
                MouseLeaveObject = LastMouseHoverObject;
            else
                MouseLeaveObject = null;

            if (LastFocusedObject != FocusedObject)
                FocusedLostObject = LastFocusedObject;
            else
                FocusedLostObject = null;


            foreach (GUIObject obj in GetRenderOrder())
            {
                obj.UpdateTickInternal();
                obj.UpdateTick();
            }
        }

        public LockedList<GUIObject> GetRenderOrder()
        {
            lock (MyGame.RenderLock)
            {
                var Output = new LockedList<GUIObject>(ref MyGame.RenderLock);
                foreach (var obj in GUIObjects)
                {
                    if (obj.Visible == false)
                        continue;

                    Output.Add(obj);
                    Output.AddRange(obj.GetChildrenRecursive());
                }
                return Output;
            }
        }

        public void Destroy(GUIObject gUIObject)
        {
            lock (MyGame.RenderLock)
                gUIObject.Destroy();
        }

        public void Add(GUIObject gUIObject, GUIObject Parent = null)
        {
            if (Parent == null)
            {
                GUI.GUIObjects.Add(gUIObject);
            }
            else
            {
                Parent.AddChild(gUIObject);
            }
        }

        public void AddRange(GUIObject[] gUIObject, GUIObject Parent = null)
        {
            foreach (var obj in gUIObject)
            {
                Add(obj, Parent);
            }
        }

        internal void RefreshALL()
        {
            Array.ForEach(this.GetRenderOrder().ToArray(), (x) => x.QueueDraw());
        }
    }
}
