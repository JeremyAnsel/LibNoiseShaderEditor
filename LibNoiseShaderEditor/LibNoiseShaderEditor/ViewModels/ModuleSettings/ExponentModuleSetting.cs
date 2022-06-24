namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class ExponentModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public ExponentModuleSetting()
        {
            this.ExponentValue = 1.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _exponentValue;

        public float ExponentValue
        {
            get => _exponentValue;
            set => this.RaiseAndSetIfChanged(ref _exponentValue, value);
        }
    }
}
