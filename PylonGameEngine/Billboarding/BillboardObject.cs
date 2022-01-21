using PylonGameEngine.General;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Utilities;

namespace PylonGameEngine.Billboarding
{
    public class BillboardObject : UniqueNameInterface, IGameObject
    {
        public bool OnTop = false;
        public Material Material;

        public virtual Mesh GetMesh(Vector3 CameraPosition)
        {
            return new Mesh();
        }
    }
}
