using Scripts.Scenes.LevelScene;
using Scripts.Tools.Attributes;
using UnityEngine;
using Zenject;

namespace Scenes.Scenes.LevelScene
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField, FindInScene] protected private LevelSceneController _levelSceneController;
        [SerializeField] protected private GridController _gridController;
        [SerializeField] protected private CameraController _cameraController;

        public override void InstallBindings() {
            Container.Bind<GridController>().FromInstance(_gridController).AsSingle();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
            Container.Bind<LevelSceneController>().FromInstance(_levelSceneController).AsSingle();
            
            Container.BindInterfacesAndSelfTo<FocusTargetHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SwapContextSettingsRequestHandler>().AsSingle();
        }
    }
}