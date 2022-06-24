using LibNoiseShaderEditor.ViewModels.Builders;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Builders
{
    /// <summary>
    /// Logique d'interaction pour CylinderBuilderNodeView.xaml
    /// </summary>
    public partial class CylinderBuilderNodeView : UserControl, IViewFor<CylinderBuilderViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(CylinderBuilderViewModel), typeof(CylinderBuilderNodeView), new PropertyMetadata(null));

        public CylinderBuilderViewModel ViewModel
        {
            get => (CylinderBuilderViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CylinderBuilderViewModel)value;
        }

        public CylinderBuilderNodeView()
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
