namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class RotatePointModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public RotatePointModuleSetting()
        {
            this.AngleX = 0.0f;
            this.AngleY = 0.0f;
            this.AngleZ = 0.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _angleX;

        public float AngleX
        {
            get => _angleX;
            set => this.RaiseAndSetIfChanged(ref _angleX, value);
        }

        private float _angleY;

        public float AngleY
        {
            get => _angleY;
            set => this.RaiseAndSetIfChanged(ref _angleY, value);
        }

        private float _angleZ;

        public float AngleZ
        {
            get => _angleZ;
            set => this.RaiseAndSetIfChanged(ref _angleZ, value);
        }
    }
}
