using System.Linq;
using Scripts.Services.EventBus;
using Scripts.Systems.Camera.LocationView;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal class SwapContextSettingsRequestHandler : ISwapContextSettingsRequestHandler, IInitializable, ILateDisposable
    {
        protected readonly GridController _gridController;

        public SwapContextSettingsRequestHandler(GridController gridController) {
            _gridController = gridController;
        }

        void IInitializable.Initialize() => EventBus<IExternalLocationViewEventSubscriber>.Subscribe(this);

        void ILateDisposable.LateDispose() => EventBus<IExternalLocationViewEventSubscriber>.Unsubscribe(this);
        
        void ISwapContextSettingsRequestHandler.GetContextSettings(ISwapContextContainer container) {
            container.SwapContextSettings = new SwapContextSettings(
                leftMapSideX: _gridController.Map.MapInfo.First().Key.x,
                rightMapSideX: _gridController.Map.MapInfo.Last().Key.x,
                upperMapSideY: _gridController.Map.UpperSideMapY,
                lowerMapSideY: _gridController.Map.LowerSideMapY);
        }
    }
}