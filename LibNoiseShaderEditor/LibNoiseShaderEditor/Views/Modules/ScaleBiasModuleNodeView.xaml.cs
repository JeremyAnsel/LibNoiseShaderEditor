using LibNoiseShaderEditor.ViewModels.Modules;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour ScaleBiasModuleNodeView.xaml
    /// </summary>
    public partial class ScaleBiasModuleNodeView : UserControl, IViewFor<ScaleBiasModuleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(ScaleBiasModuleViewModel), typeof(ScaleBiasModuleNodeView), new PropertyMetadata(null));

        public ScaleBiasModuleViewModel ViewModel
        {
            get => (ScaleBiasModuleViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ScaleBiasModuleViewModel)value;
        }

        public ScaleBiasModuleNodeView()
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
