namespace LibNoiseShaderEditor.ViewModels.BuilderSettings
{
    public sealed class CylinderBuilderSetting : PropertyChangedObject, IBuilderSetting
    {
        public CylinderBuilderSetting()
        {
            this.LowerAngleBound = -180f;
            this.UpperAngleBound = 180f;
            this.LowerHeightBound = -1.0f;
            this.UpperHeightBound = 1.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _lowerAngleBound;

        public float LowerAngleBound
        {
            get => _lowerAngleBound;
            set => this.RaiseAndSetIfChanged(ref _lowerAngleBound, value);
        }

        private float _lowerHeightBound;

        public float LowerHeightBound
        {
            get => _lowerHeightBound;
            set => this.RaiseAndSetIfChanged(ref _lowerHeightBound, value);
        }

        private float _upperAngleBound;

        public float UpperAngleBound
        {
            get => _upperAngleBound;
            set => this.RaiseAndSetIfChanged(ref _upperAngleBound, value);
        }

        private float _upperHeightBound;

        public float UpperHeightBound
        {
            get => _upperHeightBound;
            set => this.RaiseAndSetIfChanged(ref _upperHeightBound, value);
        }
    }
}
