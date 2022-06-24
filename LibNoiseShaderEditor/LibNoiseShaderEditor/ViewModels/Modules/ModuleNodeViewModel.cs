using JeremyAnsel.LibNoiseShader.Modules;
using LibNoiseShaderEditor.ViewModels.ModuleSettings;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LibNoiseShaderEditor.ViewModels.Modules
{
    public abstract class ModuleNodeViewModel : NodeViewModel
    {
    }

    public abstract class ModuleNodeViewModel<T> : ModuleNodeViewModel
        where T : PropertyChangedObject, IModuleSetting, new()
    {
        public ModuleNodeViewModel(MainViewModel main)
        {
            this.Main = main;
        }

        public MainViewModel Main { get; }

        public T Setting { get; } = new T();

        private IModule _currentModule;

        public IModule CurrentModule
        {
            get => _currentModule;
            set => this.RaiseAndSetIfChanged(ref _currentModule, value);
        }

        protected IObservable<IModule> BuildOutputValue()
        {
            var outputValue = this
                .WhenAnyValue(
                vm => vm.Parent,
                vm => vm.Main.Noise,
                vm => vm.Setting.Changed)
                .Select(_ => (IModule)null);

            return outputValue;
        }

        protected IObservable<IModule> BuildOutputValue<T1>(IObservable<T1> value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var outputValue = Observable.Merge(
                this.BuildOutputValue(),
                value.Select(_ => (IModule)null));

            return outputValue;
        }
    }
}
