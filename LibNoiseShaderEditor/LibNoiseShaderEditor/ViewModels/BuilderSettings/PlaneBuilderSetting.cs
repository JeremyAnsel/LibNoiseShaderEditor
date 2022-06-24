namespace LibNoiseShaderEditor.ViewModels.BuilderSettings
{
    public sealed class PlaneBuilderSetting : PropertyChangedObject, IBuilderSetting
    {
        public PlaneBuilderSetting()
        {
            this.IsSeamless = false;
            this.LowerBoundX = -1.0f;
            this.UpperBoundX = 1.0f;
            this.LowerBoundY = -1.0f;
            this.UpperBoundY = 1.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private bool _isSeamless;

        public bool IsSeamless
        {
            get => _isSeamless;
            set => this.RaiseAndSetIfChanged(ref _isSeamless, value);
        }

        private float _lowerBoundX;

        public float LowerBoundX
        {
            get => _lowerBoundX;
            set => this.RaiseAndSetIfChanged(ref _lowerBoundX, value);
        }

        private float _upperBoundX;

        public float UpperBoundX
        {
            get => _upperBoundX;
            set => this.RaiseAndSetIfChanged(ref _upperBoundX, value);
        }

        private float _lowerBoundY;

        public float LowerBoundY
        {
            get => _lowerBoundY;
            set => this.RaiseAndSetIfChanged(ref _lowerBoundY, value);
        }

        private float _upperBoundY;

        public float UpperBoundY
        {
            get => _upperBoundY;
            set => this.RaiseAndSetIfChanged(ref _upperBoundY, value);
        }
    }
}
