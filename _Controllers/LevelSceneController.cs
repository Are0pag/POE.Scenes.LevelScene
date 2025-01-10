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

        /* protected? */
        [Inject]
        internal void Construct(GridController gridController, CameraController cameraController) {
            _gridController = gridController;
            _cameraController = cameraController;
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        internal async void RunLocationView() {
            EventBus<IExternalLocationViewEventSubscriber>.RaiseEvent<IFreeViewHandler>
                (h => h.OnViewLocation(new ShowMapArgs(_gridCall.CentersOfSections), _asyncOperationHandler));
            await _asyncOperationHandler.RunAsync();
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
                EventBus<IExternalLocationViewEventSubscriber>.RaiseEvent<Scripts.Systems.Camera.LocationView.IInputHandler>(h => h.OnMouseButtonDown());
            }
        }

        public void OnMouseButtonUp() {
            EventBus<IExternalLocationViewEventSubscriber>.RaiseEvent<Scripts.Systems.Camera.LocationView.IInputHandler>(h => h.OnMouseButtonUp());
        }
    }
    
    /*
     * Проблема: INPUT_CONFLICT
     * Solution1:
     *      Внедрить Input Module непосредственно в контроллер / принимать сигналы от него в контроллере
     *          -> вытекающая проблема: организация опосредованного регулирования исполняемых модулей (сейчас они саморегулирующиеся)
     * 
     *              -EventBus- И подписчик и издатель ивента должны знать о его типе, но исполняемые модули не должны ссылаться на контроллер
     *                  однако исполняемый модуль может располагать функционалом, к которому МОЖЕТ обратиться контроллер
     * 
     *              -Агрегация- Обращение контроллера по абстрактной ссылке на интерфейс модуля
     */
}