namespace LibNoiseShaderEditor.ViewModels.BuilderSettings
{
    public sealed class SphereBuilderSetting : PropertyChangedObject, IBuilderSetting
    {
        public SphereBuilderSetting()
        {
            this.SouthLatBound = -90.0f;
            this.NorthLatBound = 90.0f;
            this.WestLonBound = -180.0f;
            this.EastLonBound = 180.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _southLatBound;

        public float SouthLatBound
        {
            get => _southLatBound;
            set => this.RaiseAndSetIfChanged(ref _southLatBound, value);
        }

        private float _northLatBound;

        public float NorthLatBound
        {
            get => _northLatBound;
            set => this.RaiseAndSetIfChanged(ref _northLatBound, value);
        }

        private float _westLonBound;

        public float WestLonBound
        {
            get => _westLonBound;
            set => this.RaiseAndSetIfChanged(ref _westLonBound, value);
        }

        private float _eastLonBound;

        public float EastLonBound
        {
            get => _eastLonBound;
            set => this.RaiseAndSetIfChanged(ref _eastLonBound, value);
        }
    }
}
