namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class CylinderModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public CylinderModuleSetting()
        {
            this.Frequency = 1.0f;
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
    }
}
