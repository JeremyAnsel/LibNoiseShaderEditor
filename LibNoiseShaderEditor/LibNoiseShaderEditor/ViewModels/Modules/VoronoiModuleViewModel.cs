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
    public sealed class VoronoiModuleViewModel : ModuleNodeViewModel<VoronoiModuleSetting>
    {
        static VoronoiModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new VoronoiModuleNodeView(), typeof(IViewFor<VoronoiModuleViewModel>));
        }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public VoronoiModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Voronoi Module";

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

            var module = new VoronoiModule(Main.Noise)
            {
                Name = Setting.Name,
                IsDistanceApplied = Setting.IsDistanceApplied,
                Displacement = Setting.Displacement,
                Frequency = Setting.Frequency,
                SeedOffset = Setting.SeedOffset
            };

            return CurrentModule = module;
        }
    }
}
