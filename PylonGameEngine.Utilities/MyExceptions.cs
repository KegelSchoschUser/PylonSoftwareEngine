using System;
using System.Diagnostics;

namespace PylonGameEngine.Utilities
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
