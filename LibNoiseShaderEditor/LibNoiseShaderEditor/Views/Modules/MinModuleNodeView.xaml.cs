using LibNoiseShaderEditor.ViewModels.Modules;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace LibNoiseShaderEditor.Views.Modules
{
    /// <summary>
    /// Logique d'interaction pour MinModuleNodeView.xaml
    /// </summary>
    public partial class MinModuleNodeView : UserControl, IViewFor<MinModuleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register(nameof(ViewModel), typeof(MinModuleViewModel), typeof(MinModuleNodeView), new PropertyMetadata(null));

        public MinModuleViewModel ViewModel
        {
            get => (MinModuleViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MinModuleViewModel)value;
        }

        public MinModuleNodeView()
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
