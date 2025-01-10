using Scripts.Systems.GridGeneration;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal class LevelSceneModelInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            Container.Bind<ContextGridGenerationSettings>().AsSingle();
            Container.Bind<Presenter>().AsSingle();
        }
    }
}