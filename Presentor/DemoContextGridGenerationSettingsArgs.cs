using System;

namespace Scripts.Scenes.LevelScene
{
    public struct DemoContextGridGenerationSettingsArgs
    {
        public readonly string FieldInfoName;
        public readonly Action<int> OnInput;
        public readonly int InitializeValue;
        public readonly int MaxValue;
        public readonly int MinValue;

        public DemoContextGridGenerationSettingsArgs(string fieldInfoName, Action<int> onInput, int initializeValue, int maxValue, int minValue) {
            FieldInfoName = fieldInfoName;
            OnInput = onInput;
            InitializeValue = initializeValue;
            MaxValue = maxValue;
            MinValue = minValue;
        }
    }
}