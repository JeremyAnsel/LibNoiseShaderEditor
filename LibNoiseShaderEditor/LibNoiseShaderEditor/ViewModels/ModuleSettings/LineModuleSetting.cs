namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class LineModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public LineModuleSetting()
        {
            this.Attenuate = true;
            this.StartPointX = 0.0f;
            this.StartPointY = 0.0f;
            this.StartPointZ = 0.0f;
            this.EndPointX = 1.0f;
            this.EndPointY = 1.0f;
            this.EndPointZ = 1.0f;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private bool _attenuate;

        public bool Attenuate
        {
            get => _attenuate;
            set => this.RaiseAndSetIfChanged(ref _attenuate, value);
        }

        private float _startPointX;

        public float StartPointX
        {
            get => _startPointX;
            set => this.RaiseAndSetIfChanged(ref _startPointX, value);
        }

        private float _startPointY;

        public float StartPointY
        {
            get => _startPointY;
            set => this.RaiseAndSetIfChanged(ref _startPointY, value);
        }

        private float _startPointZ;

        public float StartPointZ
        {
            get => _startPointZ;
            set => this.RaiseAndSetIfChanged(ref _startPointZ, value);
        }

        private float _endPointX;

        public float EndPointX
        {
            get => _endPointX;
            set => this.RaiseAndSetIfChanged(ref _endPointX, value);
        }

        private float _endPointY;

        public float EndPointY
        {
            get => _endPointY;
            set => this.RaiseAndSetIfChanged(ref _endPointY, value);
        }

        private float _endPointZ;

        public float EndPointZ
        {
            get => _endPointZ;
            set => this.RaiseAndSetIfChanged(ref _endPointZ, value);
        }
    }
}
