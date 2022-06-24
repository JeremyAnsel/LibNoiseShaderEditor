using JeremyAnsel.DirectX.DXMath;
using System.Runtime.InteropServices;

namespace LibNoiseShaderEditor.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ConstantBufferData
    {
        public XMFloat4X4 Model;

        public XMFloat4X4 View;

        public XMFloat4X4 Projection;

        public static readonly uint Size = (uint)Marshal.SizeOf<ConstantBufferData>();
    }
}
