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
    public sealed class SphereBuilderViewModel : BuilderNodeViewModel<SphereBuilderSetting>
    {
        static SphereBuilderViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new SphereBuilderNodeView(), typeof(IViewFor<SphereBuilderViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Source { get; }

        public ValueNodeOutputViewModel<IBuilder> Output { get; }

        public SphereBuilderViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Sphere Builder";

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

            var builder = new SphereBuilder(Source.Value)
            {
                Name = Setting.Name,
                SouthLatBound = Setting.SouthLatBound,
                NorthLatBound = Setting.NorthLatBound,
                WestLonBound = Setting.WestLonBound,
                EastLonBound = Setting.EastLonBound
            };

            return CurrentBuilder = builder;
        }
    }
}
