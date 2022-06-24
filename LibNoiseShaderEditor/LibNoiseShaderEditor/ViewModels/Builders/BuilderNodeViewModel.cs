using JeremyAnsel.LibNoiseShader.Builders;
using LibNoiseShaderEditor.ViewModels.BuilderSettings;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LibNoiseShaderEditor.ViewModels.Builders
{
    public abstract class BuilderNodeViewModel : NodeViewModel
    {

    }

    public abstract class BuilderNodeViewModel<T> : BuilderNodeViewModel
        where T : PropertyChangedObject, IBuilderSetting, new()
    {
        public BuilderNodeViewModel(MainViewModel main)
        {
            this.Main = main;
        }

        public MainViewModel Main { get; }

        public T Setting { get; } = new T();

        private IBuilder _currentBuilder;

        public IBuilder CurrentBuilder
        {
            get => _currentBuilder;
            set => this.RaiseAndSetIfChanged(ref _currentBuilder, value);
        }

        protected IObservable<IBuilder> BuildOutputValue()
        {
            var outputValue = this
                .WhenAnyValue(
                vm => vm.Parent,
                vm => vm.Main.Noise,
                vm => vm.Setting.Changed)
                .Select(_ => (IBuilder)null);

            return outputValue;
        }

        protected IObservable<IBuilder> BuildOutputValue<T1>(IObservable<T1> value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var outputValue = Observable.Merge(
                this.BuildOutputValue(),
                value.Select(_ => (IBuilder)null));

            return outputValue;
        }
    }
}
