namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class ScaleBiasModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public ScaleBiasModuleSetting()
        {
            this.Bias = 0.0f;
            this.Scale = 1.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _bias;

        public float Bias
        {
            get => _bias;
            set => this.RaiseAndSetIfChanged(ref _bias, value);
        }

        private float _scale;

        public float Scale
        {
            get => _scale;
            set => this.RaiseAndSetIfChanged(ref _scale, value);
        }
    }
}
