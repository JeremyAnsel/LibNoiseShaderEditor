using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.Maps;
using JeremyAnsel.LibNoiseShader.Renderers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LibNoiseShaderEditor.Converters
{
    public sealed class BuilderWindowImageConverter : IMultiValueConverter
    {
        public static readonly BuilderWindowImageConverter Default = new();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || values[0] is not IBuilder builder || values[1] is not Noise3D noise)
            {
                return null;
            }

            var task = Task.Run(async () =>
            {
                int width = 512;
                int height = 256;

                var renderer = new ImageRenderer(builder);
                renderer.BuildTerrainGradient();

                ColorMap data;

                try
                {
                    data = MapGenerator.GenerateColorMapOnGpu(noise, renderer, width, height);
                }
                catch (Exception ex)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current.MainWindow, ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                BitmapSource source = null;
                await Application.Current.Dispatcher.InvokeAsync(() => source = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, data.Data, width * 4));
                return source;
            });

            return new TaskCompletionNotifier<BitmapSource>(task);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
