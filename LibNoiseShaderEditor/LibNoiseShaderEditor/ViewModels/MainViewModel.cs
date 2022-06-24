using DynamicData;
using JeremyAnsel.LibNoiseShader;
using LibNoiseShaderEditor.Models;
using LibNoiseShaderEditor.ViewModels.Builders;
using LibNoiseShaderEditor.ViewModels.Modules;
using LibNoiseShaderEditor.ViewModels.Renderers;
using NodeNetwork;
using NodeNetwork.Toolkit;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System.Linq;

namespace LibNoiseShaderEditor.ViewModels
{
    public sealed class MainViewModel : ReactiveObject
    {
        static MainViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new MainWindow(), typeof(IViewFor<MainViewModel>));
        }

        public NodeListViewModel ListViewModel { get; } = new NodeListViewModel();

        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();

        private string _currentFileName;

        public string CurrentFileName
        {
            get => _currentFileName;
            set => this.RaiseAndSetIfChanged(ref _currentFileName, value);
        }

        private int _globalSeed;

        public int GlobalSeed
        {
            get => _globalSeed;

            set
            {
                this.RaiseAndSetIfChanged(ref _globalSeed, value);
                Noise = new Noise3D(value);
            }
        }

        private Noise3D _noise;

        public Noise3D Noise
        {
            get => _noise;
            set => this.RaiseAndSetIfChanged(ref _noise, value);
        }

        public MainViewModel()
        {
            SetDefaultValues();
            SetListViewModes();
            SetNetworkValidator();
        }

        private void SetDefaultValues()
        {
            GlobalSeed = 0;

            ListViewModel.Display = NodeListViewModel.DisplayMode.List;

            NetworkViewModel.MinZoomLevel = GlobalConstants.NetworkMinZoomLevel;
            NetworkViewModel.MaxZoomLevel = GlobalConstants.NetworkMaxZoomLevel;

        }

        private void SetListViewModes()
        {
            SetListViewRenderersModes();
            SetListViewBuildersModes();
            SetListViewModulesModes();
        }

        private void SetListViewModulesModes()
        {
            ListViewModel.AddNodeType(() => new AbsModuleViewModel(this));
            ListViewModel.AddNodeType(() => new AddModuleViewModel(this));
            ListViewModel.AddNodeType(() => new BillowModuleViewModel(this));
            ListViewModel.AddNodeType(() => new BlendModuleViewModel(this));
            ListViewModel.AddNodeType(() => new CacheModuleViewModel(this));
            ListViewModel.AddNodeType(() => new CheckerboardModuleViewModel(this));
            ListViewModel.AddNodeType(() => new ClampModuleViewModel(this));
            ListViewModel.AddNodeType(() => new ConstantModuleViewModel(this));
            ListViewModel.AddNodeType(() => new CurveModuleViewModel(this));
            ListViewModel.AddNodeType(() => new CylinderModuleViewModel(this));
            ListViewModel.AddNodeType(() => new DisplaceModuleViewModel(this));
            ListViewModel.AddNodeType(() => new ExponentModuleViewModel(this));
            ListViewModel.AddNodeType(() => new InvertModuleViewModel(this));
            ListViewModel.AddNodeType(() => new LineModuleViewModel(this));
            ListViewModel.AddNodeType(() => new MaxModuleViewModel(this));
            ListViewModel.AddNodeType(() => new MinModuleViewModel(this));
            ListViewModel.AddNodeType(() => new MultiplyModuleViewModel(this));
            ListViewModel.AddNodeType(() => new PerlinModuleViewModel(this));
            ListViewModel.AddNodeType(() => new PowerModuleViewModel(this));
            ListViewModel.AddNodeType(() => new RidgedMultiModuleViewModel(this));
            ListViewModel.AddNodeType(() => new RotatePointModuleViewModel(this));
            ListViewModel.AddNodeType(() => new ScaleBiasModuleViewModel(this));
            ListViewModel.AddNodeType(() => new ScalePointModuleViewModel(this));
            ListViewModel.AddNodeType(() => new SelectorModuleViewModel(this));
            ListViewModel.AddNodeType(() => new SphereModuleViewModel(this));
            ListViewModel.AddNodeType(() => new TerraceModuleViewModel(this));
            ListViewModel.AddNodeType(() => new TranslatePointModuleViewModel(this));
            ListViewModel.AddNodeType(() => new TurbulenceModuleViewModel(this));
            ListViewModel.AddNodeType(() => new VoronoiModuleViewModel(this));
        }

        private void SetListViewBuildersModes()
        {
            ListViewModel.AddNodeType(() => new CylinderBuilderViewModel(this));
            ListViewModel.AddNodeType(() => new PlaneBuilderViewModel(this));
            ListViewModel.AddNodeType(() => new SphereBuilderViewModel(this));
        }

        private void SetListViewRenderersModes()
        {
            ListViewModel.AddNodeType(() => new BlendRendererViewModel(this));
            ListViewModel.AddNodeType(() => new ImageRendererViewModel(this));
            ListViewModel.AddNodeType(() => new NormalRendererViewModel(this));
        }

        private void SetNetworkValidator()
        {
            NetworkViewModel.Validator = network =>
            {
                bool containsLoops = GraphAlgorithms.FindLoops(network).Any();

                if (containsLoops)
                {
                    return new NetworkValidationResult(false, false, new ErrorMessageViewModel("Network contains loops!"));
                }

                return new NetworkValidationResult(true, true, null);
            };
        }

        public void ClearNetwork()
        {
            CurrentFileName = null;
            NetworkViewModel.ClearSelection();
            NetworkViewModel.Connections.Clear();
            NetworkViewModel.Nodes.Clear();
            GlobalSeed = 0;
        }

        public void GenerateLayout()
        {
            NodesLayoutContext.BuildLayout(NetworkViewModel);
        }
    }
}
