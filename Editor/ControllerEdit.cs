using Scripts.Scenes.LevelScene;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Scenes.LevelScene.Editor
{
    [CanEditMultipleObjects]
    /*[CustomEditor(typeof(GridController))]*/
    public class ControllerEdit : UnityEditor.Editor
    {
        /*public override void OnInspectorGUI() {
            AddButtons();
            base.OnInspectorGUI();
        }

        private void AddButtons() {
            var controller = (GridController)target;
            if (Application.isPlaying && GUILayout.Button(nameof(controller.InitializeScene))) controller.InitializeScene();
        }*/
    }
}