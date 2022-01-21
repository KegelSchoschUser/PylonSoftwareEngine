using System;

namespace PylonGameEngine.Utilities
{
    public static class MyFileSystem
    {
        public static string ROAMING => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
