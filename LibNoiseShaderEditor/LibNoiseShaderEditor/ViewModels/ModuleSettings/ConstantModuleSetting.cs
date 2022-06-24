namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class ConstantModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public ConstantModuleSetting()
        {
            this.ConstantValue = 0.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _constantValue;

        public float ConstantValue
        {
            get => _constantValue;
            set => this.RaiseAndSetIfChanged(ref _constantValue, value);
        }
    }
}
