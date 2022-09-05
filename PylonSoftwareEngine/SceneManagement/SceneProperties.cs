/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Render11;
using PylonSoftwareEngine.ShaderLibrary.CoreShaders;
using System;

namespace PylonSoftwareEngine.SceneManagement
{
    public class SceneProperties
    {
        private Scene Scene;

        public SkyBox SkyBox;

        internal SceneProperties(Scene scene, SkyBox skybox)
        {
            Scene = scene;
            SkyBox = skybox;
        }

    }

    public class SkyBox
    {
        internal Material SkyBoxMaterial;

        public static SkyBox DefaultSkybox => new SkyBox(RGBColor.DarkGray);

        public SkyBox(RGBColor color)
        {
            SkyBoxMaterial = new Material("", new ColorShader(color));
        }

        public SkyBox(Texture texture)
        {
            throw new Exception();
            //SkyBoxMaterial = new Material("", new ShaderLibrary.TextureShader(texture));
        }

        public void ChangeMaterial(RGBColor color)
        {
            ((ColorShader)SkyBoxMaterial.Shader).Properties.Color = color;
        }

        public void ChangeMaterial(Texture texture)
        {
            throw new Exception();
            //((ShaderLibrary.TextureShader)SkyBoxMaterial.Shader).Textures[0] = texture;
        }
    }
}
