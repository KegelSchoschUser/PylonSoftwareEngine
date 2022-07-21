namespace PylonGameEngine.SceneManagement.Objects
{
    public class MeshObject : GameObject3D
    {
        public Mesh Mesh;

        public MeshObject()
        {
            Mesh = new Mesh();
        }

        //public MeshObject(string Filename, bool RightHanded = false)
        //{
        //    LoadMeshFromOBJFile(Filename, RightHanded);
        //    Name = Path.GetFileNameWithoutExtension(Filename);
        //}

        public void SetName(string name)
        {
            Name = name;
        }

        public void OnDestroy()
        {
            Mesh.Destroy();
        }

        //[Obsolete]
        //public void LoadMeshFromOBJFile(string Filename, bool RightHanded = false)
        //{
        //    Mesh = Mesh.LoadFromObjectFile(Filename, RightHanded);

        //    foreach (Material material in Mesh.Materials)
        //    {
        //        if (MyGame.Materials.Get(material.Name) == null)
        //        {
        //            MyGame.Materials.Add(material);
        //        }
        //    }

        //}
    }
}
