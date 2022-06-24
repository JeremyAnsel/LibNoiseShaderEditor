using LibNoiseShaderEditor.ViewModels.Builders;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Builders
{
    /// <summary>
    /// Logique d'interaction pour PlaneBuilderNodeView.xaml
    /// </summary>
    public partial class PlaneBuilderNodeView : UserControl, IViewFor<PlaneBuilderViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(PlaneBuilderViewModel), typeof(PlaneBuilderNodeView), new PropertyMetadata(null));

        public PlaneBuilderViewModel ViewModel
        {
            get => (PlaneBuilderViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PlaneBuilderViewModel)value;
        }

        public PlaneBuilderNodeView()
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
