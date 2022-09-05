/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System.Runtime.InteropServices;

namespace PylonSoftwareEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawVertex
    {
        public Vector3 Position;
        public Vector2 UV;
        public Vector3 Normal;

        public RawVertex(Vector3 pos, Vector2 uv, Vector3 normal)
        {
            Position = pos;
            UV = uv;
            Normal = normal;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RawVertex2D
    {
        public Vector2 Position;
        public Vector2 UV;

        public RawVertex2D(Vector2 pos, Vector2 uv)
        {
            Position = pos;
            UV = uv;
        }
    }
}
