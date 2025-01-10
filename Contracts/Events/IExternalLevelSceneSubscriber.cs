namespace Scripts.Scenes.LevelScene
{
    public interface IExternalLevelSceneSubscriber
    {
        
    }

    public interface IFocusTargetAbsenceNotification : IExternalLevelSceneSubscriber
    {
        void Notify();
    }
}