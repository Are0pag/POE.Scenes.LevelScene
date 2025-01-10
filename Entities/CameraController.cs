using System.Linq;
using Scripts.Services.EventBus;
using Scripts.Systems.Camera.LocationView;
using Scripts.Systems.GridMovement;
using Scripts.Tools.AsyncOperationsHandle;
using Scripts.Tools.Attributes;
using UnityEngine;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal class CameraController : MonoBehaviour, ISwapContextSettingsRequestHandler, IFocusTargetRequestHandler, ITargetChangeNotification
    {
        internal protected ViewModule ViewModule { get; protected set; }
        protected GridController _gridController;
        protected readonly AsyncOperationHandlerInitialized _zoomOperationHandler = new AsyncOperationHandlerInitialized();
        protected readonly AsyncOperationHandlerInitialized _viewOperationHandler = new AsyncOperationHandlerInitialized();

        protected IFocusTargetContainer _focusTargetContainer;
        protected LocationViewDataAdapter _locationViewDataAdapter;

        [Inject]
        internal void Construct(ViewModule viewModule, GridController gridController, LocationViewDataAdapter locationViewDataAdapter) {
            ViewModule = viewModule;
            _gridController = gridController;
            _locationViewDataAdapter = locationViewDataAdapter;
        }

        
        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        internal void SetSwapState() {
            ViewModule.SetState(ViewModule.SwapState);
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        internal void SetFocusState() {
            ViewModule.SetState(ViewModule.FocusState);
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        internal async void ViewLocation() {
            if (_viewOperationHandler.IsRunning)
                return;

            EventBus<IExternalLocationViewEventSubscriber>
               .RaiseEvent<IFreeViewHandler>(h => h.OnViewLocation(
                                                 args: new ShowMapArgs(movePointsTrace: _locationViewDataAdapter.AdaptMoveTrace(_gridController.Map.CentersOfSections)),
                                                 asyncOperationHandler: _viewOperationHandler));

            await _viewOperationHandler.RunAsync();
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        internal async void ZoomIn() {
            if (_zoomOperationHandler.IsRunning)
                return;

            EventBus<IExternalLocationViewEventSubscriber>.RaiseEvent<IZoomHandler>(h => h.ZoomIn(_zoomOperationHandler));
            await _zoomOperationHandler.RunAsync();
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        internal async void ZoomOut() {
            if (_zoomOperationHandler.IsRunning)
                return;

            EventBus<IExternalLocationViewEventSubscriber>.RaiseEvent<IZoomHandler>(h => h.ZoomOut(_zoomOperationHandler));
            await _zoomOperationHandler.RunAsync();
        }
        

        void ISwapContextSettingsRequestHandler.GetContextSettings(ISwapContextContainer container) {
            container.SwapContextSettings = new SwapContextSettings(
                leftMapSideX: _gridController.Map.MapInfo.First().Key.x,
                rightMapSideX: _gridController.Map.MapInfo.Last().Key.x,
                upperMapSideY: _gridController.Map.UpperSideMapY,
                lowerMapSideY: _gridController.Map.LowerSideMapY);
        }

        private void OnEnable() {
            EventBus<IExternalLocationViewEventSubscriber>.Subscribe(this);
            EventBus<IExternalGridMovementSubscriber>.Subscribe(this);
        }

        private void OnDisable() {
            EventBus<IExternalLocationViewEventSubscriber>.Unsubscribe(this);
            EventBus<IExternalGridMovementSubscriber>.Unsubscribe(this);
        }

        void IFocusTargetRequestHandler.GetTarget(IFocusTargetContainer focusTargetContainer) {
            if (_gridController.GridContent.SelectedHero == null) {
                EventBus<IExternalLevelSceneSubscriber>.
                    RaiseEvent<IFocusTargetAbsenceNotification>(n => n.Notify());
                return;
            }

            _focusTargetContainer ??= focusTargetContainer;
            _focusTargetContainer.Target = _gridController.GridContent.SelectedHero?.MovingObject.transform;
        }

        void ITargetChangeNotification.OnTargetChange(IGridMovable movable) {
            if (_focusTargetContainer != null)
                _focusTargetContainer.Target = movable.MovingObject.transform;
        }
    }
}