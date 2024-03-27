using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.IO;
using JeremyAnsel.LibNoiseShader.IO.Models;
using JeremyAnsel.LibNoiseShader.Modules;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.Helpers;
using LibNoiseShaderEditor.ViewModels.ModuleSettings;
using LibNoiseShaderEditor.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour ModuleNodeView.xaml
    /// </summary>
    public partial class ModuleNodeView : UserControl
    {
        public static readonly DependencyProperty SettingProperty =
                DependencyProperty.Register(nameof(Setting), typeof(IModuleSetting), typeof(ModuleNodeView), new PropertyMetadata(null));

        public IModuleSetting Setting
        {
            get => (IModuleSetting)GetValue(SettingProperty);
            set => SetValue(SettingProperty, value);
        }

        public static readonly DependencyProperty ModuleProperty =
                DependencyProperty.Register(nameof(Module), typeof(IModule), typeof(ModuleNodeView), new PropertyMetadata(null));

        public IModule Module
        {
            get => (IModule)GetValue(ModuleProperty);
            set => SetValue(ModuleProperty, value);
        }

        public static readonly DependencyProperty ModuleNoiseProperty =
                DependencyProperty.Register(nameof(ModuleNoise), typeof(Noise3D), typeof(ModuleNodeView), new PropertyMetadata(null));

        public Noise3D ModuleNoise
        {
            get => (Noise3D)GetValue(ModuleNoiseProperty);
            set => SetValue(ModuleNoiseProperty, value);
        }

        public ModuleNodeView()
        {
            InitializeComponent();
        }

        private void ModuleExport_Click(object sender, RoutedEventArgs e)
        {
            if (Module is null || ModuleNoise is null)
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
                LibNoiseShaderFile file = LibNoiseShaderFileWriteContext.BuildLibNoiseShaderFile(Module, ModuleNoise);
                file.Write(fileName);

                Xceed.Wpf.Toolkit.MessageBox.Show("Exported " + fileName);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ModuleViewFullHlsl_Click(object sender, RoutedEventArgs e)
        {
            if (Module is null)
            {
                return;
            }

            string hlsl = Module.GetFullHlsl();

            var dialog = new TextWindow(hlsl, "hlsl")
            {
                Owner = Application.Current.MainWindow,
                Title = "HLSL"
            };

            dialog.ShowDialog();
        }

        private void ModuleViewCS_Click(object sender, RoutedEventArgs e)
        {
            if (Module is null)
            {
                return;
            }

            string cs = Module.GetFullCSharp();

            var dialog = new TextWindow(cs, "cs")
            {
                Owner = Application.Current.MainWindow,
                Title = "CSharp"
            };

            dialog.ShowDialog();
        }

        private void ModuleViewPreview_Click(object sender, RoutedEventArgs e)
        {
            if (Module is null || ModuleNoise is null)
            {
                return;
            }

            var window = new ModuleWindow(Module, ModuleNoise)
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }

        private void ModuleView3d_Click(object sender, RoutedEventArgs e)
        {
            if (Module is null || ModuleNoise is null)
            {
                return;
            }

            var builder = new SphereBuilder(Module, ModuleNoise.Seed);
            var renderer = new ImageRenderer(builder);
            renderer.BuildTerrainGradient();

            var window = new Viewer3dWindow(renderer, ModuleNoise)
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }
    }
}
