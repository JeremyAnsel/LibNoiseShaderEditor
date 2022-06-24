using DynamicData;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.ViewModels.RendererSettings;
using LibNoiseShaderEditor.Views.Renderers;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LibNoiseShaderEditor.ViewModels.Renderers
{
    public sealed class NormalRendererViewModel : RendererNodeViewModel<NormalRendererSetting>
    {
        static NormalRendererViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new NormalRendererNodeView(), typeof(IViewFor<NormalRendererViewModel>));
        }

        public ValueNodeInputViewModel<IBuilder> Source { get; }

        public ValueNodeOutputViewModel<IRenderer> Output { get; }

        public NormalRendererViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Normal Renderer";

            Source = new ValueNodeInputViewModel<IBuilder>
            {
                Name = $"Source"
            };

            this.Inputs.Add(Source);

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Source.Value))
                .Select(_ => BuildBuilder());

            Output = new ValueNodeOutputViewModel<IRenderer>
            {
                Name = "Output",
                Value = outputValue
            };

            this.Outputs.Add(Output);
        }

        private IRenderer BuildBuilder()
        {
            if (Main?.Noise is null || Source.Value is null)
            {
                return CurrentRenderer = null;
            }

            var renderer = new NormalRenderer(Source.Value)
            {
                BumpHeight = Setting.BumpHeight,
                IsWrapEnabled = Setting.IsWrapEnabled
            };

            return CurrentRenderer = renderer;
        }
    }
}
