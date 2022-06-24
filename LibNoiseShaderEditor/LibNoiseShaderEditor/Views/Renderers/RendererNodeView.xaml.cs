using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.IO;
using JeremyAnsel.LibNoiseShader.IO.Models;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.Helpers;
using LibNoiseShaderEditor.ViewModels.RendererSettings;
using LibNoiseShaderEditor.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Renderers
{
    /// <summary>
    /// Logique d'interaction pour RendererNodeView.xaml
    /// </summary>
    public partial class RendererNodeView : UserControl
    {
        public static readonly DependencyProperty SettingProperty =
                DependencyProperty.Register(nameof(Setting), typeof(IRendererSetting), typeof(RendererNodeView), new PropertyMetadata(null));

        public IRendererSetting Setting
        {
            get => (IRendererSetting)GetValue(SettingProperty);
            set => SetValue(SettingProperty, value);
        }

        public static readonly DependencyProperty RendererProperty =
                DependencyProperty.Register(nameof(Renderer), typeof(IRenderer), typeof(RendererNodeView), new PropertyMetadata(null));

        public IRenderer Renderer
        {
            get => (IRenderer)GetValue(RendererProperty);
            set => SetValue(RendererProperty, value);
        }

        public static readonly DependencyProperty ModuleNoiseProperty =
                DependencyProperty.Register(nameof(ModuleNoise), typeof(Noise3D), typeof(RendererNodeView), new PropertyMetadata(null));

        public Noise3D ModuleNoise
        {
            get => (Noise3D)GetValue(ModuleNoiseProperty);
            set => SetValue(ModuleNoiseProperty, value);
        }

        public RendererNodeView()
        {
            InitializeComponent();
        }

        private void RendererExport_Click(object sender, RoutedEventArgs e)
        {
            if (Renderer is null || ModuleNoise is null)
            {
                return;
            }

            string fileName = FileDialogHelpers.GetSaveFileName(Application.Current.MainWindow, GlobalConstants.LibNoiseShaderDefaultExtension);

            if (fileName is null)
            {
                return;
            }

            try
            {
                LibNoiseShaderFile file = LibNoiseShaderFileWriteContext.BuildLibNoiseShaderFile(Renderer, ModuleNoise);
                file.Write(fileName);

                Xceed.Wpf.Toolkit.MessageBox.Show("Exported " + fileName);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RendererViewFullHlsl_Click(object sender, RoutedEventArgs e)
        {
            if (Renderer is null)
            {
                return;
            }

            string hlsl = Renderer.GetFullHlsl();

            var dialog = new TextWindow(hlsl, "hlsl")
            {
                Owner = Application.Current.MainWindow,
                Title = "HLSL"
            };

            dialog.ShowDialog();
        }

        private void RendererViewCS_Click(object sender, RoutedEventArgs e)
        {
            if (Renderer is null)
            {
                return;
            }

            string cs = Renderer.GetFullCSharp();

            var dialog = new TextWindow(cs, "cs")
            {
                Owner = Application.Current.MainWindow,
                Title = "CSharp"
            };

            dialog.ShowDialog();
        }

        private void RendererViewPreview_Click(object sender, RoutedEventArgs e)
        {
            if (Renderer is null || ModuleNoise is null)
            {
                return;
            }

            var window = new RendererWindow(Renderer, ModuleNoise)
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }

        private void RendererView3d_Click(object sender, RoutedEventArgs e)
        {
            if (Renderer is null || ModuleNoise is null)
            {
                return;
            }

            var window = new Viewer3dWindow(Renderer, ModuleNoise)
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }
    }
}
