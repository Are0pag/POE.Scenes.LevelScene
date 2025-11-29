using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal sealed class DemoContextGridGenerationSettingsPresenter : IInitializable, ILateDisposable
    {
        private readonly DemoContextGridGenerationSettingsExposer _exposer;
        private readonly DemoContextGridGenerationSettingsView _view;

        internal DemoContextGridGenerationSettingsPresenter(DemoContextGridGenerationSettingsView view, DemoContextGridGenerationSettingsExposer exposer) {
            _view = view;
            _exposer = exposer;
        }

        void IInitializable.Initialize() {
            _exposer.OnContextGridGenerationSettingsExposed += _view.InitializeSettingsPanel;
        }

        void ILateDisposable.LateDispose() {
            _exposer.OnContextGridGenerationSettingsExposed -= _view.InitializeSettingsPanel;
        }
    }
}