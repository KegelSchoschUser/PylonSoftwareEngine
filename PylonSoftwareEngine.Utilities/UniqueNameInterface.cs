/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

namespace PylonSoftwareEngine.Utilities
{
    public class UniqueNameInterface
    {
        private string _Name = null;
        public string Name
        {
            get
            {
                if (_Name == null)
                {
                    return this.GetType().Name;
                }
                else
                {
                    return _Name;
                }
            }

            protected set
            {
                _Name = value;
            }

        }
    }
}
