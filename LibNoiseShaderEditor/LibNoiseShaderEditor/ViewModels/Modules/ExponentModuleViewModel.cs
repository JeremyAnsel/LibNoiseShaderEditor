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
    public sealed class ExponentModuleViewModel : ModuleNodeViewModel<ExponentModuleSetting>
    {
        static ExponentModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ExponentModuleNodeView(), typeof(IViewFor<ExponentModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public ExponentModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Exponent Module";

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

            var module = new ExponentModule(Input1.Value)
            {
                Name = Setting.Name,
                ExponentValue = Setting.ExponentValue
            };

            return CurrentModule = module;
        }
    }
}
