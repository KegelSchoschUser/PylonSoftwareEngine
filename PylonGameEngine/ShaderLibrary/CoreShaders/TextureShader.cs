﻿using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
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
    public class TextureShader : Shader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ShaderProperties
        {
            public RGBColor Color;
        }

        public ShaderProperties Properties = new ShaderProperties();

        public TextureShader()
        {
            Textures.Clear();
        }

        public TextureShader(Texture texture)
        {
            Textures.Clear();
            Textures.Add(texture);
        }

        public TextureShader(string texture)
        {
            Textures.Clear();
            Textures.Add(new Texture(texture));
        }

        public TextureShader(List<Texture> textures)
        {
            Textures.Clear();
            Textures = new Utilities.ObservableList<Texture>(textures);
        }

        public TextureShader(List<string> textures)
        {
            Textures.Clear();
            foreach (string tex in textures)
            {
                Textures.Add(new Texture(tex));
            }

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
                                           File.ReadAllText(@"Shaders\TextureShader.hlsl")));
        }

    }
}
