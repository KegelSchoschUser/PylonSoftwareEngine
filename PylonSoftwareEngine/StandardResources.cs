/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.ShaderLibrary;
using PylonSoftwareEngine.ShaderLibrary.CoreShaders;

namespace PylonSoftwareEngine
{
    internal static class StandardResources
    {
        public static void AddResources()
        {
            Material White = new Material("DEBUG_White");
            Material Black = new Material("DEBUG_Black");
            Material Red = new Material("DEBUG_Red");
            Material Green = new Material("DEBUG_Green");
            Material Blue = new Material("DEBUG_Blue");


            var RedColorShader = new ColorShader();
            RedColorShader.Properties.Color = new RGBColor(1, 0, 0);
            Red.Shader = RedColorShader;
            MySoftware.Materials.Add(Red);

            var GreenColorShader = new ColorShader();
            GreenColorShader.Properties.Color = new RGBColor(0, 1, 0);
            Green.Shader = GreenColorShader;
            MySoftware.Materials.Add(Green);

            var BlueColorShader = new ColorShader();
            BlueColorShader.Properties.Color = new RGBColor(0, 0, 1);
            Blue.Shader = BlueColorShader;
            MySoftware.Materials.Add(Blue);

            var WhiteColorShader = new ColorShader();
            WhiteColorShader.Properties.Color = new RGBColor(1, 1, 1);
            White.Shader = WhiteColorShader;
            MySoftware.Materials.Add(White);

            var BlackColorShader = new ColorShader();
            BlackColorShader.Properties.Color = new RGBColor(0, 0, 0);
            Black.Shader = BlackColorShader;
            MySoftware.Materials.Add(Black);
        }
    }
}