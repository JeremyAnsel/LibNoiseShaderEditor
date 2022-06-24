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
    public sealed class PlaneBuilderViewModel : BuilderNodeViewModel<PlaneBuilderSetting>
    {
        static PlaneBuilderViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new PlaneBuilderNodeView(), typeof(IViewFor<PlaneBuilderViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Source { get; }

        public ValueNodeOutputViewModel<IBuilder> Output { get; }

        public PlaneBuilderViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Plane Builder";

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

            var builder = new PlaneBuilder(Source.Value)
            {
                Name = Setting.Name,
                IsSeamless = Setting.IsSeamless,
                LowerBoundX = Setting.LowerBoundX,
                UpperBoundX = Setting.UpperBoundX,
                LowerBoundY = Setting.LowerBoundY,
                UpperBoundY = Setting.UpperBoundY
            };

            return CurrentBuilder = builder;
        }
    }
}
