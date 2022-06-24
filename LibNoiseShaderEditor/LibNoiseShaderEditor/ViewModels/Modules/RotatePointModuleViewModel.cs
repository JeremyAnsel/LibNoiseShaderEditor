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
    public sealed class RotatePointModuleViewModel : ModuleNodeViewModel<RotatePointModuleSetting>
    {
        static RotatePointModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new RotatePointModuleNodeView(), typeof(IViewFor<RotatePointModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public RotatePointModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "RotatePoint Module";

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

            var module = new RotatePointModule(Input1.Value)
            {
                Name = Setting.Name
            };

            module.SetAngles(Setting.AngleX, Setting.AngleY, Setting.AngleZ);

            return CurrentModule = module;
        }
    }
}
