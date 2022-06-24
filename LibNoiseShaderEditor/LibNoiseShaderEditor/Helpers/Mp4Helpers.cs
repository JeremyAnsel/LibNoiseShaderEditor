using MediaFoundation;
using MediaFoundation.ReadWrite;
using System;
using System.Runtime.InteropServices;

namespace LibNoiseShaderEditor.Helpers
{
    public static class Mp4Helpers
    {
        public static void Startup()
        {
            Marshal.ThrowExceptionForHR((int)MFExtern.MFStartup(0x20070, MFStartup.Full));
        }

        public static void Shutdown()
        {
            Marshal.ThrowExceptionForHR((int)MFExtern.MFShutdown());
        }

        public static void InitializeSinkWriter(string outputUrl, int width, int height, int fps, out IMFSinkWriter writer, out int videoStreamIndex)
        {
            Marshal.ThrowExceptionForHR((int)MFExtern.MFCreateAttributes(out IMFAttributes attributes, 0));

            try
            {
                Marshal.ThrowExceptionForHR((int)attributes.SetUINT32(MFAttributesClsid.MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS, 1));
                Marshal.ThrowExceptionForHR((int)attributes.SetUINT32(MFAttributesClsid.MF_SINK_WRITER_DISABLE_THROTTLING, 1));

                Marshal.ThrowExceptionForHR((int)MFExtern.MFCreateSinkWriterFromURL(outputUrl, null, attributes, out writer));
            }
            finally
            {
                Marshal.ReleaseComObject(attributes);
            }

            try
            {
                // Set the video output media type.
                Marshal.ThrowExceptionForHR((int)MFExtern.MFCreateMediaType(out IMFMediaType videoMediaTypeOut));

                try
                {
                    Marshal.ThrowExceptionForHR((int)videoMediaTypeOut.SetGUID(MFAttributesClsid.MF_MT_MAJOR_TYPE, MFMediaType.Video));
                    Marshal.ThrowExceptionForHR((int)videoMediaTypeOut.SetGUID(MFAttributesClsid.MF_MT_SUBTYPE, MFMediaType.H264));
                    Marshal.ThrowExceptionForHR((int)videoMediaTypeOut.SetUINT32(MFAttributesClsid.MF_MT_AVG_BITRATE, 1600000));
                    Marshal.ThrowExceptionForHR((int)videoMediaTypeOut.SetUINT32(MFAttributesClsid.MF_MT_INTERLACE_MODE, (int)MFVideoInterlaceMode.Progressive));
                    Marshal.ThrowExceptionForHR((int)MFExtern.MFSetAttributeSize(videoMediaTypeOut, MFAttributesClsid.MF_MT_FRAME_SIZE, width, height));
                    Marshal.ThrowExceptionForHR((int)MFExtern.MFSetAttributeRatio(videoMediaTypeOut, MFAttributesClsid.MF_MT_FRAME_RATE, fps, 1));
                    Marshal.ThrowExceptionForHR((int)MFExtern.MFSetAttributeRatio(videoMediaTypeOut, MFAttributesClsid.MF_MT_PIXEL_ASPECT_RATIO, 1, 1));
                    Marshal.ThrowExceptionForHR((int)writer.AddStream(videoMediaTypeOut, out videoStreamIndex));
                }
                finally
                {
                    Marshal.ReleaseComObject(videoMediaTypeOut);
                }

                // Set the video input media type.
                Marshal.ThrowExceptionForHR((int)MFExtern.MFCreateMediaType(out IMFMediaType videoMediaTypeIn));

                try
                {
                    Marshal.ThrowExceptionForHR((int)videoMediaTypeIn.SetGUID(MFAttributesClsid.MF_MT_MAJOR_TYPE, MFMediaType.Video));
                    Marshal.ThrowExceptionForHR((int)videoMediaTypeIn.SetGUID(MFAttributesClsid.MF_MT_SUBTYPE, MFMediaType.ARGB32));
                    Marshal.ThrowExceptionForHR((int)videoMediaTypeIn.SetUINT32(MFAttributesClsid.MF_MT_INTERLACE_MODE, (int)MFVideoInterlaceMode.Progressive));
                    Marshal.ThrowExceptionForHR((int)MFExtern.MFSetAttributeSize(videoMediaTypeIn, MFAttributesClsid.MF_MT_FRAME_SIZE, width, height));
                    Marshal.ThrowExceptionForHR((int)MFExtern.MFSetAttributeRatio(videoMediaTypeIn, MFAttributesClsid.MF_MT_FRAME_RATE, fps, 1));
                    Marshal.ThrowExceptionForHR((int)MFExtern.MFSetAttributeRatio(videoMediaTypeIn, MFAttributesClsid.MF_MT_PIXEL_ASPECT_RATIO, 1, 1));
                    Marshal.ThrowExceptionForHR((int)writer.SetInputMediaType(videoStreamIndex, videoMediaTypeIn, null));
                }
                finally
                {
                    Marshal.ReleaseComObject(videoMediaTypeIn);
                }
            }
            catch
            {
                Marshal.ReleaseComObject(writer);
                throw;
            }

            // Tell the sink writer to start accepting data.
            Marshal.ThrowExceptionForHR((int)writer.BeginWriting());
        }

        public static void WriteVideoFrame(long frameDuration, IMFSinkWriter writer, int videoStreamIndex, long rtStart, byte[] videoFrameBuffer)
        {
            Marshal.ThrowExceptionForHR((int)MFExtern.MFCreateSample(out IMFSample sample));

            try
            {
                Marshal.ThrowExceptionForHR((int)MFExtern.MFCreateMemoryBuffer(videoFrameBuffer.Length, out IMFMediaBuffer buffer));

                try
                {
                    Marshal.ThrowExceptionForHR((int)buffer.Lock(out IntPtr pData, out int maxLength, out int currentLength));

                    try
                    {
                        Marshal.Copy(videoFrameBuffer, 0, pData, videoFrameBuffer.Length);
                    }
                    finally
                    {
                        Marshal.ThrowExceptionForHR((int)buffer.Unlock());
                    }

                    Marshal.ThrowExceptionForHR((int)buffer.SetCurrentLength(videoFrameBuffer.Length));
                    Marshal.ThrowExceptionForHR((int)sample.AddBuffer(buffer));
                }
                finally
                {
                    Marshal.ReleaseComObject(buffer);
                }

                Marshal.ThrowExceptionForHR((int)sample.SetSampleTime(rtStart));
                Marshal.ThrowExceptionForHR((int)sample.SetSampleDuration(frameDuration));
                Marshal.ThrowExceptionForHR((int)writer.WriteSample(videoStreamIndex, sample));
            }
            finally
            {
                Marshal.ReleaseComObject(sample);
            }
        }
    }
}
