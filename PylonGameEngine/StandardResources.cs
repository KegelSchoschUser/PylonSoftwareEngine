using PylonGameEngine.Mathematics;
using PylonGameEngine.ShaderLibrary;

namespace PylonGameEngine
{
    internal static class StandardResources
    {
        public static void AddResources()
        {
            Material Red = new Material("DEBUG_Red");
            Material Green = new Material("DEBUG_Green");
            Material Blue = new Material("DEBUG_Blue");
            var RedColorShader = new ColorShader();
            RedColorShader.Input.Color = new RGBColor(1, 0, 0);
            Red.Shader = RedColorShader;
            MyGame.Materials.Add(Red);

            var GreenColorShader = new ColorShader();
            GreenColorShader.Input.Color = new RGBColor(0, 1, 0);
            Green.Shader = GreenColorShader;
            MyGame.Materials.Add(Green);

            var BlueColorShader = new ColorShader();
            BlueColorShader.Input.Color = new RGBColor(0, 0, 1);
            Blue.Shader = BlueColorShader;
            MyGame.Materials.Add(Blue);
        }
    }
}