using PylonGameEngine.Render11;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static PylonGameEngine.ShaderLibrary.Core.Shader;

namespace PylonGameEngine.ShaderLibrary.Core
{
    public abstract class Shader
    {
        public List<ShaderStep> ShaderSteps;
        public ObservableList<Texture> Textures = new ObservableList<Texture>();
        internal ID3D11Buffer VertexShaderPropertiesBuffer;
        internal ID3D11Buffer PixelShaderPropertiesBuffer;

        public Shader()
        {
            Initialize();

            Textures.On_ItemAdded += (i) => UpdateTextures();
            Textures.On_ItemRemoved += (i) => UpdateTextures();
            Textures.On_Item_Changed += (i) => UpdateTextures();
        }

        internal void Initialize()
        {
            ShaderSteps = new List<ShaderStep>();

            AddSteps();

            foreach (var step in ShaderSteps)
            {
                step.Initialize();
            }
        }

        internal void Render()
        {
            SetVSBuffer0();
            SetPSBuffer0();

            UpdateTextures();
        }

        private void UpdateTextures()
        {
            for (int i = 0; i < Textures.Count; i++)
            {
                D3D11GraphicsDevice.DeviceContext.PSSetShaderResource(i, Textures[i].GetShaderResourceView());
            }
        }

        private void SetPSBuffer0()
        {
            dynamic ShaderProperties = GetPixelShaderProperties();
            if (ShaderProperties is null)
                return;

            BufferDescription InputBufferDecription = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(ShaderProperties),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(ShaderProperties));
            Marshal.StructureToPtr(ShaderProperties, pnt, false);

            PixelShaderPropertiesBuffer = D3D11GraphicsDevice.Device.CreateBuffer(InputBufferDecription, pnt);
            Marshal.FreeHGlobal(pnt);

            D3D11GraphicsDevice.DeviceContext.PSSetConstantBuffer(0, PixelShaderPropertiesBuffer);
        }

        private void SetVSBuffer0()
        {
            dynamic ShaderProperties = GetVertexShaderProperties();
            if (ShaderProperties is null)
                return;

            BufferDescription BufferDecription = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(ShaderProperties),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(ShaderProperties));
            Marshal.StructureToPtr(ShaderProperties, pnt, false);

            PixelShaderPropertiesBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BufferDecription, pnt);
            Marshal.FreeHGlobal(pnt);

            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(0, PixelShaderPropertiesBuffer);
        }

        protected abstract void AddSteps();

        internal abstract dynamic GetPixelShaderProperties();

        internal abstract dynamic GetVertexShaderProperties();

        internal static InputElementDescription[] CreateInputLayoutDescription/*3D*/()
        {
            InputElementDescription[] InputElements = new InputElementDescription[]
            {
                    new InputElementDescription()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "TEXCOORD",
                        SemanticIndex = 0,
                        Format = Format.R32G32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "NORMAL",
                        SemanticIndex = 0,
                        Format = Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    }

            };

            return InputElements;
        }

        /*internal static InputElementDescription[] CreateInputLayoutDescription2D()
        {
            InputElementDescription[] InputElements = new InputElementDescription[]
            {
                    new InputElementDescription()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "TEXCOORD",
                        SemanticIndex = 0,
                        Format = Format.R32G32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    }

            };

            return InputElements;
        }*/
    }
}
