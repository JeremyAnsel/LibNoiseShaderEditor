using DynamicData;
using JeremyAnsel.LibNoiseShader.Builders;
using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.ViewModels.RendererSettings;
using LibNoiseShaderEditor.Views.Renderers;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace LibNoiseShaderEditor.ViewModels.Renderers
{
    public sealed class ImageRendererViewModel : RendererNodeViewModel<ImageRendererSetting>
    {
        static ImageRendererViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ImageRendererNodeView(), typeof(IViewFor<ImageRendererViewModel>));
        }

        public ValueNodeInputViewModel<IBuilder> Source { get; }

        public ValueNodeOutputViewModel<IRenderer> Output { get; }

        public ImageRendererViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Image Renderer";

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

            var renderer = new ImageRenderer(Source.Value)
            {
                IsWrapEnabled = Setting.IsWrapEnabled,
                IsLightEnabled = Setting.IsLightEnabled,
                LightAzimuth = Setting.LightAzimuth,
                LightBrightness = Setting.LightBrightness,
                LightColor = System.Drawing.Color.FromArgb(
                    Setting.LightColor.A,
                    Setting.LightColor.R,
                    Setting.LightColor.G,
                    Setting.LightColor.B),
                LightContrast = Setting.LightContrast,
                LightElevation = Setting.LightElevation,
                LightIntensity = Setting.LightIntensity
            };

            renderer.SetGradientPoints(
                Setting.GradientPoints.Select(
                    t => new KeyValuePair<float, System.Drawing.Color>(
                        t.Key,
                        System.Drawing.Color.FromArgb(
                            t.Value.A,
                            t.Value.R,
                            t.Value.G,
                            t.Value.B))));

            return CurrentRenderer = renderer;
        }
    }
}
