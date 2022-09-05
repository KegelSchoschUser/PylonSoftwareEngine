using PylonSoftwareEngine.Mathematics;

namespace PylonSoftwareEngine.Billboarding
{
    public class Line : BillboardObject
    {
        Vector3 Point1;
        Vector3 Point2;
        float Thickness;

        public Line(Vector3 p1, Vector3 p2, float thickness, ref Material material, bool onTop = false)
        {
            Point1 = p1;
            Point2 = p2;
            Thickness = thickness;
            Material = material;
            OnTop = onTop;
        }

        public override Mesh GetMesh(Vector3 CameraPosition)
        {
            Vector3 dir = (Vector3)(Point2 - Point1);
            float len = dir.Length();
            dir = dir.Normalize();
            Vector3 cameraToPoint = (CameraPosition - Point1).Normalize();
            Vector3 sideVector = Vector3.GetVector3Scaled(Vector3.Cross(dir, cameraToPoint), Thickness);
            Mesh m = new Mesh();
            m.AddTriangle(new Triangle(Point2 + sideVector, Point2 - sideVector, Point1 - sideVector, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1)), Material);
            m.AddTriangle(new Triangle(Point1 + sideVector, Point2 + sideVector, Point1 - sideVector, new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 1)), Material);

            return m;
        }
    }
}
