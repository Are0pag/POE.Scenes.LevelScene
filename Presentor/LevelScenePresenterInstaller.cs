using Scripts.Scenes.LevelScene;
using Scripts.Systems.GridGeneration;
using Scripts.Tools.Attributes;
using UnityEngine;
using Zenject;

namespace POE.Scenes.LevelScene
{
    internal class LevelScenePresenterInstaller : MonoInstaller
    {
        [SerializeField, FindInScene] 
        protected private DemoContextGridGenerationSettingsView _demoContextGridGenerationSettingsView;

        [SerializeField, FindInScene] 
        protected private DemoCameraControlView _cameraControlView;

        public override void InstallBindings() {
            Container.Bind<ContextGridGenerationSettings>().AsSingle();

            Container.Bind<DemoContextGridGenerationSettingsExposer>().AsSingle();
            Container.Bind<DemoContextGridGenerationSettingsView>().FromInstance(_demoContextGridGenerationSettingsView).AsSingle();
            Container.BindInterfacesAndSelfTo<DemoContextGridGenerationSettingsPresenter>().AsSingle();
            
            Container.Bind<DemoCameraControlView>().FromInstance(_cameraControlView).AsSingle();
            Container.BindInterfacesAndSelfTo<DemoCameraControlPresenter>().AsSingle();
        }
    }
}