using System.Linq;
using System.Reflection;
using Scripts.Systems.GridGeneration;
using Scripts.Tools.Attributes;
using Scripts.Tools.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal class Presenter : MonoBehaviour, IFocusTargetAbsenceNotification
    {
        [SerializeField, FindInScene] 
        protected GridLayoutGroup _generationSettingsGridLayoutGroup;
        
        [SerializeField] 
        protected GameObject _sliderPrefab;
        
        [SerializeField] 
        protected GameObject _focusAbsenceHintPrefab;

        protected Scripts.Systems.GridGeneration.Config _gridGenerationConfig;
        protected ContextGridGenerationSettings _contextGenerationSettings;

        [Inject]
        internal void Construct(Scripts.Systems.GridGeneration.Config gridGenerationConfig, ContextGridGenerationSettings generationSettings) {
            _gridGenerationConfig = gridGenerationConfig;
            _contextGenerationSettings = generationSettings;
        }

        [InspectorButton]
        internal void SetGridGenerationSettingsPanel() {
            var type = _gridGenerationConfig.DefaultGenerationSettings.GetType();
            foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (fieldInfo.GetCustomAttributes(typeof(RangeAttribute)).FirstOrDefault() as RangeAttribute is not { } rangeAttribute)
                    return;

                var go = GameObject.Instantiate(_sliderPrefab, _generationSettingsGridLayoutGroup.transform, false);

                go.GetComponent<SliderDisplayedNameAndValue>().Initialize(
                    nameText: fieldInfo.Name,
                    onInput: (value) => _contextGenerationSettings
                                       .GetType()
                                       .GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.NonPublic)
                                      ?.SetValue(_contextGenerationSettings, value),
                    
                    initializeValue: (int)fieldInfo.GetValue(_gridGenerationConfig.DefaultGenerationSettings),
                    minValue: (int)rangeAttribute.min,
                    maxValue: (int)rangeAttribute.max);
            }
            
            _generationSettingsGridLayoutGroup.gameObject.SetActive(false);
        }

        void IFocusTargetAbsenceNotification.Notify() {
            
        }
    }
}