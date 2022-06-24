using JeremyAnsel.DirectX.DXMath;
using System;

namespace LibNoiseShaderEditor.DirectX
{
    public sealed class BasicCamera
    {
        private XMFloat3 position;

        private XMFloat3 direction;

        private XMFloat4X4 view;

        private XMFloat4X4 projection;

        public XMFloat4X4 GetViewMatrix()
        {
            return this.view;
        }

        public XMFloat4X4 GetProjectionMatrix()
        {
            return this.projection;
        }

        public void SetViewParameters(XMFloat3 eyePosition, XMFloat3 lookPosition, XMFloat3 up)
        {
            this.position = eyePosition;
            this.direction = XMVector3.Normalize(XMVector.Subtract(lookPosition, eyePosition));

            XMVector zAxis = this.direction.ToVector().Negate();
            XMVector xAxis = XMVector3.Normalize(XMVector3.Cross(up, zAxis));
            XMVector yAxis = XMVector3.Cross(zAxis, xAxis);
            float xOffset = -XMVector3.Dot(xAxis, this.position).X;
            float yOffset = -XMVector3.Dot(yAxis, this.position).X;
            float zOffset = -XMVector3.Dot(zAxis, this.position).X;

            this.view = new XMFloat4X4(
                xAxis.X, xAxis.Y, xAxis.Z, xOffset,
                yAxis.X, yAxis.Y, yAxis.Z, yOffset,
                zAxis.X, zAxis.Y, zAxis.Z, zOffset,
                0.0f, 0.0f, 0.0f, 1.0f
                );
        }

        public void SetProjectionParameters(float minimumFieldOfView, float aspectRatio, float nearPlane, float farPlane)
        {
            float minScale = 1.0f / (float)Math.Tan(minimumFieldOfView * XMMath.PI / 360.0f);
            float xScale;
            float yScale;

            if (aspectRatio < 1.0f)
            {
                xScale = minScale;
                yScale = minScale * aspectRatio;
            }
            else
            {
                xScale = minScale / aspectRatio;
                yScale = minScale;
            }

            float zScale = farPlane / (farPlane - nearPlane);

            this.projection = new XMFloat4X4(
                xScale, 0.0f, 0.0f, 0.0f,
                0.0f, yScale, 0.0f, 0.0f,
                0.0f, 0.0f, -zScale, -nearPlane * zScale,
                0.0f, 0.0f, -1.0f, 0.0f
                );
        }
    }
}
