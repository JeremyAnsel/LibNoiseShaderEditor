using LibNoiseShaderEditor.ViewModels.Modules;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour SelectorModuleNodeView.xaml
    /// </summary>
    public partial class SelectorModuleNodeView : UserControl, IViewFor<SelectorModuleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(SelectorModuleViewModel), typeof(SelectorModuleNodeView), new PropertyMetadata(null));

        public SelectorModuleViewModel ViewModel
        {
            get => (SelectorModuleViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (SelectorModuleViewModel)value;
        }

        public SelectorModuleNodeView()
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
