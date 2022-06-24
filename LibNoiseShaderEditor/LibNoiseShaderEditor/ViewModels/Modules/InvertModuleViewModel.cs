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
    public sealed class InvertModuleViewModel : ModuleNodeViewModel<InvertModuleSetting>
    {
        static InvertModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new InvertModuleNodeView(), typeof(IViewFor<InvertModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public InvertModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Invert Module";

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

            var module = new InvertModule(Input1.Value)
            {
                Name = Setting.Name
            };

            return CurrentModule = module;
        }
    }
}
