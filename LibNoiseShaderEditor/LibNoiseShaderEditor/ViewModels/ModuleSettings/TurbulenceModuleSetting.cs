namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class TurbulenceModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public TurbulenceModuleSetting()
        {
            this.Frequency = 1.0f;
            this.Power = 1.0f;
            this.Roughness = 3;
            this.SeedOffset = 0;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _frequency;

        public float Frequency
        {
            get => _frequency;
            set => this.RaiseAndSetIfChanged(ref _frequency, value);
        }

        private float _power;

        public float Power
        {
            get => _power;
            set => this.RaiseAndSetIfChanged(ref _power, value);
        }

        private int _roughness;

        public int Roughness
        {
            get => _roughness;
            set => this.RaiseAndSetIfChanged(ref _roughness, value);
        }

        private int _seedOffset;

        public int SeedOffset
        {
            get => _seedOffset;
            set => this.RaiseAndSetIfChanged(ref _seedOffset, value);
        }
    }
}
