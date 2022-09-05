/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.ShaderLibrary.Core;
using PylonSoftwareEngine.ShaderLibrary.CoreShaders;
using PylonSoftwareEngine.Utilities;

namespace PylonSoftwareEngine
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
                return MySoftware.Materials.IndexOf(this);
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
