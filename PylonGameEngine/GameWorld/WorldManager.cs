using PylonGameEngine.Utilities;

namespace PylonGameEngine.GameWorld
{
    public static class WorldManager
    {
        public static LockedList<GameObject3D> Objects = new LockedList<GameObject3D>(ref MyGame.RenderLock);
        public static LockedList<CameraObject> CameraObjects = new LockedList<CameraObject>(ref MyGame.RenderLock);
        //public static LockedList<GUIObject> GUIObjects = new LockedList<GUIObject>(ref GlobalManager.RenderLock);
        public static LockedList<GameScript> Scripts = new LockedList<GameScript>(ref MyGame.RenderLock);

        public static void Add(GameObject3D gameObject3D)
        {
            Objects.Add(gameObject3D);
        }

        //public static void Add(GUIObject gUIObject)
        //{
        //    GUIObjects.Add(gUIObject);
        //}

        public static void Add(GameScript gameScript)
        {
            Scripts.Add(gameScript);
        }

        public static void Add(CameraObject camera)
        {
            CameraObjects.Add(camera);
        }


        public static void Remove(GameObject3D gameObject3D)
        {
            Objects.Remove(gameObject3D);
        }

        public static void Remove(CameraObject camera)
        {
            CameraObjects.Remove(camera);
        }

        //public static void Remove(GUIObject gUIObject)
        //{
        //    GUIObjects.Remove(gUIObject);
        //}

        public static void Remove(GameScript gameScript)
        {
            Scripts.Remove(gameScript);
        }
    }
}
