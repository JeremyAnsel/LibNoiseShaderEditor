namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class ScalePointModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public ScalePointModuleSetting()
        {
            this.ScaleX = 1.0f;
            this.ScaleY = 1.0f;
            this.ScaleZ = 1.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _scaleX;

        public float ScaleX
        {
            get => _scaleX;
            set => this.RaiseAndSetIfChanged(ref _scaleX, value);
        }

        private float _scaleY;

        public float ScaleY
        {
            get => _scaleY;
            set => this.RaiseAndSetIfChanged(ref _scaleY, value);
        }

        private float _scaleZ;

        public float ScaleZ
        {
            get => _scaleZ;
            set => this.RaiseAndSetIfChanged(ref _scaleZ, value);
        }
    }
}
