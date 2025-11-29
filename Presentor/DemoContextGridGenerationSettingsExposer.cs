using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scripts.Systems.GridGeneration;
using UnityEngine;

namespace Scripts.Scenes.LevelScene
{
    internal sealed class DemoContextGridGenerationSettingsExposer
    {
        private readonly Scripts.Systems.GridGeneration.Config _gridGenerationConfig;
        private readonly ContextGridGenerationSettings _contextGenerationSettings;

        internal System.Action<List<DemoContextGridGenerationSettingsArgs>> OnContextGridGenerationSettingsExposed;

        internal DemoContextGridGenerationSettingsExposer(Config gridGenerationConfig, ContextGridGenerationSettings contextGenerationSettings) {
            _gridGenerationConfig = gridGenerationConfig;
            _contextGenerationSettings = contextGenerationSettings;
        }

        internal void SetGridGenerationSettingsPanel() {
            var list = new List<DemoContextGridGenerationSettingsArgs>();
            
            var type = _gridGenerationConfig.DefaultGenerationSettings.GetType();
            foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (fieldInfo.GetCustomAttributes(typeof(RangeAttribute)).FirstOrDefault() as RangeAttribute is not { } rangeAttribute)
                    continue;

                list.Add(
                    new DemoContextGridGenerationSettingsArgs(
                            fieldInfoName: fieldInfo.Name,
                            onInput: (value) => _contextGenerationSettings
                                               .GetType()
                                               .GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.NonPublic)
                                              ?.SetValue(_contextGenerationSettings, value),
                            initializeValue: (int)fieldInfo.GetValue(_gridGenerationConfig.DefaultGenerationSettings),
                            maxValue: (int)rangeAttribute.min,
                            minValue: (int)rangeAttribute.max
                        ));
            }
            OnContextGridGenerationSettingsExposed?.Invoke(list);
        }
    }
}