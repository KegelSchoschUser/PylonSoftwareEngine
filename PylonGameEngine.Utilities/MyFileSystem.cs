using System;

namespace PylonGameEngine.Utils
{
    public static class MyFileSystem
    {
        public static string ROAMING => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
