namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class SelectorModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public SelectorModuleSetting()
        {
            this.EdgeFalloff = 0.0f;
            this.LowerBound = -1.0f;
            this.UpperBound = 1.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _edgeFalloff;

        public float EdgeFalloff
        {
            get => _edgeFalloff;
            set => this.RaiseAndSetIfChanged(ref _edgeFalloff, value);
        }

        private float _lowerBound;

        public float LowerBound
        {
            get => _lowerBound;
            set => this.RaiseAndSetIfChanged(ref _lowerBound, value);
        }

        private float _upperBound;

        public float UpperBound
        {
            get => _upperBound;
            set => this.RaiseAndSetIfChanged(ref _upperBound, value);
        }
    }
}
