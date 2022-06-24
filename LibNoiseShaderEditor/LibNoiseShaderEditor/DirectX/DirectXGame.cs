using JeremyAnsel.DirectX.D3D11;
using JeremyAnsel.DirectX.GameWindow;
using JeremyAnsel.DirectX.Window;
using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Renderers;
using System;

namespace LibNoiseShaderEditor.DirectX
{
    public sealed class DirectXGame : GameWindowBase
    {
        private readonly IRenderer renderer;

        private readonly Noise3D noise;

        private DirectXGameComponent gameComponent;

        public DirectXGame(IRenderer renderer, Noise3D noise)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.noise = noise ?? throw new ArgumentNullException(nameof(noise));

#if DEBUG
            this.DeviceResourcesOptions.Debug = true;
#endif

            this.RequestedD3DFeatureLevel = D3D11FeatureLevel.FeatureLevel100;
        }

        protected override void Init()
        {
            this.gameComponent = this.CheckMinimalFeatureLevel(new DirectXGameComponent(this.renderer, this.noise));

            base.Init();

            this.ExitOnEscapeKey = false;
            this.FpsTextRenderer.IsEnabled = false;
        }

        protected override void CreateDeviceDependentResources()
        {
            base.CreateDeviceDependentResources();

            this.gameComponent.CreateDeviceDependentResources(this.DeviceResources);
        }

        protected override void ReleaseDeviceDependentResources()
        {
            base.ReleaseDeviceDependentResources();

            this.gameComponent.ReleaseDeviceDependentResources();
        }

        protected override void CreateWindowSizeDependentResources()
        {
            base.CreateWindowSizeDependentResources();

            this.gameComponent.CreateWindowSizeDependentResources();
        }

        protected override void ReleaseWindowSizeDependentResources()
        {
            base.ReleaseWindowSizeDependentResources();

            this.gameComponent.ReleaseWindowSizeDependentResources();
        }

        protected override void Update()
        {
            base.Update();

            this.gameComponent.Update(this.Timer);
        }

        protected override void Render()
        {
            this.gameComponent.Render();
        }

        protected override void OnKeyboardEvent(VirtualKey key, int repeatCount, bool wasDown, bool isDown)
        {
            base.OnKeyboardEvent(key, repeatCount, wasDown, isDown);
        }
    }
}
