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
    public sealed class CylinderModuleViewModel : ModuleNodeViewModel<CylinderModuleSetting>
    {
        static CylinderModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CylinderModuleNodeView(), typeof(IViewFor<CylinderModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public CylinderModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Cylinder Module";

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

            var module = new CylinderModule
            {
                Name = Setting.Name,
                Frequency = Setting.Frequency
            };

            return CurrentModule = module;
        }
    }
}
