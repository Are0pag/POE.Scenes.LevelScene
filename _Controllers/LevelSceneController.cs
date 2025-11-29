using DG.Tweening;
using Scripts.Services.EventBus;
using Scripts.Services.Input;
using Scripts.Systems.Camera.LocationView;
using Scripts.Systems.GridGeneration;
using Scripts.Tools.AsyncOperationsHandle;
using Scripts.Tools.Attributes;
using UnityEngine;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal class LevelSceneController : MonoBehaviour, Scripts.Services.Input.IInputHandler
    {
        protected GridController _gridController;
        protected CameraController _cameraController;
        protected readonly AsyncOperationHandlerInitialized _asyncOperationHandler = new AsyncOperationHandlerInitialized();
        private GenerationInfoCallback _gridCall;

        [Inject]
        internal void Construct(GridController gridController, CameraController cameraController) {
            _gridController = gridController;
            _cameraController = cameraController;
        }
        
        private void OnEnable() {
            EventBus<IInputSubscriber>.Subscribe(this);
            DOTween.SetTweensCapacity(10000, 50);
        }

        private void OnDisable() {
            EventBus<IInputSubscriber>.Unsubscribe(this);
        }

        public void OnMouseButtonDown() {
            if (!_gridController.GridContent.IsMouseOnGrid) {
                EventBus<IExternalLocationViewEventSubscriber>
                   .RaiseEvent<Scripts.Systems.Camera.LocationView.IInputHandler>(h => h.OnMouseButtonDown());
            }
        }

        public void OnMouseButtonUp() {
            EventBus<IExternalLocationViewEventSubscriber>
               .RaiseEvent<Scripts.Systems.Camera.LocationView.IInputHandler>(h => h.OnMouseButtonUp());
        }
    }
}