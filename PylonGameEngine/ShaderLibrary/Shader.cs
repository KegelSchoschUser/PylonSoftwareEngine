using PylonGameEngine.Render11;
using PylonGameEngine.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace PylonGameEngine.ShaderLibrary
{
    public class Shader
    {
        public ID3D11PixelShader PixelShader { get; protected set; }

        public bool SamplerDescriptionChangeRequest = false;
        private SamplerDescription samplerDescription;
        public SamplerDescription SamplerDescription
        {
            get => samplerDescription;
            set
            {
                samplerDescription = value;
                SamplerDescriptionChangeRequest = true;
            }
        }
        public ID3D11SamplerState SamplerState { get; protected set; }
        public List<Texture> Textures = new List<Texture>();

        public bool InputStructureChangeRequest = false;
        private dynamic inputStructure;
        public dynamic InputStructure
        {
            get => inputStructure;
            protected set
            {
                inputStructure = value;
                InputStructureChangeRequest = true;
            }
        }

        private ID3D11Buffer InputBuffer;

        public string ShaderCode = "";
        public string ShaderEntryPoint = "";


        public Shader()
        {
            SamplerDescription = CreateSamplerDescription();
            InputStructureChangeRequest = true;
        }



        public void InitializeShader(ID3D11Device1 device, ID3D11DeviceContext1 deviceContext)
        {
            UpdateChild();
            CompileIfNeeded(device);
            LoadShader(deviceContext);
            CreateSamplerStateIfNeeded(device);
            SetSamplerSate(deviceContext);
            CreateInputBufferIfNeeded(device);
            SetInputBuffer(deviceContext);
            SetShaderTextures(device, deviceContext);
        }

        public void Update(ID3D11Device1 device, ID3D11DeviceContext1 deviceContext)
        {
            UpdateChild();
            if (CreateInputBufferIfNeeded(device))
            {
                SetInputBuffer(deviceContext);
            }
        }

        public void CompileIfNeeded(ID3D11Device1 device)
        {
            if (PixelShader == null)
            {
                Compile(device);
            }
        }
        private void Compile(ID3D11Device1 device)
        {
            //Compiler.CompileFromFile(ShaderCode, ShaderEntryPoint, "ps_4_0", out Blob pixelShaderByteCode, out Blob ErrorBlob);//, ShaderFlags.None, EffectFlags.None);
            Compiler.Compile(ShaderCode, ShaderEntryPoint, this.GetType().Name, "ps_4_0", out Blob pixelShaderByteCode, out Blob ErrorBlob);
            if (ErrorBlob != null)
            {
                MyLog.Default.Write("ShaderCompileError: " + Encoding.Default.GetString(ErrorBlob.GetBytes()), LogSeverity.Critical);
            }

            PixelShader = device.CreatePixelShader(pixelShaderByteCode);
        }

        private void CreateSamplerStateIfNeeded(ID3D11Device1 device)
        {
            if (SamplerState == null || SamplerDescriptionChangeRequest)
            {
                CreateSamplerState(device);
            }
        }
        private void CreateSamplerState(ID3D11Device1 device)
        {

            SamplerState = device.CreateSamplerState(SamplerDescription);
            SamplerDescriptionChangeRequest = false;
        }

        private bool CreateInputBufferIfNeeded(ID3D11Device1 device)
        {
            if (InputBuffer == null || InputStructureChangeRequest)
            {
                CreateInputBuffer(device);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetSamplerSate(ID3D11DeviceContext1 deviceContext)
        {
            deviceContext.PSSetSampler(0, SamplerState);
        }

        public unsafe void CreateInputBuffer(ID3D11Device1 device)
        {
            if (InputStructure is null)
            {
                return;
            }

            BufferDescription InputBufferDecription = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(InputStructure),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(InputStructure));
            Marshal.StructureToPtr(InputStructure, pnt, false);

            InputBuffer = device.CreateBuffer(InputBufferDecription, pnt);
            Marshal.FreeHGlobal(pnt);
            InputStructureChangeRequest = false;
        }

        public void LoadShader(ID3D11DeviceContext1 deviceContext)
        {
            deviceContext.PSSetShader(PixelShader);
        }

        public void SetInputBuffer(ID3D11DeviceContext1 deviceContext)
        {
            deviceContext.PSSetConstantBuffer(0, InputBuffer);
        }

        public void SetShaderTextures(ID3D11Device1 device, ID3D11DeviceContext1 deviceContext)
        {
            for (int i = 0; i < Textures.Count; i++)
            {
                deviceContext.PSSetShaderResource(i, Textures[i].GetShaderResourceView());
            }
        }

        public virtual SamplerDescription CreateSamplerDescription()
        {
            SamplerDescription samplerDesc = new SamplerDescription()
            {
                Filter = Filter.MinMagMipPoint,
                AddressU = TextureAddressMode.Mirror,
                AddressV = TextureAddressMode.Mirror,
                AddressW = TextureAddressMode.Mirror,
                MipLODBias = 0,
                MaxAnisotropy = 1,
                ComparisonFunction = ComparisonFunction.Always,
                BorderColor = new Color4(0, 0, 0, 0),
                MinLOD = 0,
                MaxLOD = float.MaxValue
            };

            return samplerDesc;
        }


        public virtual void UpdateChild()
        {

        }
    }
}
