using JeremyAnsel.DirectX.D3D11;
using JeremyAnsel.DirectX.GameWindow;
using JeremyAnsel.DirectX.Window.Wpf;
using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Maps;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.DirectX;
using LibNoiseShaderEditor.Helpers;
using MediaFoundation.ReadWrite;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace LibNoiseShaderEditor.Windows
{
    /// <summary>
    /// Logique d'interaction pour Viewer3dWindow.xaml
    /// </summary>
    public partial class Viewer3dWindow : Window
    {
        private readonly IRenderer _renderer;

        private readonly Noise3D _noise;

        private readonly DirectXGame _game;

        public static readonly DependencyProperty ExportWidthProperty =
            DependencyProperty.Register(nameof(ExportWidth), typeof(int), typeof(Viewer3dWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultWidth, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinWidth), GlobalConstants.ExportMaxWidth)));

        public int ExportWidth
        {
            get { return (int)GetValue(ExportWidthProperty); }
            set { SetValue(ExportWidthProperty, value); }
        }

        public static readonly DependencyProperty ExportHeightProperty =
            DependencyProperty.Register(nameof(ExportHeight), typeof(int), typeof(Viewer3dWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultHeight, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinHeight), GlobalConstants.ExportMaxHeight)));

        public int ExportHeight
        {
            get { return (int)GetValue(ExportHeightProperty); }
            set { SetValue(ExportHeightProperty, value); }
        }

        public Viewer3dWindow(IRenderer renderer, Noise3D noise)
        {
            InitializeComponent();

            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _noise = noise ?? throw new ArgumentNullException(nameof(noise));

            _game = new DirectXGame(_renderer, _noise);
            ControlHostElement.Child = new WindowHost(_game);
        }

        private void ExportImage_Click(object sender, RoutedEventArgs e)
        {
            string fileName = FileDialogHelpers.GetSaveFileName(this, new[] { "png", "bmp", "jpg" });

            if (fileName is null)
            {
                return;
            }

            int width = ExportWidth;
            int height = ExportHeight;

            BusyAction.Run(BusyIndicator, () =>
            {
                byte[] data;

                var options = new DeviceResourcesOptions
                {
                    ForceWarp = false,
                    UseHighestFeatureLevel = false
                };

                var deviceResources = new RenderTargetDeviceResources((uint)width, (uint)height, D3D11FeatureLevel.FeatureLevel100, options);
                var component = new DirectXGameComponent(_renderer, _noise);

                try
                {
                    component.CreateDeviceDependentResources(deviceResources);
                    component.CreateWindowSizeDependentResources();
                    component.Update(null);
                    component.Render();
                    deviceResources.Present();

                    data = deviceResources.GetBackBufferContent();
                }
                finally
                {
                    component.ReleaseWindowSizeDependentResources();
                    component.ReleaseDeviceDependentResources();
                    deviceResources.Release();
                }

                var colorMap = new ColorMap(width, height, data);
                colorMap.SaveBitmap(fileName);
            });
        }

        private void ExportMp4_Click(object sender, RoutedEventArgs e)
        {
            string fileName = FileDialogHelpers.GetSaveFileName(this, "mp4");

            if (fileName is null)
            {
                return;
            }

            int width = ExportWidth;
            int height = ExportHeight;

            BusyAction.Run(BusyIndicator, disp =>
            {
                disp(() => ControlHostElement.Visibility = Visibility.Hidden);

                var options = new DeviceResourcesOptions
                {
                    ForceWarp = false,
                    UseHighestFeatureLevel = false
                };

                var deviceResources = new RenderTargetDeviceResources((uint)width, (uint)height, D3D11FeatureLevel.FeatureLevel100, options);
                var component = new DirectXGameComponent(_renderer, _noise);

                bool mp4Startup = false;
                IMFSinkWriter writer = null;

                try
                {
                    component.CreateDeviceDependentResources(deviceResources);
                    component.CreateWindowSizeDependentResources();

                    int fps = 60;
                    long frameDuration = 10 * 1000 * 1000 / fps;

                    Mp4Helpers.Startup();
                    mp4Startup = true;

                    Mp4Helpers.InitializeSinkWriter(
                        fileName,
                        width,
                        height,
                        fps,
                        out writer,
                        out int videoStream);

                    int framesCount = fps * 6;

                    long rtStartVideo = 0;

                    var timer = new FixedTimer();

                    for (int frameIndex = 0; frameIndex < framesCount; frameIndex++)
                    {
                        disp(() => BusyIndicator.BusyContent = $"Computing frame {frameIndex} / {framesCount}");
                        component.Update(timer);
                        component.Render();
                        deviceResources.Present();

                        byte[] videoData = deviceResources.GetBackBufferContent();

                        Mp4Helpers.WriteVideoFrame(frameDuration, writer, videoStream, rtStartVideo, videoData);
                        rtStartVideo += frameDuration;
                        timer.Tick(1.0 / fps);
                    }

                    disp(() => BusyIndicator.BusyContent = "Finalize");
                    writer.Finalize_();
                }
                finally
                {
                    if (writer is not null)
                    {
                        Marshal.ReleaseComObject(writer);
                    }

                    if (mp4Startup)
                    {
                        Mp4Helpers.Shutdown();
                    }

                    component.ReleaseWindowSizeDependentResources();
                    component.ReleaseDeviceDependentResources();
                    deviceResources.Release();
                }

                disp(() => ControlHostElement.Visibility = Visibility.Visible);
            });
        }
    }
}
