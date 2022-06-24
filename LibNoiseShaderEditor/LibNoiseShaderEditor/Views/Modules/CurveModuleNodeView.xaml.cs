using LibNoiseShaderEditor.ViewModels.Modules;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour CurveModuleNodeView.xaml
    /// </summary>
    public partial class CurveModuleNodeView : UserControl, IViewFor<CurveModuleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(CurveModuleViewModel), typeof(CurveModuleNodeView), new PropertyMetadata(null));

        public CurveModuleViewModel ViewModel
        {
            get => (CurveModuleViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CurveModuleViewModel)value;
        }

        public CurveModuleNodeView()
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
