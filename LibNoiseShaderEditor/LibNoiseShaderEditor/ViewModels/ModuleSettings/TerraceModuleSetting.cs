using JeremyAnsel.LibNoiseShader.Modules;
using System.Collections.Generic;

namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class TerraceModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public TerraceModuleSetting()
        {
            this.ControlPointsCount = 2;
            this.IsInverted = false;
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private List<float> _controlPoints;

        public List<float> ControlPoints
        {
            get => _controlPoints;

            set
            {
                if (value is null || value.Count < TerraceModule.MinimumControlPointsCount)
                {
                    value = new List<float>(TerraceModule.BuildControlPoints(0));
                }

                this.RaiseAndSetIfChanged(ref _controlPoints, value);
                this.RaisePropertyChanged(nameof(ControlPointsCount));
            }
        }

        public int ControlPointsCount
        {
            get => ControlPoints.Count;

            set
            {
                ControlPoints = new List<float>(TerraceModule.BuildControlPoints(value));
            }
        }

        private bool _isInverted;

        public bool IsInverted
        {
            get => _isInverted;
            set => this.RaiseAndSetIfChanged(ref _isInverted, value);
        }
    }
}
