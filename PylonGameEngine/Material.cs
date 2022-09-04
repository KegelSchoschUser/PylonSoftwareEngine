using PylonGameEngine.ShaderLibrary.Core;
using PylonGameEngine.ShaderLibrary.CoreShaders;
using PylonGameEngine.Utilities;

namespace PylonGameEngine
{
    public class Material : UniqueNameInterface
    {
        public Shader Shader;

        public Material(string displayname)
        {
            Name = displayname;
            Shader = new ColorShader();
            Shader.Initialize();
        }


        public Material(string displayname, Shader shader)
        {
            Name = displayname;
            Shader = shader;
            Shader.Initialize();
        }

        public int Index
        {
            get
            {
                return MyGame.Materials.IndexOf(this);
            }
        }

        public override string ToString()
        {
            return $"MATERIAL: Name: {Name}\n";
        }

        //public static Material GetEmpty()
        //{
        //    return new Material("");
        //}

    }
}
