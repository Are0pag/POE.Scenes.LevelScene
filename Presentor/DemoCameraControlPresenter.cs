using System.Reflection;
using Scripts.Scenes.LevelScene;
using Scripts.Tools.View;
using Zenject;

namespace POE.Scenes.LevelScene
{
    internal sealed class DemoCameraControlPresenter : IInitializable, ILateDisposable
    {
        private readonly DemoCameraControlView _view;
        private readonly CameraController _cameraController;

        internal DemoCameraControlPresenter(CameraController cameraController, DemoCameraControlView view) {
            _cameraController = cameraController;
            _view = view;
        }

        void IInitializable.Initialize() {
            _view.ZoomInButton.onClick.AddListener(() => _cameraController.ZoomIn());
            _view.ZoomOutButton.onClick.AddListener(() => _cameraController.ZoomOut());
            _view.StartLocationViewButton.onClick.AddListener(() => _cameraController.ViewLocation());
            _view.SkipLocationViewButton.onClick.AddListener(() => _cameraController.SkipViewLocation());
            _view.SetFocusStateButton.onClick.AddListener(() => _cameraController.SetFocusState());
            _view.SetSwapStateButton.onClick.AddListener(() => _cameraController.SetSwapState());
        }

        void ILateDisposable.LateDispose() {
            foreach (var buttonField in _view.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)) {
                LinkedButton but = (LinkedButton)buttonField.GetValue(_view);
                but.onClick.RemoveAllListeners();
            }
        }
    }
}