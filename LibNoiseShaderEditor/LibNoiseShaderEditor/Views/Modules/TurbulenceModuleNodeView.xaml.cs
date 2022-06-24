using LibNoiseShaderEditor.ViewModels.Modules;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour TurbulenceModuleNodeView.xaml
    /// </summary>
    public partial class TurbulenceModuleNodeView : UserControl, IViewFor<TurbulenceModuleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(TurbulenceModuleViewModel), typeof(TurbulenceModuleNodeView), new PropertyMetadata(null));

        public TurbulenceModuleViewModel ViewModel
        {
            get => (TurbulenceModuleViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TurbulenceModuleViewModel)value;
        }

        public TurbulenceModuleNodeView()
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
