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
    public sealed class SphereModuleViewModel : ModuleNodeViewModel<SphereModuleSetting>
    {
        static SphereModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new SphereModuleNodeView(), typeof(IViewFor<SphereModuleViewModel>));
        }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public SphereModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Sphere Module";

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

            var module = new SphereModule
            {
                Name = Setting.Name,
                Frequency = Setting.Frequency
            };

            return CurrentModule = module;
        }
    }
}
