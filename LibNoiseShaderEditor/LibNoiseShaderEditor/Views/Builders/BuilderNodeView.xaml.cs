using JeremyAnsel.LibNoiseShader;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.IO;
using JeremyAnsel.LibNoiseShader.IO.Models;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.Helpers;
using LibNoiseShaderEditor.ViewModels.BuilderSettings;
using LibNoiseShaderEditor.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Builders
{
    /// <summary>
    /// Logique d'interaction pour BuilderNodeView.xaml
    /// </summary>
    public partial class BuilderNodeView : UserControl
    {
        public static readonly DependencyProperty SettingProperty =
                DependencyProperty.Register(nameof(Setting), typeof(IBuilderSetting), typeof(BuilderNodeView), new PropertyMetadata(null));

        public IBuilderSetting Setting
        {
            get => (IBuilderSetting)GetValue(SettingProperty);
            set => SetValue(SettingProperty, value);
        }

        public static readonly DependencyProperty BuilderProperty =
                DependencyProperty.Register(nameof(Builder), typeof(IBuilder), typeof(BuilderNodeView), new PropertyMetadata(null));

        public IBuilder Builder
        {
            get => (IBuilder)GetValue(BuilderProperty);
            set => SetValue(BuilderProperty, value);
        }

        public static readonly DependencyProperty ModuleNoiseProperty =
                DependencyProperty.Register(nameof(ModuleNoise), typeof(Noise3D), typeof(BuilderNodeView), new PropertyMetadata(null));

        public Noise3D ModuleNoise
        {
            get => (Noise3D)GetValue(ModuleNoiseProperty);
            set => SetValue(ModuleNoiseProperty, value);
        }

        public BuilderNodeView()
        {
            InitializeComponent();
        }

        private void BuilderExport_Click(object sender, RoutedEventArgs e)
        {
            if (Builder is null || ModuleNoise is null)
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
                LibNoiseShaderFile file = LibNoiseShaderFileWriteContext.BuildLibNoiseShaderFile(Builder, ModuleNoise);
                file.Write(fileName);

                Xceed.Wpf.Toolkit.MessageBox.Show("Exported " + fileName);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BuilderViewFullHlsl_Click(object sender, RoutedEventArgs e)
        {
            if (Builder is null)
            {
                return;
            }

            string hlsl = Builder.GetFullHlsl();

            var dialog = new TextWindow(hlsl, "hlsl")
            {
                Owner = Application.Current.MainWindow,
                Title = "HLSL"
            };

            dialog.ShowDialog();
        }

        private void BuilderViewCS_Click(object sender, RoutedEventArgs e)
        {
            if (Builder is null)
            {
                return;
            }

            string cs = Builder.GetFullCSharp();

            var dialog = new TextWindow(cs, "cs")
            {
                Owner = Application.Current.MainWindow,
                Title = "CSharp"
            };

            dialog.ShowDialog();
        }

        private void BuilderViewPreview_Click(object sender, RoutedEventArgs e)
        {
            if (Builder is null || ModuleNoise is null)
            {
                return;
            }

            var window = new BuilderWindow(Builder, ModuleNoise)
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }

        private void BuilderView3d_Click(object sender, RoutedEventArgs e)
        {
            if (Builder is null || ModuleNoise is null)
            {
                return;
            }

            var renderer = new ImageRenderer(Builder);
            renderer.BuildTerrainGradient();

            var window = new Viewer3dWindow(renderer, ModuleNoise)
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }
    }
}
