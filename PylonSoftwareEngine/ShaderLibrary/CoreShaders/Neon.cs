using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Render11;
using PylonSoftwareEngine.ShaderLibrary.Core;
using PylonSoftwareEngine.ShaderLibrary.UtilitySteps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PylonSoftwareEngine.ShaderLibrary.CoreShaders
{
    public class Neon : Shader
    {



        public Neon()
        {

        }

        public Neon(RGBColor color)
        {
            Properties.Color = color;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PSProperties
        {
            public RGBColor Color;
        }

        public PSProperties Properties = new PSProperties();

        internal override dynamic GetPixelShaderProperties()
        {
            return Properties;
        }


        [StructLayout(LayoutKind.Sequential)]
        internal struct VSProperties
        {
            internal float screenWidth;
            internal float screenHeight;
            internal float Multiplier;
            internal float padding;
        }

        private VSProperties _VSProperties = new VSProperties();

        internal override dynamic GetVertexShaderProperties()
        {
            return _VSProperties;
        }

        protected override void AddSteps()
        {
            _VSProperties.screenWidth = MySoftware.Windows[0].Size.X;
            _VSProperties.screenHeight = MySoftware.Windows[0].Size.Y;
            _VSProperties.Multiplier = 10f;

            RenderTexture FirstRender = new RenderTexture(MySoftware.Windows[0].Size);
            RenderTexture BlurRender1 = new RenderTexture(MySoftware.Windows[0].Size);
            RenderTexture BlurRender2 = new RenderTexture(MySoftware.Windows[0].Size);

            Textures.Add(new Texture(@"C:\Users\Endric\Desktop\Mathe LK.png"));
            ShaderSteps.Add(new ShaderStep(this,
                                           File.ReadAllText(@"Shaders\VertexShader3D.hlsl"),
                                           File.ReadAllText(@"Shaders\TextureShader.hlsl"),
                                           FirstRender));

            ShaderSteps.Add(new HorizontalBlur(this, FirstRender, BlurRender1));

            ShaderSteps.Add(new VerticalBlur(this, BlurRender1, BlurRender2));

            ShaderSteps.Add(new RenderToCameraStep(this, BlurRender2));
        }

    }
}
