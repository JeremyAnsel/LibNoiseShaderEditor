using LibNoiseShaderEditor.ViewModels.Modules;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour AbsModuleNodeView.xaml
    /// </summary>
    public partial class AbsModuleNodeView : UserControl, IViewFor<AbsModuleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(AbsModuleViewModel), typeof(AbsModuleNodeView), new PropertyMetadata(null));

        public AbsModuleViewModel ViewModel
        {
            get => (AbsModuleViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AbsModuleViewModel)value;
        }

        public AbsModuleNodeView()
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
