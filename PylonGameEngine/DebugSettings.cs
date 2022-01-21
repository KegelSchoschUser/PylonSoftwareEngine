using PylonGameEngine.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PylonGameEngine
{
    public static class DebugSettings
    {
        public static class UISettings
        {
            private static bool _DrawLayoutRectangle = false;












            public static bool DrawLayoutRectangle
            {
                get { return _DrawLayoutRectangle; }
                set { _DrawLayoutRectangle = value; MyGameWorld.GUI.RefreshALL(); }
            }
        }
    }
}
