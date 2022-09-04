using PylonGameEngine.Mathematics;
using PylonGameEngine.ShaderLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.ShaderLibrary.CoreShaders
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
