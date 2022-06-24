namespace LibNoiseShaderEditor.ViewModels.RendererSettings
{
    public sealed class BlendRendererSetting : PropertyChangedObject, IRendererSetting
    {
        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }
}
