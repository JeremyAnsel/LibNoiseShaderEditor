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
    public sealed class ConstantModuleViewModel : ModuleNodeViewModel<ConstantModuleSetting>
    {
        static ConstantModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ConstantModuleNodeView(), typeof(IViewFor<ConstantModuleViewModel>));
        }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public ConstantModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Constant Module";

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

            var module = new ConstantModule
            {
                Name = Setting.Name,
                ConstantValue = Setting.ConstantValue
            };

            return CurrentModule = module;
        }
    }
}
