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
    public sealed class CheckerboardModuleViewModel : ModuleNodeViewModel<CheckerboardModuleSetting>
    {
        static CheckerboardModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CheckerboardModuleNodeView(), typeof(IViewFor<CheckerboardModuleViewModel>));
        }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public CheckerboardModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Checkerboard Module";

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

            var module = new CheckerboardModule
            {
                Name = Setting.Name
            };

            return CurrentModule = module;
        }
    }
}
