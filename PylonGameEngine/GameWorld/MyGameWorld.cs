using PylonGameEngine.Mathematics;
using PylonGameEngine.Physics;
using PylonGameEngine.Render11;
using PylonGameEngine.Utils;
using System.Collections.Generic;

namespace PylonGameEngine.GameWorld
{
    public static class MyGameWorld
    {
        public static LockedList<GameObject3D> Objects { get; private set; }
        public static UI.GUI GUI { get; private set; }

        public static CameraObject ActiveCamera;
        public static DirectController3D DirectController3D { get; private set; }
        public static RGBColor SkyboxColor = new RGBColor(0, 0, 0);
        public static WindowRenderTarget WindowRenderTarget;

        static MyGameWorld()
        {
            WindowRenderTarget = new WindowRenderTarget(MyGame.MainWindow);
            DirectController3D = new DirectController3D();
            Objects = new LockedList<GameObject3D>(ref MyGame.RenderLock);
            GUI = new UI.GUI();
            MyGame.GameTickLoop.Tick += (delegate ()
            {

                foreach (GameScript item in WorldManager.Scripts)
                {
                    item.UpdateTick();
                }

                GUI.UpdateTick();
               // Input.Keyboard.Cycle();

                lock (MyGame.RenderLock)
                    MyPhysics.Update(60f);
            });

            //MyGame.RenderLoop.Tick += (delegate ()
            //{

            //});
        }

        internal static List<GameObject3D> GetRenderOrder()
        {
            var Output = new List<GameObject3D>();
            foreach (var obj in Objects)
            {
                Output.Add(obj);
                Output.AddRange(obj.GetChildrenRecursive());
            }
            return Output;
        }


    }
}
