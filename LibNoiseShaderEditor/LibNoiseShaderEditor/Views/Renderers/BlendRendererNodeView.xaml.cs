using LibNoiseShaderEditor.ViewModels.Renderers;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Renderers
{
    /// <summary>
    /// Logique d'interaction pour BlendRendererNodeView.xaml
    /// </summary>
    public partial class BlendRendererNodeView : UserControl, IViewFor<BlendRendererViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(BlendRendererViewModel), typeof(BlendRendererNodeView), new PropertyMetadata(null));

        public BlendRendererViewModel ViewModel
        {
            get => (BlendRendererViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (BlendRendererViewModel)value;
        }

        public BlendRendererNodeView()
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
