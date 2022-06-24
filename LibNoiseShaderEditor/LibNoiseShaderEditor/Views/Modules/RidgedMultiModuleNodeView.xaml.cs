using LibNoiseShaderEditor.ViewModels.Modules;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour RidgedMultiModuleNodeView.xaml
    /// </summary>
    public partial class RidgedMultiModuleNodeView : UserControl, IViewFor<RidgedMultiModuleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(RidgedMultiModuleViewModel), typeof(RidgedMultiModuleNodeView), new PropertyMetadata(null));

        public RidgedMultiModuleViewModel ViewModel
        {
            get => (RidgedMultiModuleViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (RidgedMultiModuleViewModel)value;
        }

        public RidgedMultiModuleNodeView()
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
