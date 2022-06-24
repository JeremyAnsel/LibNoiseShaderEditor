using JeremyAnsel.LibNoiseShader.Modules;

namespace LibNoiseShaderEditor.ViewModels.ModuleSettings
{
    public sealed class PerlinModuleSetting : PropertyChangedObject, IModuleSetting
    {
        public PerlinModuleSetting()
        {
            this.Frequency = 1.0f;
            this.Lacunarity = 2.0f;
            this.OctaveCount = 6;
            this.Persistence = 0.5f;
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

        private float _persistence;

        public float Persistence
        {
            get => _persistence;
            set => this.RaiseAndSetIfChanged(ref _persistence, value);
        }

        private int _seedOffset;

        public int SeedOffset
        {
            get => _seedOffset;
            set => this.RaiseAndSetIfChanged(ref _seedOffset, value);
        }
    }
}
