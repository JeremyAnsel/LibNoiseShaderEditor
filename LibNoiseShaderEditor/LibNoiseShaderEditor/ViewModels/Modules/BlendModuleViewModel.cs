﻿using DynamicData;
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
    public sealed class BlendModuleViewModel : ModuleNodeViewModel<BlendModuleSetting>
    {
        static BlendModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new BlendModuleNodeView(), typeof(IViewFor<BlendModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeInputViewModel<IModule> Input2 { get; }

        public ValueNodeInputViewModel<IModule> Control { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public BlendModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Blend Module";

            Input1 = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Input 1"
            };

            this.Inputs.Add(Input1);

            Input2 = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Input 2"
            };

            this.Inputs.Add(Input2);

            Control = new ValueNodeInputViewModel<IModule>
            {
                Name = $"Control"
            };

            this.Inputs.Add(Control);

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Input1.Value,
                    vm => vm.Input2.Value,
                    vm => vm.Control.Value))
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
            if (Main?.Noise is null || Input1.Value is null || Input2.Value is null || Control.Value is null)
            {
                return CurrentModule = null;
            }

            var module = new BlendModule(Input1.Value, Input2.Value, Control.Value)
            {
                Name = Setting.Name
            };

            return CurrentModule = module;
        }
    }
}
