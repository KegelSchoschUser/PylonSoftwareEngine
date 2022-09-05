/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Utilities;

namespace PylonSoftwareEngine.Billboarding
{
    public static class BillBoard
    {
        public static LockedList<BillboardObject> BillboardObjects = new LockedList<BillboardObject>(ref MySoftware.RenderLock);

        public static void Clear()
        {
            BillboardObjects.Clear();
        }

        public static void DrawLine(Vector3 p1, Vector3 p2, Material material, bool OnTop = false)
        {
            BillboardObjects.Add(new Line(p1, p2, 0.4f, ref material, OnTop));
        }

        public static void DrawLine(Vector3 p1, Vector3 p2, float thickness, Material material, bool OnTop = false)
        {
            BillboardObjects.Add(new Line(p1, p2, thickness, ref material, OnTop));
        }

        public static void DrawLineCube(Vector3 Position, Vector3 Size, Material material, bool OnTop = false)
        {
            DrawLineCube(Position, Size, 0.4f, material, OnTop);
        }
        public static void DrawLineCube(Vector3 Position, Vector3 Size, float thickness, Material material, bool OnTop = false)
        {
            Vector3 Position2 = Position + Size;

            //Bottom:
            BillboardObjects.Add(new Line(Position, new Vector3(Position2.X, Position.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position.Y, Position.Z), new Vector3(Position2.X, Position.Y, Position2.Z), thickness, ref material, OnTop));

            BillboardObjects.Add(new Line(new Vector3(Position.X, Position.Y, Position2.Z), new Vector3(Position.X, Position.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position.Y, Position2.Z), new Vector3(Position2.X, Position.Y, Position2.Z), thickness, ref material, OnTop));


            //Y:
            BillboardObjects.Add(new Line(Position, new Vector3(Position.X, Position2.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position.Y, Position.Z), new Vector3(Position2.X, Position2.Y, Position.Z), thickness, ref material, OnTop));

            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position.Y, Position2.Z), new Vector3(Position2.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position.Y, Position2.Z), new Vector3(Position.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));

            //Top:
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position2.Y, Position.Z), new Vector3(Position2.X, Position2.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position2.Y, Position.Z), new Vector3(Position2.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));

            BillboardObjects.Add(new Line(new Vector3(Position.X, Position2.Y, Position2.Z), new Vector3(Position.X, Position2.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position2.Y, Position2.Z), new Vector3(Position2.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));
        }

        public static void DrawBoundingBox(BoundingBox BoundingBox, float thickness, Material material, bool OnTop = false)
        {
            Vector3 Position = BoundingBox.Min;
            Vector3 Position2 = BoundingBox.Max;

            //Bottom:
            BillboardObjects.Add(new Line(Position, new Vector3(Position2.X, Position.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position.Y, Position.Z), new Vector3(Position2.X, Position.Y, Position2.Z), thickness, ref material, OnTop));

            BillboardObjects.Add(new Line(new Vector3(Position.X, Position.Y, Position2.Z), new Vector3(Position.X, Position.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position.Y, Position2.Z), new Vector3(Position2.X, Position.Y, Position2.Z), thickness, ref material, OnTop));


            //Y:
            BillboardObjects.Add(new Line(Position, new Vector3(Position.X, Position2.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position.Y, Position.Z), new Vector3(Position2.X, Position2.Y, Position.Z), thickness, ref material, OnTop));

            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position.Y, Position2.Z), new Vector3(Position2.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position.Y, Position2.Z), new Vector3(Position.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));

            //Top:
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position2.Y, Position.Z), new Vector3(Position2.X, Position2.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position2.X, Position2.Y, Position.Z), new Vector3(Position2.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));

            BillboardObjects.Add(new Line(new Vector3(Position.X, Position2.Y, Position2.Z), new Vector3(Position.X, Position2.Y, Position.Z), thickness, ref material, OnTop));
            BillboardObjects.Add(new Line(new Vector3(Position.X, Position2.Y, Position2.Z), new Vector3(Position2.X, Position2.Y, Position2.Z), thickness, ref material, OnTop));
        }
    }
}
