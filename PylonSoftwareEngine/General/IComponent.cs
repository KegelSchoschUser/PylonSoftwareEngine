/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.SceneManagement;
using PylonSoftwareEngine.Utilities;

namespace PylonSoftwareEngine.General
{
    public class IComponent : UniqueNameInterface
    {
        public Scene SceneContext { get; internal set; }
        public float DeltaTime => MySoftware.RenderLoop.DeltaTime;
        public float FixedDeltaTime => MySoftware.SoftwareTickLoop.DeltaTime;

        public virtual void Initialize()
        {

        }

        public virtual void OnDestroy()
        {

        }


        public virtual void UpdateTick()
        {

        }

        public virtual void UpdateFrame()
        {

        }
    }
}
