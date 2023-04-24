using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.Maps;
using JeremyAnsel.LibNoiseShader.Modules;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.Helpers;
using System;
using System.Windows;

namespace LibNoiseShaderEditor.Windows
{
    /// <summary>
    /// Logique d'interaction pour ModuleWindow.xaml
    /// </summary>
    public partial class ModuleWindow : Window
    {
        public static readonly DependencyProperty ModuleProperty =
                DependencyProperty.Register(nameof(Module), typeof(IModule), typeof(ModuleWindow), new PropertyMetadata(null));

        public IModule Module
        {
            get => (IModule)GetValue(ModuleProperty);
            set => SetValue(ModuleProperty, value);
        }

        public static readonly DependencyProperty ModuleNoiseProperty =
                DependencyProperty.Register(nameof(ModuleNoise), typeof(Noise3D), typeof(ModuleWindow), new PropertyMetadata(null));

        public Noise3D ModuleNoise
        {
            get => (Noise3D)GetValue(ModuleNoiseProperty);
            set => SetValue(ModuleNoiseProperty, value);
        }

        public static readonly DependencyProperty ExportWidthProperty =
            DependencyProperty.Register(nameof(ExportWidth), typeof(int), typeof(ModuleWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultWidth, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinWidth), GlobalConstants.ExportMaxWidth)));

        public int ExportWidth
        {
            get { return (int)GetValue(ExportWidthProperty); }
            set { SetValue(ExportWidthProperty, value); }
        }

        public static readonly DependencyProperty ExportHeightProperty =
            DependencyProperty.Register(nameof(ExportHeight), typeof(int), typeof(ModuleWindow),
                new PropertyMetadata(GlobalConstants.ExportDefaultHeight, null, (dp, v) => Math.Min(Math.Max((int)v, GlobalConstants.ExportMinHeight), GlobalConstants.ExportMaxHeight)));

        public int ExportHeight
        {
            get { return (int)GetValue(ExportHeightProperty); }
            set { SetValue(ExportHeightProperty, value); }
        }

        public ModuleWindow(IModule module, Noise3D moduleNoise)
        {
            InitializeComponent();

            this.Module = module ?? throw new ArgumentNullException(nameof(module));
            this.ModuleNoise = moduleNoise ?? throw new ArgumentNullException(nameof(moduleNoise));
        }

        private void ExportPlaneGrayscale_Click(object sender, RoutedEventArgs e)
        {
            var builder = new PlaneBuilder(Module);
            var renderer = new ImageRenderer(builder);
            renderer.BuildGrayscaleGradient();
            ExportRenderer(renderer);
        }

        private void ExportPlaneColor_Click(object sender, RoutedEventArgs e)
        {
            var builder = new PlaneBuilder(Module);
            var renderer = new ImageRenderer(builder);
            renderer.BuildTerrainGradient();
            ExportRenderer(renderer);
        }

        private void ExportCylinderGrayscale_Click(object sender, RoutedEventArgs e)
        {
            var builder = new CylinderBuilder(Module);
            var renderer = new ImageRenderer(builder);
            renderer.BuildGrayscaleGradient();
            ExportRenderer(renderer);
        }

        private void ExportCylinderColor_Click(object sender, RoutedEventArgs e)
        {
            var builder = new CylinderBuilder(Module);
            var renderer = new ImageRenderer(builder);
            renderer.BuildTerrainGradient();
            ExportRenderer(renderer);
        }

        private void ExportSphereGrayscale_Click(object sender, RoutedEventArgs e)
        {
            var builder = new SphereBuilder(Module);
            var renderer = new ImageRenderer(builder);
            renderer.BuildGrayscaleGradient();
            ExportRenderer(renderer);
        }

        private void ExportSphereColor_Click(object sender, RoutedEventArgs e)
        {
            var builder = new SphereBuilder(Module);
            var renderer = new ImageRenderer(builder);
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
                    .GenerateColorMapOnCpu(noise, renderer, width, height)
                    .SaveBitmap(fileName);
            });
        }
    }
}
