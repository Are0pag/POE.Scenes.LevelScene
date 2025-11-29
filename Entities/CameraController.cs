using POE.Systems.Camera.LocationView;
using Scripts.Services.EventBus;
using Scripts.Systems.Camera.LocationView;
using Scripts.Tools.AsyncOperationsHandle;
using Scripts.Tools.Attributes;
using UnityEngine;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    public class CameraController : MonoBehaviour
    {
        internal protected ViewModule ViewModule { get; protected set; }
        protected private GridController _gridController;

        protected readonly AsyncOperationHandlerInitialized _zoomOperationHandler = new();
        protected readonly AsyncOperationHandlerInitialized _viewOperationHandler = new();

        protected LocationViewDataAdapter _locationViewDataAdapter;

        [Inject]
        internal void Construct(ViewModule viewModule, GridController gridController, LocationViewDataAdapter locationViewDataAdapter) {
            ViewModule = viewModule;
            _gridController = gridController;
            _locationViewDataAdapter = locationViewDataAdapter;
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        public void SetSwapState() {
            ViewModule.SetState(ViewModule.SwapState);
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        public void SetFocusState() {
            ViewModule.SetState(ViewModule.FocusState);
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        public async void ViewLocation() {
            if (_viewOperationHandler.IsRunning)
                return;

            EventBus<IExternalLocationViewEventSubscriber>
               .RaiseEvent<IFreeViewHandler>(h => h.OnViewLocation(
                                                 args: new ShowMapArgs(movePointsTrace: _locationViewDataAdapter.AdaptMoveTrace(_gridController.Map.CentersOfSections)),
                                                 asyncOperationHandler: _viewOperationHandler));

            await _viewOperationHandler.RunAsync();
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        public async void SkipViewLocation() {
            if (!_viewOperationHandler.IsRunning)
                return;

            _viewOperationHandler.Cancel();
            
            EventBus<IExternalLocationViewEventSubscriber>
               .RaiseEvent<ISkipFreeViewHandler>(h => h.OnSkip(_viewOperationHandler));


            await _viewOperationHandler.RunAsync();
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        public async void ZoomIn() {
            if (_zoomOperationHandler.IsRunning)
                return;

            EventBus<IExternalLocationViewEventSubscriber>.RaiseEvent<IZoomHandler>(h => h.ZoomIn(_zoomOperationHandler));
            await _zoomOperationHandler.RunAsync();
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        public async void ZoomOut() {
            if (_zoomOperationHandler.IsRunning)
                return;

            EventBus<IExternalLocationViewEventSubscriber>.RaiseEvent<IZoomHandler>(h => h.ZoomOut(_zoomOperationHandler));
            await _zoomOperationHandler.RunAsync();
        }
    }
}