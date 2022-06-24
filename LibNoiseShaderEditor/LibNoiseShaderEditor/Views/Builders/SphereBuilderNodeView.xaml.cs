using LibNoiseShaderEditor.ViewModels.Builders;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Builders
{
    /// <summary>
    /// Logique d'interaction pour SphereBuilderNodeView.xaml
    /// </summary>
    public partial class SphereBuilderNodeView : UserControl, IViewFor<SphereBuilderViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(SphereBuilderViewModel), typeof(SphereBuilderNodeView), new PropertyMetadata(null));

        public SphereBuilderViewModel ViewModel
        {
            get => (SphereBuilderViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (SphereBuilderViewModel)value;
        }

        public SphereBuilderNodeView()
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
