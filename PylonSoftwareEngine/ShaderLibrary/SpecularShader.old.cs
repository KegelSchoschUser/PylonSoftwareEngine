using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Render11;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PylonSoftwareEngine.ShaderLibrary
{
    public class SpecularShader : Shader
    {
        #region Initialization_Update
        public ShaderInput Input;
        public SpecularShader()
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonSoftwareEngine.Resources.Shaders.SpecularShader;
            base.ShaderEntryPoint = "SpecularShader";
        }

        public SpecularShader(string texture)
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonSoftwareEngine.Resources.Shaders.SpecularShader;
            base.ShaderEntryPoint = "SpecularShader";
            base.Textures.Clear();
            base.Textures.Add(new Texture(texture));
        }

        public override void UpdateChild()
        {
            (ShaderInputStructure, bool) structure = Input.GetStructure();
            if (structure.Item2)
            {
                base.InputStructure = structure.Item1;
            }
        }
        #endregion Initialization_Update

        #region Input
        [StructLayout(LayoutKind.Sequential)]
        public struct ShaderInputStructure
        {
            public RGBColor ambientColor;
            public RGBColor diffuseColor;
            public Vector3 lightDirection;
            public float specularPower;
            public RGBColor specularColor;
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

            public RGBColor ambientColor
            {
                #region get_set
                get
                {
                    TypedReference reference = __makeref(structure);
                    dynamic val = structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).GetValueDirect(reference);
                    return val;
                }
                set
                {
                    TypedReference reference = __makeref(structure);
                    structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).SetValueDirect(reference, value);
                    ValueChanged = true;

                }
                #endregion get_set
            }

            public RGBColor diffuseColor
            {
                #region get_set
                get
                {
                    TypedReference reference = __makeref(structure);
                    dynamic val = structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).GetValueDirect(reference);
                    return val;
                }
                set
                {
                    TypedReference reference = __makeref(structure);
                    structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).SetValueDirect(reference, value);
                    ValueChanged = true;

                }
                #endregion get_set
            }

            public Vector3 lightDirection
            {
                #region get_set
                get
                {
                    TypedReference reference = __makeref(structure);
                    dynamic val = structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).GetValueDirect(reference);
                    return val;
                }
                set
                {
                    TypedReference reference = __makeref(structure);
                    structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).SetValueDirect(reference, value);
                    ValueChanged = true;

                }
                #endregion get_set
            }

            public float specularPower
            {
                #region get_set
                get
                {
                    TypedReference reference = __makeref(structure);
                    dynamic val = structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).GetValueDirect(reference);
                    return val;
                }
                set
                {
                    TypedReference reference = __makeref(structure);
                    structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).SetValueDirect(reference, value);
                    ValueChanged = true;

                }
                #endregion get_set
            }

            public RGBColor specularColor
            {
                #region get_set
                get
                {
                    TypedReference reference = __makeref(structure);
                    dynamic val = structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).GetValueDirect(reference);
                    return val;
                }
                set
                {
                    TypedReference reference = __makeref(structure);
                    structure.GetType().GetField(MethodBase.GetCurrentMethod().Name.Substring(4)).SetValueDirect(reference, value);
                    ValueChanged = true;

                }
                #endregion get_set
            }
        }
        #endregion Input
    }
}
