using PylonSoftwareEngine.Mathematics;
using System.Windows.Forms;

namespace PylonSoftwareEngine.Utilities
{
    public static class MyScreen
    {
        //BUG: doesnt return the right Size
        public static float Primary_X => Screen.PrimaryScreen.Bounds.X;
        public static float Primary_Y => Screen.PrimaryScreen.Bounds.Y;
        public static float Primary_Width => Screen.PrimaryScreen.Bounds.Width;
        public static float Primary_Height => Screen.PrimaryScreen.Bounds.Height;

        public static float CursorScreen_X => Screen.FromPoint(Cursor.Position).Bounds.X;
        public static float CursorScreen_Y => Screen.FromPoint(Cursor.Position).Bounds.Y;

        public static float CursorScreen_Width => Screen.FromPoint(Cursor.Position).Bounds.Width;
        public static float CursorScreen_Height => Screen.FromPoint(Cursor.Position).Bounds.Height;

        public static Vector2 Primary_Position => new Vector2(Primary_X, Primary_Y);
        public static Vector2 Primary_Size => new Vector2(Primary_Width, Primary_Height);

        public static Vector2 CursorScreen_Position => new Vector2(CursorScreen_X, CursorScreen_Y);
        public static Vector2 CursorScreen_Size => new Vector2(CursorScreen_Width, CursorScreen_Height);
    }
}
