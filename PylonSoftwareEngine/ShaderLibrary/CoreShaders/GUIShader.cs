using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Render11;
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
    public class GUIShader : Shader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ShaderProperties
        {
            public RGBColor Color;
        }

        public ShaderProperties Properties = new ShaderProperties();

        public GUIShader()
        {
            Textures.Clear();
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
                                           File.ReadAllText(@"Shaders\VertexShader2D.hlsl"),
                                           File.ReadAllText(@"Shaders\TextureShader.hlsl")));
            ShaderSteps[0].Is2D = true;
        }

    }
}
