/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;
using System.Diagnostics;

namespace PylonSoftwareEngine.Utilities
{
    public class MyExceptions
    {
        public class EngineNotInitializedException : Exception
        {
            public EngineNotInitializedException()
            {
                Debug.Assert(false, "EngineNotInitializedException");
            }
        }

        public class LogNotInitializedException : Exception
        {
            public LogNotInitializedException()
            {
                Debug.Assert(false, "LogNotInitializedException");
            }
        }
    }
}
