using Scripts.Services.EventBus;
using Scripts.Systems.Camera.LocationView;
using Scripts.Systems.GridMovement;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal class FocusTargetHandler : IFocusTargetRequestHandler, ITargetChangeNotification, IInitializable, ILateDisposable
    {
        protected readonly GridController _gridController;
        protected IFocusTargetContainer _focusTargetContainer;

        internal FocusTargetHandler(GridController gridController) {
            _gridController = gridController;
        }

        void IInitializable.Initialize() {
            EventBus<IExternalLocationViewEventSubscriber>.Subscribe(this);
            EventBus<IExternalGridMovementSubscriber>.Subscribe(this);
        }

        void ILateDisposable.LateDispose() {
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