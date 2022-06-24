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
    public sealed class PowerModuleViewModel : ModuleNodeViewModel<PowerModuleSetting>
    {
        static PowerModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new PowerModuleNodeView(), typeof(IViewFor<PowerModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeInputViewModel<IModule> Input2 { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public PowerModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Power Module";

            Input1 = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Input 1"
            };

            this.Inputs.Add(Input1);

            Input2 = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Input 2"
            };

            this.Inputs.Add(Input2);

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Input1.Value,
                    vm => vm.Input2.Value))
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
            if (Main?.Noise is null || Input1.Value is null || Input2.Value is null)
            {
                return CurrentModule = null;
            }

            var module = new PowerModule(Input1.Value, Input2.Value)
            {
                Name = Setting.Name
            };

            return CurrentModule = module;
        }
    }
}
