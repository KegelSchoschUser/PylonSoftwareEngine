/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.General;

namespace PylonSoftwareEngine.Physics
{
    public class PhysicsComponent : Component3D
    {
        public float Friction = 1f;
        public bool UseCollisions = true;
    }
}
