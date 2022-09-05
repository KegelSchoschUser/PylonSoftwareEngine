/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.ShaderLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PylonSoftwareEngine.ShaderLibrary.CoreShaders
{
    public class ColorShader : Shader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ShaderProperties
        {
            public RGBColor Color;
        }

        public ShaderProperties Properties = new ShaderProperties();

        public ColorShader()
        {

        }

        public ColorShader(RGBColor color)
        {
            Properties.Color = color;
        }

        internal override dynamic GetPixelShaderProperties()
        {
            return Properties;
        }

        internal override dynamic GetVertexShaderProperties()
        {
            return null;
        }

        protected override void AddSteps()
        {
            ShaderSteps.Add(new ShaderStep(this,
                                           File.ReadAllText(@"Shaders\VertexShader3D.hlsl"),
                                           File.ReadAllText(@"Shaders\ColorShader.hlsl")));
        }

    }
}
