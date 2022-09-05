/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.UI.GUIObjects;
using PylonSoftwareEngine.Utilities;
using System;
using System.Linq;

namespace PylonSoftwareEngine.SceneManagement
{
    public class GUI
    {
        public LockedList<GUIObject> GUIObjects { get; private set; }

        internal GUIObject FocusedObject { get; private set; }
        internal GUIObject FocusedLostObject { get; private set; }
        private GUIObject LastFocusedObject;

        internal GUIObject MouseHoverObject { get; private set; }
        private GUIObject LastMouseHoverObject;

        internal GUIObject MouseEnterObject { get; private set; }
        internal GUIObject MouseLeaveObject { get; private set; }

        private GUIObject PlaceHolder;
        internal Scene SceneContext { get; private set; }

        public GUI(Scene scenecontext)
        {
            SceneContext = scenecontext;
            GUIObjects = new LockedList<GUIObject>(ref MySoftware.RenderLock);
            PlaceHolder = new GUIObject() { Transform = new Mathematics.Transform2D() { Size = new Mathematics.Vector2(float.MaxValue) } };
            PlaceHolder.SceneContext = scenecontext;
        }

        public void SetFocus(GUIObject obj)
        {
            FocusedObject = obj;
        }

        internal void UpdateTick()
        {
            LastMouseHoverObject = MouseHoverObject;
            LastFocusedObject = FocusedObject;


            LockedList<GUIObject> Objects = new LockedList<GUIObject>(ref MySoftware.RenderLock);
            Objects.AddRange(GUIObjects);
            Objects.Reverse();
            Objects.Add(PlaceHolder);

            foreach (var obj in Objects)
            {

                if (obj.MouseInBounds())
                {
                    var objs = obj.CheckChildrenMouseBound();
                    if (objs.Item1 != null || objs.Item2 != null)
                    {
                        MouseHoverObject = objs.Item1;
                        if (objs.Item2 != null)
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
        }

        public void Remove(GUIObject gUIObject)
        {
            GUIObjects.Remove(gUIObject);

            if (gUIObject.Parent == null)
            {
                foreach (var child in gUIObject.Children)
                {
                    child.Parent = null;
                }
            }
            else
            {
                foreach (var child in gUIObject.Children)
                {
                    child.Parent = gUIObject.Parent;
                }
            }

            gUIObject.Destroy();
        }

        internal void UpdateFrame()
        {

        }

        public LockedList<GUIObject> GetRenderOrder()
        {
            lock (MySoftware.RenderLock)
            {
                var Output = new LockedList<GUIObject>(ref MySoftware.RenderLock);
                foreach (var obj in GUIObjects)
                {
                    if (obj.Visible == false)
                        continue;

                    if (!Output.Contains(obj))
                    {
                        Output.Add(obj);
                        Output.AddRange(obj.GetChildrenRecursive());
                    }
                }
                return Output;
            }
        }

        public void Destroy(GUIObject gUIObject)
        {
            lock (MySoftware.RenderLock)
                gUIObject.Destroy();
        }

        public void Add(GUIObject gUIObject, GUIObject Parent = null)
        {
            gUIObject.SceneContext = SceneContext;
            if (Parent == null)
            {
                GUIObjects.Add(gUIObject);
                gUIObject.OnAddScene();
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
                obj.SceneContext = SceneContext;
                Add(obj, Parent);
            }
        }

        internal void RefreshALL()
        {
            Array.ForEach(this.GetRenderOrder().ToArray(), (x) => x.QueueDraw());
        }
    }
}
