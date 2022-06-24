using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.Maps;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.Helpers;
using System;
using System.Windows;

namespace LibNoiseShaderEditor.Windows
{
    /// <summary>
    /// Logique d'interaction pour BuilderWindow.xaml
    /// </summary>
    public partial class BuilderWindow : Window
    {
        public static readonly DependencyProperty BuilderProperty =
                DependencyProperty.Register(nameof(Builder), typeof(IBuilder), typeof(BuilderWindow), new PropertyMetadata(null));

        public IBuilder Builder
        {
            get => (IBuilder)GetValue(BuilderProperty);
            set => SetValue(BuilderProperty, value);
        }

        public static readonly DependencyProperty ModuleNoiseProperty =
                DependencyProperty.Register(nameof(ModuleNoise), typeof(Noise3D), typeof(BuilderWindow), new PropertyMetadata(null));

        public Noise3D ModuleNoise
        {
            get => (Noise3D)GetValue(ModuleNoiseProperty);
            set => SetValue(ModuleNoiseProperty, value);
        }

        public static readonly DependencyProperty ExportWidthProperty =
            DependencyProperty.Register(nameof(ExportWidth), typeof(int), typeof(BuilderWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultWidth, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinWidth), GlobalConstants.ExportMaxWidth)));

        public int ExportWidth
        {
            get { return (int)GetValue(ExportWidthProperty); }
            set { SetValue(ExportWidthProperty, value); }
        }

        public static readonly DependencyProperty ExportHeightProperty =
            DependencyProperty.Register(nameof(ExportHeight), typeof(int), typeof(BuilderWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultHeight, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinHeight), GlobalConstants.ExportMaxHeight)));

        public int ExportHeight
        {
            get { return (int)GetValue(ExportHeightProperty); }
            set { SetValue(ExportHeightProperty, value); }
        }

        public BuilderWindow(IBuilder builder, Noise3D moduleNoise)
        {
            InitializeComponent();

            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.ModuleNoise = moduleNoise ?? throw new ArgumentNullException(nameof(moduleNoise));
        }

        private void ExportGrayscale_Click(object sender, RoutedEventArgs e)
        {
            var renderer = new ImageRenderer(Builder);
            renderer.BuildGrayscaleGradient();
            ExportRenderer(renderer);
        }

        private void ExportColor_Click(object sender, RoutedEventArgs e)
        {
            var renderer = new ImageRenderer(Builder);
            renderer.BuildTerrainGradient();
            ExportRenderer(renderer);
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
                    .GenerateColorMapOnGpu(noise, renderer, width, height)
                    .SaveBitmap(fileName);
            });
        }
    }
}
