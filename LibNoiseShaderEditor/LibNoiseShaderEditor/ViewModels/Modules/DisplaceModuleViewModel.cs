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
    public sealed class DisplaceModuleViewModel : ModuleNodeViewModel<DisplaceModuleSetting>
    {
        static DisplaceModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new DisplaceModuleNodeView(), typeof(IViewFor<DisplaceModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeInputViewModel<IModule> DisplaceX { get; }

        public ValueNodeInputViewModel<IModule> DisplaceY { get; }

        public ValueNodeInputViewModel<IModule> DisplaceZ { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public DisplaceModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Displace Module";

            Input1 = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Input"
            };

            this.Inputs.Add(Input1);

            DisplaceX = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Displace X"
            };

            this.Inputs.Add(DisplaceX);

            DisplaceY = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Displace Y"
            };

            this.Inputs.Add(DisplaceY);

            DisplaceZ = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Displace Z"
            };

            this.Inputs.Add(DisplaceZ);

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Input1.Value,
                    vm => vm.DisplaceX.Value,
                    vm => vm.DisplaceY.Value,
                    vm => vm.DisplaceZ.Value))
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
            if (Main?.Noise is null || Input1.Value is null || DisplaceX.Value is null || DisplaceY.Value is null || DisplaceZ.Value is null)
            {
                return CurrentModule = null;
            }

            var module = new DisplaceModule(Input1.Value, DisplaceX.Value, DisplaceY.Value, DisplaceZ.Value)
            {
                Name = Setting.Name
            };

            return CurrentModule = module;
        }
    }
}
