using PylonSoftwareEngine.General;
using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Utilities;

namespace PylonSoftwareEngine.Billboarding
{
    public class BillboardObject : UniqueNameInterface, ISoftwareObject
    {
        public bool OnTop = false;
        public Material Material;

        public virtual Mesh GetMesh(Vector3 CameraPosition)
        {
            return new Mesh();
        }
    }
}
