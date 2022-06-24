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
    public sealed class TurbulenceModuleViewModel : ModuleNodeViewModel<TurbulenceModuleSetting>
    {
        static TurbulenceModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new TurbulenceModuleNodeView(), typeof(IViewFor<TurbulenceModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public TurbulenceModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Turbulence Module";

            Input1 = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Input"
            };

            this.Inputs.Add(Input1);

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Input1.Value))
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
            if (Main?.Noise is null || Input1.Value is null)
            {
                return CurrentModule = null;
            }

            var module = new TurbulenceModule(Main.Noise, Input1.Value)
            {
                Name = Setting.Name,
                Frequency = Setting.Frequency,
                Power = Setting.Power,
                Roughness = Setting.Roughness,
                SeedOffset = Setting.SeedOffset
            };

            return CurrentModule = module;
        }
    }
}
