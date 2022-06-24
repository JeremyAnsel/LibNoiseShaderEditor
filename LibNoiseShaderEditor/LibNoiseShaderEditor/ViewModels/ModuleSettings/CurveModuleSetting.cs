using System.Collections.Generic;

namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class CurveModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public CurveModuleSetting()
        {
            ControlPoints = new Dictionary<float, float>
            {
                { -1.0f, -1.0f },
                { -0.5f, -0.5f },
                { 0.0f, 0.0f },
                { 0.5f, 0.5f },
                { 1.0f, 1.0f }
            };

        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private Dictionary<float, float> _controlPoints;

        public Dictionary<float, float> ControlPoints
        {
            get => _controlPoints;
            set => this.RaiseAndSetIfChanged(ref _controlPoints, value);
        }
    }
}
