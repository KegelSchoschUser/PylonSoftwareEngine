/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace PylonSoftwareEngine.Render11
{
    public class RenderTexture : Texture
    {
        internal ID3D11RenderTargetView InternalRenderTarget;
        internal ID3D11Texture2D DepthStencilBuffer;
        internal ID3D11DepthStencilView DepthStencilView;


        public RenderTexture(int width, int height) : base(width, height)
        {
            CreateDepth();
            CreateRenderTarget();
        }

        public RenderTexture(Vector2 size) : base((int)size.X, (int)size.Y)
        {
            CreateDepth();
            CreateRenderTarget();
        }

        protected override void Refresh(int width, int height)
        {
            base.Refresh(width, height);

            CreateDepth();
            CreateRenderTarget();
        }

        private void CreateDepth()
        {
            if(DepthStencilBuffer != null)
                DepthStencilBuffer.Release();
            DepthStencilBuffer = D3D11GraphicsDevice.Device.CreateTexture2D((int)Size.X, (int)Size.Y, Format.D32_Float, 1, 1, null, BindFlags.DepthStencil);

            if (DepthStencilView != null)
                DepthStencilView.Release();
            DepthStencilView = D3D11GraphicsDevice.Device.CreateDepthStencilView(DepthStencilBuffer);
        }

        private void CreateRenderTarget()
        {
            if (InternalRenderTarget != null)
                InternalRenderTarget.Release();
            InternalRenderTarget = D3D11GraphicsDevice.Device.CreateRenderTargetView(InternalTexture);
            Clear();
        }

        internal virtual void OnRender()
        {

        }


        public void Clear()
        {
            lock (MySoftware.RenderLock)
            {
                D3D11GraphicsDevice.DeviceContext.ClearRenderTargetView(InternalRenderTarget, RGBColor.Transparent);
                D3D11GraphicsDevice.DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1f, 0);
            }
        }
    }
}