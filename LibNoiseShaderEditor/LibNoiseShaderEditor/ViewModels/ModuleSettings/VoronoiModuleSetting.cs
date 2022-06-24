namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class VoronoiModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public VoronoiModuleSetting()
        {
            this.IsDistanceApplied = false;
            this.Displacement = 1.0f;
            this.Frequency = 1.0f;
            this.SeedOffset = 0;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private bool _isDistanceApplied;

        public bool IsDistanceApplied
        {
            get => _isDistanceApplied;
            set => this.RaiseAndSetIfChanged(ref _isDistanceApplied, value);
        }

        private float _displacement;

        public float Displacement
        {
            get => _displacement;
            set => this.RaiseAndSetIfChanged(ref _displacement, value);
        }

        private float _frequency;

        public float Frequency
        {
            get => _frequency;
            set => this.RaiseAndSetIfChanged(ref _frequency, value);
        }

        private int _seedOffset;

        public int SeedOffset
        {
            get => _seedOffset;
            set => this.RaiseAndSetIfChanged(ref _seedOffset, value);
        }
    }
}
