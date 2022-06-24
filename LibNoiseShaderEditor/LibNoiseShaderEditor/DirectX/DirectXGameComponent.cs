using JeremyAnsel.DirectX.D3D11;
using JeremyAnsel.DirectX.D3DCompiler;
using JeremyAnsel.DirectX.Dxgi;
using JeremyAnsel.DirectX.DXMath;
using JeremyAnsel.DirectX.GameWindow;
using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.Renderers;
using System;

namespace LibNoiseShaderEditor.DirectX
{
    public sealed class DirectXGameComponent : IGameComponent
    {
        private const D3DCompileOptions compileOptions = D3DCompileOptions.SkipOptimization | D3DCompileOptions.SkipValidation;

        private readonly IRenderer renderer;

        private readonly Noise3D noise;

        private DeviceResources deviceResources;

        private D3D11ShaderResourceView permTextureView;

        private D3D11ShaderResourceView perm2DTextureView;

        private D3D11ShaderResourceView gradPermTextureView;

        private D3D11Buffer vertexBuffer;

        private D3D11Buffer indexBuffer;

        private int vertexCount;

        private int indexCount;

        private D3D11InputLayout inputLayout;

        private D3D11VertexShader vertexShader;

        private D3D11PixelShader pixelShader;

        private D3D11Buffer constantBuffer;

        private ConstantBufferData constantBufferData;

        private BasicCamera camera;

        public DirectXGameComponent(IRenderer renderer, Noise3D noise)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.noise = noise ?? throw new ArgumentNullException(nameof(noise));
        }

        public D3D11FeatureLevel MinimalFeatureLevel
        {
            get
            {
                return D3D11FeatureLevel.FeatureLevel100;
            }
        }

        public void CreateDeviceDependentResources(DeviceResources resources)
        {
            this.deviceResources = resources;

            this.CreatePermutationTexture(this.noise.RetrievePermutationBuffer());
            this.CreatePerm2DTexture(this.noise.RetrievePerm2DBuffer());
            this.CreateGradPermTexture(this.noise.RetrieveGradPermTexture());

            var shapes = new BasicShapes(this.deviceResources.D3DDevice);
            shapes.CreateSphere(out this.vertexBuffer, out this.indexBuffer, out this.vertexCount, out this.indexCount);

            CreateNoiseVertexShader(out this.vertexShader, out this.inputLayout);
            CreateNoisePixelShader(out this.pixelShader);

            var constantBufferDesc = new D3D11BufferDesc(ConstantBufferData.Size, D3D11BindOptions.ConstantBuffer);
            this.constantBuffer = this.deviceResources.D3DDevice.CreateBuffer(constantBufferDesc);

            this.camera = new BasicCamera();
        }

        private void CreateNoiseVertexShader(out D3D11VertexShader shader, out D3D11InputLayout layout)
        {
            string vertexShaderHlsl = @"
cbuffer ConstantBuffer : register(b0)
{
    matrix model;
    matrix view;
    matrix projection;
};

struct VertexShaderInput
{
    float3 pos : POSITION;
    float3 norm : NORMAL;
    float2 tex : TEXCOORD0;
};

struct PixelShaderInput
{
    float4 pos : SV_POSITION;
    float3 norm : NORMAL;
    float2 tex : TEXCOORD0;
};

".NormalizeEndLines();

            if (this.renderer is ImageRenderer imageRenderer)
            {
                IBuilder builder = imageRenderer.GetSourceBuilder();
                vertexShaderHlsl += builder.GetFullHlsl();

                vertexShaderHlsl += @"
PixelShaderInput main(VertexShaderInput input)
{
    PixelShaderInput vertexShaderOutput;
    float4 pos = float4(input.pos, 1.0f);

    float v = Builder_root(input.tex.x * 2.0 - 1.0, input.tex.y * 2.0 - 1.0) * 0.5 + 0.5;
    pos.xyz += input.norm * v * 0.05;

    pos = mul(pos, model);
    pos = mul(pos, view);
    pos = mul(pos, projection);
    vertexShaderOutput.pos = pos;
    vertexShaderOutput.tex = input.tex;
    vertexShaderOutput.norm = mul(float4(input.norm, 1.0f), model).xyz;
    return vertexShaderOutput;
}
".NormalizeEndLines();
            }
            else
            {
                vertexShaderHlsl += @"
PixelShaderInput main(VertexShaderInput input)
{
    PixelShaderInput vertexShaderOutput;
    float4 pos = float4(input.pos, 1.0f);
    pos = mul(pos, model);
    pos = mul(pos, view);
    pos = mul(pos, projection);
    vertexShaderOutput.pos = pos;
    vertexShaderOutput.tex = input.tex;
    vertexShaderOutput.norm = mul(float4(input.norm, 1.0f), model).xyz;
    return vertexShaderOutput;
}
".NormalizeEndLines();
            }

            D3DCompile.Compile(
                vertexShaderHlsl,
                nameof(vertexShaderHlsl),
                "main",
                D3DTargets.VS_4_0,
                compileOptions,
                out byte[] vertexShaderBytecode,
                out string _);

            shader = this.deviceResources.D3DDevice.CreateVertexShader(vertexShaderBytecode, null);

            D3D11InputElementDesc[] basicVertexLayoutDesc = new D3D11InputElementDesc[]
            {
                    new D3D11InputElementDesc
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = DxgiFormat.R32G32B32Float,
                        InputSlot = 0,
                        AlignedByteOffset = 0,
                        InputSlotClass = D3D11InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new D3D11InputElementDesc
                    {
                        SemanticName = "NORMAL",
                        SemanticIndex = 0,
                        Format = DxgiFormat.R32G32B32Float,
                        InputSlot = 0,
                        AlignedByteOffset = 12,
                        InputSlotClass = D3D11InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new D3D11InputElementDesc
                    {
                        SemanticName = "TEXCOORD",
                        SemanticIndex = 0,
                        Format = DxgiFormat.R32G32Float,
                        InputSlot = 0,
                        AlignedByteOffset = 24,
                        InputSlotClass = D3D11InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    }
            };

            try
            {
                layout = this.deviceResources.D3DDevice.CreateInputLayout(basicVertexLayoutDesc, vertexShaderBytecode);
            }
            catch
            {
                D3D11Utils.DisposeAndNull(ref shader);
                throw;
            }
        }

        private void CreateNoisePixelShader(out D3D11PixelShader shader)
        {
            string pixelShaderHlsl = this.renderer.GetFullHlsl() + @"
struct PixelShaderInput
{
    float4 pos : SV_POSITION;
    float3 norm : NORMAL;
    float2 tex : TEXCOORD0;
};

float4 main(PixelShaderInput input) : SV_TARGET
{
    input.tex -= float2( abs(ddx(input.tex.x)) * 0.5f, abs(ddy(input.tex.y)) * 0.5f );
    float4 color = Renderer_root(input.tex.x * 2.0 - 1.0, input.tex.y * 2.0 - 1.0);
    float3 lightDirection = normalize(float3(1, -0.5, 0));
    color *= 0.8f * saturate(dot(input.norm, -lightDirection)) + 0.2f;
    return float4(color.xyz, 1.0f);
}
".NormalizeEndLines();

            D3DCompile.Compile(
                pixelShaderHlsl,
                nameof(pixelShaderHlsl),
                "main",
                D3DTargets.PS_4_0,
                compileOptions,
                out byte[] pixelShaderBytecode,
                out string _);

            shader = this.deviceResources.D3DDevice.CreatePixelShader(pixelShaderBytecode, null);
        }

        private void CreatePermutationTexture(int[] perm)
        {
            D3D11Texture1DDesc textureDesc = new(DxgiFormat.R32UInt, 256, 1, 1);

            D3D11SubResourceData[] textureSubResData = new[]
                {
                    new D3D11SubResourceData(perm, 256 * 4)
                };

            using var texture = this.deviceResources.D3DDevice.CreateTexture1D(textureDesc, textureSubResData);
            this.permTextureView = this.deviceResources.D3DDevice.CreateShaderResourceView(texture, null);
        }

        private void CreatePerm2DTexture(int[] perm2D)
        {
            D3D11Texture2DDesc textureDesc = new(DxgiFormat.R32G32B32A32UInt, 256, 256, 1, 1);

            D3D11SubResourceData[] textureSubResData = new[]
                {
                    new D3D11SubResourceData(perm2D, 256 * 16)
                };

            using var texture = this.deviceResources.D3DDevice.CreateTexture2D(textureDesc, textureSubResData);
            this.perm2DTextureView = this.deviceResources.D3DDevice.CreateShaderResourceView(texture, null);
        }

        private void CreateGradPermTexture(float[] grad)
        {
            D3D11Texture1DDesc textureDesc = new(DxgiFormat.R32G32B32A32Float, 512, 1, 1);

            D3D11SubResourceData[] textureSubResData = new[]
                {
                    new D3D11SubResourceData(grad, 512 * 16)
                };

            using var texture = this.deviceResources.D3DDevice.CreateTexture1D(textureDesc, textureSubResData);
            this.gradPermTextureView = this.deviceResources.D3DDevice.CreateShaderResourceView(texture, null);
        }

        public void ReleaseDeviceDependentResources()
        {
            D3D11Utils.DisposeAndNull(ref this.permTextureView);
            D3D11Utils.DisposeAndNull(ref this.perm2DTextureView);
            D3D11Utils.DisposeAndNull(ref this.gradPermTextureView);
            D3D11Utils.DisposeAndNull(ref this.vertexBuffer);
            D3D11Utils.DisposeAndNull(ref this.indexBuffer);
            D3D11Utils.DisposeAndNull(ref this.inputLayout);
            D3D11Utils.DisposeAndNull(ref this.vertexShader);
            D3D11Utils.DisposeAndNull(ref this.pixelShader);
            D3D11Utils.DisposeAndNull(ref this.constantBuffer);
        }

        public void CreateWindowSizeDependentResources()
        {
            this.camera.SetProjectionParameters(70.0f, this.deviceResources.ScreenViewport.Width / this.deviceResources.ScreenViewport.Height, 0.01f, 100.0f);
            this.constantBufferData.Projection = this.camera.GetProjectionMatrix();
        }

        public void ReleaseWindowSizeDependentResources()
        {
        }

        public void Update(ITimer timer)
        {
            this.constantBufferData.Model = XMMatrix.RotationY((float)(-(timer?.TotalSeconds ?? 0.0) * XMMath.ConvertToRadians(60.0f)));

            this.camera.SetViewParameters(new XMFloat3(0, 1.0f, 2.0f), new XMFloat3(0, 0, 0), new XMFloat3(0, 1, 0));
            this.constantBufferData.View = this.camera.GetViewMatrix();

            this.deviceResources.D3DContext.UpdateSubresource(this.constantBuffer, 0, null, this.constantBufferData, 0, 0);
        }

        public void Render()
        {
            var context = this.deviceResources.D3DContext;

            context.OutputMergerSetRenderTargets(new[] { this.deviceResources.D3DRenderTargetView }, this.deviceResources.D3DDepthStencilView);
            context.ClearRenderTargetView(this.deviceResources.D3DRenderTargetView, XMKnownColor.Black);
            context.ClearDepthStencilView(this.deviceResources.D3DDepthStencilView, D3D11ClearOptions.Depth, 1.0f, 0);

            context.InputAssemblerSetInputLayout(this.inputLayout);

            context.InputAssemblerSetVertexBuffers(
                0,
                new[] { this.vertexBuffer },
                new uint[] { BasicVertex.Size },
                new uint[] { 0 });

            context.InputAssemblerSetIndexBuffer(this.indexBuffer, DxgiFormat.R16UInt, 0);
            context.InputAssemblerSetPrimitiveTopology(D3D11PrimitiveTopology.TriangleList);

            context.VertexShaderSetShader(this.vertexShader, null);
            context.VertexShaderSetConstantBuffers(0, new[] { this.constantBuffer });
            context.VertexShaderSetShaderResources(0, new[] { this.permTextureView, this.perm2DTextureView, this.gradPermTextureView });

            context.PixelShaderSetShader(this.pixelShader, null);
            context.PixelShaderSetShaderResources(0, new[] { this.permTextureView, this.perm2DTextureView, this.gradPermTextureView });

            context.DrawIndexed((uint)this.indexCount, 0, 0);
        }
    }
}
