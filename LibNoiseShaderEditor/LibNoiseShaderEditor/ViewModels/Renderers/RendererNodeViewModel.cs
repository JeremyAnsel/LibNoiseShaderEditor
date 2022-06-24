using JeremyAnsel.LibNoiseShader.Renderers;
using LibNoiseShaderEditor.ViewModels.RendererSettings;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LibNoiseShaderEditor.ViewModels.Renderers
{
    public abstract class RendererNodeViewModel : NodeViewModel
    {
    }

    public abstract class RendererNodeViewModel<T> : RendererNodeViewModel
        where T : PropertyChangedObject, IRendererSetting, new()
    {
        public RendererNodeViewModel(MainViewModel main)
        {
            this.Main = main;
        }

        public MainViewModel Main { get; }

        public T Setting { get; } = new T();

        private IRenderer _currentRenderer;

        public IRenderer CurrentRenderer
        {
            get => _currentRenderer;
            set => this.RaiseAndSetIfChanged(ref _currentRenderer, value);
        }

        protected IObservable<IRenderer> BuildOutputValue()
        {
            var outputValue = this
                .WhenAnyValue(
                vm => vm.Parent,
                vm => vm.Main.Noise,
                vm => vm.Setting.Changed)
                .Select(_ => (IRenderer)null);

            return outputValue;
        }

        protected IObservable<IRenderer> BuildOutputValue<T1>(IObservable<T1> value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var outputValue = Observable.Merge(
                this.BuildOutputValue(),
                value.Select(_ => (IRenderer)null));

            return outputValue;
        }
    }
}
