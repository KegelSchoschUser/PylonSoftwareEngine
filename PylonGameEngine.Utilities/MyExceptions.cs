using System;
using System.Diagnostics;

namespace PylonGameEngine.Utils
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
