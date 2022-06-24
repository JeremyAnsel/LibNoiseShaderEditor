using JeremyAnsel.DirectX.DXMath;
using System.Runtime.InteropServices;

namespace LibNoiseShaderEditor.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BasicVertex
    {
        public XMFloat3 Position;

        public XMFloat3 Normal;

        public XMFloat2 TextureCoordinates;

        public static readonly uint Size = (uint)Marshal.SizeOf<BasicVertex>();

        public BasicVertex(XMFloat3 position, XMFloat3 normal, XMFloat2 textureCoordinates)
        {
            this.Position = position;
            this.Normal = normal;
            this.TextureCoordinates = textureCoordinates;
        }
    }
}
