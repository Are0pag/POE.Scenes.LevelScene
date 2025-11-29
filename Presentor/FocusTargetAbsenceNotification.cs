using UnityEngine;

namespace Scripts.Scenes.LevelScene
{
    internal class FocusTargetAbsenceNotification : MonoBehaviour, IFocusTargetAbsenceNotification
    {
        [SerializeField] protected GameObject _focusAbsenceHintPrefab;


        void IFocusTargetAbsenceNotification.Notify() {
        }
    }
}