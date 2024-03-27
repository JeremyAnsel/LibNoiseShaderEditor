using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Maps;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.Helpers;
using System;
using System.Windows;

namespace LibNoiseShaderEditor.Windows
{
    /// <summary>
    /// Logique d'interaction pour RendererWindow.xaml
    /// </summary>
    public partial class RendererWindow : Window
    {
        public static readonly DependencyProperty RendererProperty =
                DependencyProperty.Register(nameof(Renderer), typeof(IRenderer), typeof(RendererWindow), new PropertyMetadata(null));

        public IRenderer Renderer
        {
            get => (IRenderer)GetValue(RendererProperty);
            set => SetValue(RendererProperty, value);
        }

        public static readonly DependencyProperty ModuleNoiseProperty =
                DependencyProperty.Register(nameof(ModuleNoise), typeof(Noise3D), typeof(RendererWindow), new PropertyMetadata(null));

        public Noise3D ModuleNoise
        {
            get => (Noise3D)GetValue(ModuleNoiseProperty);
            set => SetValue(ModuleNoiseProperty, value);
        }

        public static readonly DependencyProperty ExportWidthProperty =
            DependencyProperty.Register(nameof(ExportWidth), typeof(int), typeof(RendererWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultWidth, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinWidth), GlobalConstants.ExportMaxWidth)));

        public int ExportWidth
        {
            get { return (int)GetValue(ExportWidthProperty); }
            set { SetValue(ExportWidthProperty, value); }
        }

        public static readonly DependencyProperty ExportHeightProperty =
            DependencyProperty.Register(nameof(ExportHeight), typeof(int), typeof(RendererWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultHeight, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinHeight), GlobalConstants.ExportMaxHeight)));

        public int ExportHeight
        {
            get { return (int)GetValue(ExportHeightProperty); }
            set { SetValue(ExportHeightProperty, value); }
        }

        public RendererWindow(IRenderer renderer, Noise3D moduleNoise)
        {
            InitializeComponent();

            this.Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.ModuleNoise = moduleNoise ?? throw new ArgumentNullException(nameof(moduleNoise));
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportRenderer(Renderer);
        }

        private void ExportRenderer(IRenderer renderer)
        {
            string fileName = FileDialogHelpers.GetSaveFileName(this, new[] { "png", "bmp", "jpg" });

            if (fileName is null)
            {
                return;
            }

            Noise3D noise = ModuleNoise;
            int width = ExportWidth;
            int height = ExportHeight;

            BusyAction.Run(BusyIndicator, () =>
            {
                MapGenerator
                    .GenerateColorMapOnCpu(renderer, width, height)
                    .SaveBitmap(fileName);
            });
        }
    }
}
