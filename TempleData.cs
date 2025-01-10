using System;
using System.Collections.Generic;
using Scripts.Objects.Units;
using Scripts.Tools;
using UnityEngine;

namespace Scripts.Scenes.LevelScene
{
    [Serializable]
    public class TempleData
    {
        [SerializeField] private List<InterfaceRef<IUnitData>> _templeDeck = new();
        public List<InterfaceRef<IUnitData>> TempleDeck {
            get => _templeDeck;
            private set => _templeDeck = value;
        }
    }
}