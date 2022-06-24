using JeremyAnsel.LibNoiseShader.IO;
using LibNoiseShaderEditor.Helpers;
using LibNoiseShaderEditor.Models;
using LibNoiseShaderEditor.ViewModels;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace LibNoiseShaderEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        private bool _initLayout = false;

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(MainViewModel), typeof(MainWindow), new PropertyMetadata(null));

        public MainViewModel ViewModel
        {
            get => (MainViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }

        public MainWindow()
        {
            InitializeComponent();

            this.ViewModel = new MainViewModel();
            this.DataContext = this.ViewModel;

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.ListViewModel, v => v.nodeList.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.NetworkViewModel, v => v.viewHost.ViewModel).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.NetworkViewModel.ZoomFactor, v => v.zoomFactorSlider.Value).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.NetworkViewModel.MaxZoomLevel, v => v.zoomFactorSlider.Maximum).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.NetworkViewModel.MinZoomLevel, v => v.zoomFactorSlider.Minimum).DisposeWith(d);
            });

            viewHost.Events()
                .LayoutUpdated
                .Where(_ => viewHost.ViewModel != null && viewHost.ViewModel.Nodes.Count > 0)
                .Select(_ => viewHost.ViewModel.Nodes.Items.Last().Size)
                .Where(size => size.Width > 0 && size.Height > 0)
                .Subscribe(size =>
                {
                    if (_initLayout)
                    {
                        _initLayout = false;
                        ViewModel.GenerateLayout();
                        viewHost.CenterAndZoomView();
                    }
                });
        }

        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            viewHost.CenterAndZoomView();
        }

        private void GenerateRandomSeed_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GlobalSeed = new Random().Next();
        }

        private void NewNetwork_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearNetwork();
        }

        private void OpenNetwork_Click(object sender, RoutedEventArgs e)
        {
            string directory = System.IO.Path.GetDirectoryName(ViewModel.CurrentFileName);
            string fileName = FileDialogHelpers.GetOpenFileName(this, GlobalConstants.LibNoiseShaderDefaultExtension, directory);

            if (fileName is null)
            {
                return;
            }

            try
            {
                ViewModel.ClearNetwork();

                LibNoiseShaderFile file = LibNoiseShaderFile.Load(fileName);
                ViewModel.GlobalSeed = file.NoiseSeed;
                EditorLoadContext.LoadLibNoiseShaderFile(file, ViewModel);

                if (!file.HasPositions)
                {
                    _initLayout = true;
                    viewHost.InvalidateVisual();
                }

                viewHost.CenterAndZoomView();
                ViewModel.CurrentFileName = fileName;
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveNetwork_Click(object sender, RoutedEventArgs e)
        {
            if (ShowErrorMessage())
            {
                return;
            }

            if (string.IsNullOrEmpty(ViewModel.CurrentFileName))
            {
                SaveAsNetwork_Click(null, null);
                return;
            }

            SaveNetwork(ViewModel.CurrentFileName);
        }

        private void SaveAsNetwork_Click(object sender, RoutedEventArgs e)
        {
            if (ShowErrorMessage())
            {
                return;
            }

            string directory = System.IO.Path.GetDirectoryName(ViewModel.CurrentFileName);
            string fileName = FileDialogHelpers.GetSaveFileName(this, GlobalConstants.LibNoiseShaderDefaultExtension, directory);

            if (fileName is null)
            {
                return;
            }

            SaveNetwork(fileName);
        }

        private void SaveNetwork(string fileName)
        {
            try
            {
                LibNoiseShaderFile file = EditorWriteContext.BuildLibNoiseShaderFile(ViewModel.Noise, ViewModel.NetworkViewModel);
                file.Write(fileName);

                ViewModel.CurrentFileName = fileName;
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportNetwork_Click(object sender, RoutedEventArgs e)
        {
            if (ShowErrorMessage())
            {
                return;
            }

            string directory = System.IO.Path.GetDirectoryName(ViewModel.CurrentFileName);
            string fileName = FileDialogHelpers.GetOpenFileName(this, GlobalConstants.LibNoiseShaderDefaultExtension, directory);

            if (fileName is null)
            {
                return;
            }

            try
            {
                LibNoiseShaderFile file = LibNoiseShaderFile.Load(fileName);
                EditorLoadContext.ImportLibNoiseShaderFile(file, ViewModel);

                //if (!file.HasPositions)
                //{
                //    _initLayout = true;
                //    viewHost.InvalidateVisual();
                //}

                _initLayout = true;
                viewHost.InvalidateVisual();
                viewHost.CenterAndZoomView();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString(), GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowErrorMessage())
            {
                return;
            }

            ViewModel.GenerateLayout();
            viewHost.CenterAndZoomView();
        }

        private bool ShowErrorMessage()
        {
            if (ViewModel.NetworkViewModel.LatestValidation is not null && !ViewModel.NetworkViewModel.LatestValidation.IsValid)
            {
                if (ViewModel.NetworkViewModel.LatestValidation.MessageViewModel is ErrorMessageViewModel message)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(message.Message, GlobalConstants.XceedMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }

            return false;
        }
    }
}
