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
    public sealed class BillowModuleViewModel : ModuleNodeViewModel<BillowModuleSetting>
    {
        static BillowModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new BillowModuleNodeView(), typeof(IViewFor<BillowModuleViewModel>));
        }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public BillowModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Billow Module";

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

            var module = new BillowModule(Main.Noise)
            {
                Name = Setting.Name,
                Frequency = Setting.Frequency,
                Lacunarity = Setting.Lacunarity,
                OctaveCount = Setting.OctaveCount,
                Persistence = Setting.Persistence,
                SeedOffset = Setting.SeedOffset
            };

            return CurrentModule = module;
        }
    }
}
