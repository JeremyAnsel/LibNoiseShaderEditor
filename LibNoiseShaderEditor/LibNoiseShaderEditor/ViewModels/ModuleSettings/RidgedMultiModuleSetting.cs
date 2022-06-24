using JeremyAnsel.LibNoiseShader.Modules;

namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class RidgedMultiModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public RidgedMultiModuleSetting()
        {
            this.Frequency = 1.0f;
            this.Lacunarity = 2.0f;
            this.Offset = 1.0f;
            this.Gain = 2.0f;
            this.Exponent = 1.0f;
            this.OctaveCount = 6;
            this.SeedOffset = 0;
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

        private float _lacunarity;

        public float Lacunarity
        {
            get => _lacunarity;
            set => this.RaiseAndSetIfChanged(ref _lacunarity, value);
        }

        private float _offset;

        public float Offset
        {
            get => _offset;
            set => this.RaiseAndSetIfChanged(ref _offset, value);
        }

        private float _gain;

        public float Gain
        {
            get => _gain;
            set => this.RaiseAndSetIfChanged(ref _gain, value);
        }

        private float _exponent;

        public float Exponent
        {
            get => _exponent;
            set => this.RaiseAndSetIfChanged(ref _exponent, value);
        }

        private int _octaveCount;

        public int OctaveCount
        {
            get => _octaveCount;

            set
            {
                if (value < PerlinModule.MinOctave)
                {
                    value = PerlinModule.MinOctave;
                }
                else if (value > PerlinModule.MaxOctave)
                {
                    value = PerlinModule.MaxOctave;
                }

                this.RaiseAndSetIfChanged(ref _octaveCount, value);
            }
        }

        private int _seedOffset;

        public int SeedOffset
        {
            get => _seedOffset;
            set => this.RaiseAndSetIfChanged(ref _seedOffset, value);
        }
    }
}
