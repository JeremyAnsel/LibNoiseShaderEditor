using LibNoiseShaderEditor.ViewModels.Renderers;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Renderers
{
    /// <summary>
    /// Logique d'interaction pour NormalRendererNodeView.xaml
    /// </summary>
    public partial class NormalRendererNodeView : UserControl, IViewFor<NormalRendererViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(NormalRendererViewModel), typeof(NormalRendererNodeView), new PropertyMetadata(null));

        public NormalRendererViewModel ViewModel
        {
            get => (NormalRendererViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (NormalRendererViewModel)value;
        }

        public NormalRendererNodeView()
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
