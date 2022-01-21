using PylonGameEngine.Mathematics;
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace PylonGameEngine.ShaderLibrary
{
    public class ColorShader : Shader
    {
        #region Initialization_Update
        public ShaderInput Input;
        public ColorShader()
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonGameEngine.Resources.Shaders.ColorShader;
            base.ShaderEntryPoint = "ColorShader";
        }

        public ColorShader(RGBColor Color)
        {
            Input = new ShaderInput();
            base.ShaderCode = PylonGameEngine.Resources.Shaders.ColorShader;
            base.ShaderEntryPoint = "ColorShader";
            Input.Color = Color;
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

            public RGBColor Color
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
