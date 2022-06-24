using DynamicData;
using JeremyAnsel.LibNoiseShader.Modules;
using LibNoiseShaderEditor.ViewModels.ModuleSettings;
using LibNoiseShaderEditor.Views.Modules;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LibNoiseShaderEditor.ViewModels.Modules
{
    public sealed class RidgedMultiModuleViewModel : ModuleNodeViewModel<RidgedMultiModuleSetting>
    {
        static RidgedMultiModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new RidgedMultiModuleNodeView(), typeof(IViewFor<RidgedMultiModuleViewModel>));
        }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public RidgedMultiModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "RidgedMulti Module";

            var outputValue = this.BuildOutputValue()
                .Select(_ => BuildModule());

            Output = new ValueNodeOutputViewModel<IModule>
            {
                Name = "Output",
                Value = outputValue
            };

            this.Outputs.Add(Output);
        }

        private IModule BuildModule()
        {
            if (Main?.Noise is null)
            {
                return CurrentModule = null;
            }

            var module = new RidgedMultiModule(Main.Noise)
            {
                Name = Setting.Name,
                Frequency = Setting.Frequency,
                Lacunarity = Setting.Lacunarity,
                Offset = Setting.Offset,
                Gain = Setting.Gain,
                Exponent = Setting.Exponent,
                OctaveCount = Setting.OctaveCount,
                SeedOffset = Setting.SeedOffset
            };

            return CurrentModule = module;
        }
    }
}
