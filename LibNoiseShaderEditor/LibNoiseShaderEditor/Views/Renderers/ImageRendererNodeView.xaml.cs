using LibNoiseShaderEditor.ViewModels.Renderers;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Renderers
{
    /// <summary>
    /// Logique d'interaction pour ImageRendererNodeView.xaml
    /// </summary>
    public partial class ImageRendererNodeView : UserControl, IViewFor<ImageRendererViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(ImageRendererViewModel), typeof(ImageRendererNodeView), new PropertyMetadata(null));

        public ImageRendererViewModel ViewModel
        {
            get => (ImageRendererViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ImageRendererViewModel)value;
        }

        public ImageRendererNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                NodeView.EndpointsStackingOrientation = Orientation.Horizontal;
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.NodeView.ViewModel).DisposeWith(d);
            });
        }
    }
}
