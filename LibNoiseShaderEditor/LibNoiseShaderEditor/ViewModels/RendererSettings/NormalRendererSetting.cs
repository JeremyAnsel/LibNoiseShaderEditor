namespace LibNoiseShaderEditor.ViewModels.RendererSettings
{
    public sealed class NormalRendererSetting : PropertyChangedObject, IRendererSetting
    {
        public NormalRendererSetting()
        {
            this.BumpHeight = 1.0f;
            this.IsWrapEnabled = false;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _bumpHeight;

        public float BumpHeight
        {
            get => _bumpHeight;
            set => this.RaiseAndSetIfChanged(ref _bumpHeight, value);
        }

        private bool _isWrapEnabled;

        public bool IsWrapEnabled
        {
            get => _isWrapEnabled;
            set => this.RaiseAndSetIfChanged(ref _isWrapEnabled, value);
        }
    }
}
