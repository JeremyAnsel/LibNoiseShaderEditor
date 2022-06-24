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
    public sealed class MinModuleViewModel : ModuleNodeViewModel<MinModuleSetting>
    {
        static MinModuleViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new MinModuleNodeView(), typeof(IViewFor<MinModuleViewModel>));
        }

        public ValueNodeInputViewModel<IModule> Input1 { get; }

        public ValueNodeInputViewModel<IModule> Input2 { get; }

        public ValueNodeOutputViewModel<IModule> Output { get; }

        public MinModuleViewModel(MainViewModel main)
            : base(main)
        {
            this.Name = "Min Module";

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

            var outputValue = this.BuildOutputValue(
                this.WhenAnyValue(
                    vm => vm.Input1.Value,
                    vm => vm.Input2.Value))
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
            if (Main?.Noise is null || Input1.Value is null || Input2.Value is null)
            {
                return CurrentModule = null;
            }

            var module = new MinModule(Input1.Value, Input2.Value)
            {
                Name = Setting.Name
            };

            return CurrentModule = module;
        }
    }
}
