using DynamicData;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.Modules;
using LibNoiseShaderEditor.ViewModels.BuilderSettings;
using LibNoiseShaderEditor.Views.Builders;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LibNoiseShaderEditor.ViewModels.Builders
{
    public sealed class CylinderBuilderViewModel : BuilderNodeViewModel<CylinderBuilderSetting>
    {
        static CylinderBuilderViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CylinderBuilderNodeView(), typeof(IViewFor<CylinderBuilderViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Source { get; }

        public ValueNodeOutputViewModel<IBuilder> Output { get; }

        public CylinderBuilderViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Cylinder Builder";

            Source = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Source"
            };

            this.Inputs.Add(Source);

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Source.Value))
                .Select(_ => BuildBuilder());

            Output = new ValueNodeOutputViewModel<IBuilder>
            {
                Name = "Output",
                Value = outputValue
            };

            this.Outputs.Add(Output);
        }

        private IBuilder BuildBuilder()
        {
            if (Main?.Noise is null || Source.Value is null)
            {
                return CurrentBuilder = null;
            }

            var builder = new CylinderBuilder(Source.Value, Main.Noise.Seed)
            {
                Name = Setting.Name,
                LowerAngleBound = Setting.LowerAngleBound,
                UpperAngleBound = Setting.UpperAngleBound,
                LowerHeightBound = Setting.LowerHeightBound,
                UpperHeightBound = Setting.UpperHeightBound
            };

            return CurrentBuilder = builder;
        }
    }
}
