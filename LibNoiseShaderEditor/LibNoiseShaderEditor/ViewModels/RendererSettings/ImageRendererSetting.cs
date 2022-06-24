using JeremyAnsel.LibNoiseShader.Renderers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace LibNoiseShaderEditor.ViewModels.RendererSettings
{
    public sealed class ImageRendererSetting : PropertyChangedObject, IRendererSetting
    {
        public ImageRendererSetting()
        {
            this.IsWrapEnabled = false;
            this.IsLightEnabled = false;
            this.LightAzimuth = 45.0f;
            this.LightBrightness = 1.0f;
            this.LightColor = Colors.White;
            this.LightContrast = 1.0f;
            this.LightElevation = 45.0f;
            this.LightIntensity = 1.0f;

            this.GradientPoints = ImageRenderer
                .GetDefaultTerrainGradient()
                .ToDictionary(t => t.Key, t => Color.FromArgb(t.Value.A, t.Value.R, t.Value.G, t.Value.B));
        }

        private string _name;

        [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.PropertyOrder(0)]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private bool _isWrapEnabled;

        public bool IsWrapEnabled
        {
            get => _isWrapEnabled;
            set => this.RaiseAndSetIfChanged(ref _isWrapEnabled, value);
        }

        private bool _isLightEnabled;

        public bool IsLightEnabled
        {
            get => _isLightEnabled;
            set => this.RaiseAndSetIfChanged(ref _isLightEnabled, value);
        }

        private float _lightAzimuth;

        public float LightAzimuth
        {
            get => _lightAzimuth;
            set => this.RaiseAndSetIfChanged(ref _lightAzimuth, value);
        }

        private float _lightBrightness;

        public float LightBrightness
        {
            get => _lightBrightness;
            set => this.RaiseAndSetIfChanged(ref _lightBrightness, value);
        }

        private Color _lightColor;

        public Color LightColor
        {
            get => _lightColor;
            set => this.RaiseAndSetIfChanged(ref _lightColor, value);
        }

        private float _lightContrast;

        public float LightContrast
        {
            get => _lightContrast;
            set => this.RaiseAndSetIfChanged(ref _lightContrast, value);
        }

        private float _lightElevation;

        public float LightElevation
        {
            get => _lightElevation;
            set => this.RaiseAndSetIfChanged(ref _lightElevation, value);
        }

        private float _lightIntensity;

        public float LightIntensity
        {
            get => _lightIntensity;
            set => this.RaiseAndSetIfChanged(ref _lightIntensity, value);
        }

        private Dictionary<float, Color> _gradientPoints;

        public Dictionary<float, Color> GradientPoints
        {
            get => _gradientPoints;

            set
            {
                if (value is null || value.Count < 2)
                {
                    value = ImageRenderer
                        .GetDefaultGrayscaleGradient()
                        .ToDictionary(t => t.Key, t => Color.FromArgb(t.Value.A, t.Value.R, t.Value.G, t.Value.B));
                }

                this.RaiseAndSetIfChanged(ref _gradientPoints, value);
            }
        }
    }
}
