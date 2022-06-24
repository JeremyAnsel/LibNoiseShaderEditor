using DynamicData;
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
    public sealed class BlendRendererViewModel : RendererNodeViewModel<BlendRendererSetting>
    {
        static BlendRendererViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new BlendRendererNodeView(), typeof(IViewFor<BlendRendererViewModel>));
        }

        public ValueNodeInputViewModel<IRenderer> Input1 { get; }

        public ValueNodeInputViewModel<IRenderer> Input2 { get; }

        public ValueNodeOutputViewModel<IRenderer> Output { get; }

        public BlendRendererViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Blend Renderer";

            Input1 = new ValueNodeInputViewModel<IRenderer>
            {
                Name = $"Input 1"
            };

            this.Inputs.Add(Input1);

            Input2 = new ValueNodeInputViewModel<IRenderer>
            {
                Name = $"Input 2"
            };

            this.Inputs.Add(Input2);

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Input1.Value,
                    vm => vm.Input2.Value))
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
            if (Main?.Noise is null || Input1.Value is null || Input2.Value is null)
            {
                return CurrentRenderer = null;
            }

            var renderer = new BlendRenderer(Input1.Value, Input2.Value);

            return CurrentRenderer = renderer;
        }
    }
}
