using JeremyAnsel.DirectX.D3D11;
using JeremyAnsel.DirectX.DXMath;
using System;

namespace LibNoiseShaderEditor.DirectX
{
    public sealed class BasicShapes
    {
        private readonly D3D11Device d3dDevice;

        public BasicShapes(D3D11Device d3dDevice)
        {
            this.d3dDevice = d3dDevice ?? throw new ArgumentNullException(nameof(d3dDevice));
        }

        public void CreateSphere(out D3D11Buffer vertexBuffer, out D3D11Buffer indexBuffer, out int vertexCount, out int indexCount)
        {
            const int numSegments = 64;
            const int numSlices = numSegments / 2;

            const int numVertices = (numSlices + 1) * (numSegments + 1);
            var sphereVertices = new BasicVertex[numVertices];

            for (int slice = 0; slice <= numSlices; slice++)
            {
                float v = (float)slice / numSlices;
                float inclination = v * XMMath.PI;
                float y = (float)Math.Cos(inclination);
                float r = (float)Math.Sin(inclination);

                for (int segment = 0; segment <= numSegments; segment++)
                {
                    float u = (float)segment / numSegments;
                    float azimuth = u * XMMath.PI * 2.0f;
                    int vetexIndex = slice * (numSegments + 1) + segment;

                    sphereVertices[vetexIndex].Position = new XMFloat3(r * (float)Math.Sin(azimuth), y, r * (float)Math.Cos(azimuth));
                    sphereVertices[vetexIndex].Normal = sphereVertices[vetexIndex].Position;
                    sphereVertices[vetexIndex].TextureCoordinates = new XMFloat2(u, v);
                }
            }

            const int numIndices = numSlices * (numSegments - 2) * 6;
            var sphereIndices = new ushort[numIndices];

            uint index = 0;
            for (int slice = 0; slice < numSlices; slice++)
            {
                ushort sliceBase0 = (ushort)((slice) * (numSegments + 1));
                ushort sliceBase1 = (ushort)((slice + 1) * (numSegments + 1));

                for (int segment = 0; segment < numSegments; segment++)
                {
                    if (slice > 0)
                    {
                        sphereIndices[index++] = (ushort)(sliceBase0 + segment);
                        sphereIndices[index++] = (ushort)(sliceBase0 + segment + 1);
                        sphereIndices[index++] = (ushort)(sliceBase1 + segment + 1);
                    }

                    if (slice < numSlices - 1)
                    {
                        sphereIndices[index++] = (ushort)(sliceBase0 + segment);
                        sphereIndices[index++] = (ushort)(sliceBase1 + segment + 1);
                        sphereIndices[index++] = (ushort)(sliceBase1 + segment);
                    }
                }
            }

            this.CreateVertexBuffer(sphereVertices, out vertexBuffer);

            try
            {
                this.CreateIndexBuffer(sphereIndices, out indexBuffer);
            }
            catch
            {
                D3D11Utils.DisposeAndNull(ref vertexBuffer);
                throw;
            }

            vertexCount = numVertices;
            indexCount = numIndices;
        }

        private void CreateVertexBuffer(BasicVertex[] vertexData, out D3D11Buffer vertexBuffer)
        {
            D3D11BufferDesc vertexBufferDesc = D3D11BufferDesc.From(vertexData, D3D11BindOptions.VertexBuffer);
            vertexBuffer = this.d3dDevice.CreateBuffer(vertexBufferDesc, vertexData, 0, 0);
        }

        private void CreateIndexBuffer(ushort[] indexData, out D3D11Buffer indexBuffer)
        {
            D3D11BufferDesc indexBufferDesc = D3D11BufferDesc.From(indexData, D3D11BindOptions.IndexBuffer);
            indexBuffer = this.d3dDevice.CreateBuffer(indexBufferDesc, indexData, 0, 0);
        }
    }
}
