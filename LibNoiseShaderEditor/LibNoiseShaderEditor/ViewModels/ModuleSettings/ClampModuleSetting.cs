namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class ClampModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public ClampModuleSetting()
        {
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
