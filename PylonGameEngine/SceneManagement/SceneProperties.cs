using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;

namespace PylonGameEngine.SceneManagement
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
            SkyBoxMaterial = new Material("", new ShaderLibrary.ColorShader(color));
        }

        public SkyBox(Texture texture)
        {
            SkyBoxMaterial = new Material("", new ShaderLibrary.TextureShader(texture));
        }

        public void ChangeMaterial(RGBColor color)
        {
            ((ShaderLibrary.ColorShader)SkyBoxMaterial.Shader).Input.Color = color;
        }

        public void ChangeMaterial(Texture texture)
        {
            ((ShaderLibrary.TextureShader)SkyBoxMaterial.Shader).Textures[0] = texture;
        }
    }
}
