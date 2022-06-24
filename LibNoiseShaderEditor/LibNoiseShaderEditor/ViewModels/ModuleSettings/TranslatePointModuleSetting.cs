namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class TranslatePointModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public TranslatePointModuleSetting()
        {
            this.TranslateX = 0.0f;
            this.TranslateY = 0.0f;
            this.TranslateZ = 0.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _translateX;

        public float TranslateX
        {
            get => _translateX;
            set => this.RaiseAndSetIfChanged(ref _translateX, value);
        }

        private float _translateY;

        public float TranslateY
        {
            get => _translateY;
            set => this.RaiseAndSetIfChanged(ref _translateY, value);
        }

        private float _translateZ;

        public float TranslateZ
        {
            get => _translateZ;
            set => this.RaiseAndSetIfChanged(ref _translateZ, value);
        }
    }
}
