using System.Collections.Generic;
using Scripts.Tools.Attributes;
using Scripts.Tools.View;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Scenes.LevelScene
{
    internal sealed class DemoContextGridGenerationSettingsView : MonoBehaviour
    {
        [SerializeField, FindInScene] private GridLayoutGroup _generationSettingsGridLayoutGroup;

        [SerializeField] private GameObject _sliderPrefab;

        internal void InitializeSettingsPanel(List<DemoContextGridGenerationSettingsArgs> argsList) {
            foreach (var arg in argsList) {
                Instantiate(_sliderPrefab, _generationSettingsGridLayoutGroup.transform, false)
                   .GetComponent<SliderDisplayedNameAndValue>().Initialize(arg.FieldInfoName, arg.OnInput, arg.InitializeValue, arg.MaxValue, arg.MaxValue);
            }
        }
    }
}