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
        public static float Primary_X => Screen.PrimaryScreen.WorkingArea.X;
        public static float Primary_Y => Screen.PrimaryScreen.WorkingArea.Y;
        public static float Primary_Width => Screen.PrimaryScreen.WorkingArea.Width;
        public static float Primary_Height => Screen.PrimaryScreen.WorkingArea.Height;

        public static Vector2 Primary_Position => new Vector2(Primary_X, Primary_Y);
        public static Vector2 Primary_Size => new Vector2(Primary_Width, Primary_Height);
    }
}
