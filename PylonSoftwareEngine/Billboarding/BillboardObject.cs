/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

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
