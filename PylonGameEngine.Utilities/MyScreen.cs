using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PylonGameEngine.Utilities
{
    public static class MyScreen
    {
        //BUG: doesnt return the right Size
        public static float Primary_X => Screen.PrimaryScreen.Bounds.X;
        public static float Primary_Y => Screen.PrimaryScreen.Bounds.Y;
        public static float Primary_Width => Screen.PrimaryScreen.Bounds.Width;
        public static float Primary_Height => Screen.PrimaryScreen.Bounds.Height;

        public static Vector2 Primary_Position => new Vector2(Primary_X, Primary_Y);
        public static Vector2 Primary_Size => new Vector2(Primary_Width, Primary_Height);
    }
}
