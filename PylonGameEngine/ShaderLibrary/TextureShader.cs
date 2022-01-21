using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Vortice.Direct3D11;

namespace PylonGameEngine.ShaderLibrary
{
    public class TextureShader : Shader
    {
        #region Initialization_Update
        public ShaderInput Input;
        public TextureShader()
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonGameEngine.Resources.Shaders.TextureShader;
            base.ShaderEntryPoint = "TextureShader";
            base.Textures.Clear();
        }

        public TextureShader(Texture texture)
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonGameEngine.Resources.Shaders.TextureShader;
            base.ShaderEntryPoint = "TextureShader";
            base.Textures.Clear();
            base.Textures.Add(texture);
        }

        //public TextureShader(ID3D11ShaderResourceView textureressource)
        //{
        //    Input = new ShaderInput();
        //    base.ShaderCode = PylonGameEngine.Resources.Shaders.TextureShader;
        //    base.ShaderEntryPoint = "TextureShader";
        //    base.Textures.Clear();
        //    Texture tex = new Texture
        //    {
        //        Resource = textureressource,
        //        Initialized = true
        //    };
        //    base.Textures.Add(tex);
        //}

        public TextureShader(string texture)
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonGameEngine.Resources.Shaders.TextureShader;
            base.ShaderEntryPoint = "TextureShader";
            base.Textures.Clear();
            base.Textures.Add(new Texture(texture));
        }

        public TextureShader(List<Texture> textures)
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonGameEngine.Resources.Shaders.TextureShader;
            base.ShaderEntryPoint = "TextureShader";
            base.Textures.Clear();
            base.Textures = textures;
        }

        public TextureShader(List<string> textures)
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonGameEngine.Resources.Shaders.TextureShader;
            base.ShaderEntryPoint = "TextureShader";
            base.Textures.Clear();
            foreach (string tex in textures)
            {
                base.Textures.Add(new Texture(tex));
            }

        }

        public override void UpdateChild()
        {
            (ShaderInputStructure, bool) structure = Input.GetStructure();
            if (structure.Item2)
            {
                base.InputStructure = structure.Item1;
            }
        }

        //public override SamplerDescription CreateSamplerDescription()
        //{
        //    SamplerDescription desc = base.CreateSamplerDescription();
        //    desc.Filter = Filter.MinMagLinearMipPoint;
        //    desc.ComparisonFunction = ComparisonFunction.Always;
        //    desc.BorderColor = new Color4(0, 0, 0, 0);
        //    return desc;
        //}
        #endregion Initialization_Update

        #region Input
        [StructLayout(LayoutKind.Sequential)]
        public struct ShaderInputStructure
        {
            public RGBColor Color;
        }

        public class ShaderInput
        {
            #region Head
            private ShaderInputStructure structure = new ShaderInputStructure();
            private bool ValueChanged = false;
            public ValueTuple<ShaderInputStructure, bool> GetStructure()
            {
                if (ValueChanged)
                {
                    ValueChanged = false;
                    return (structure, true);
                }
                else
                {
                    return (structure, false);
                }
            }
            #endregion Head

            public ShaderInput()
            {

            }
        }
        #endregion Input
    }
}
