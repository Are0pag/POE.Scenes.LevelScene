using System.Linq;
using Cysharp.Threading.Tasks;
using Scripts.Objects.Units;
using Scripts.Services.EventBus;
using Scripts.Services.Input;
using Scripts.Systems.GridGeneration;
using Scripts.Systems.GridMovement;
using Scripts.Systems.GridPlacement;
using Scripts.Tools.Attributes;
using UnityEngine;
using Zenject;

namespace Scripts.Scenes.LevelScene
{
    internal class GridController : MonoBehaviour
    {
        [SerializeField] private TempleData _templeData;
        [SerializeField] private float _intervalBetweenSpawnCells;

        private GridBuilder _gridBuilder;
        private GridPlacer _gridPlacer;
        
        internal protected GridContent GridContent { get; protected set; }
        internal protected GenerationInfoCallback Map { get; protected set; }

        [Inject]
        internal void Construct(GridBuilder gridBuilder, GridPlacer gridPlacer, GridContent gridContent) {
            _gridBuilder = gridBuilder;
            _gridPlacer = gridPlacer;
            GridContent = gridContent;
        }

        [InspectorButton(ExecutingMode.IsPlayingOnly)]
        internal async UniTask<GenerationInfoCallback> InitializeMap() {
            Map = _gridBuilder.SetMapInfo();
            await _gridBuilder.InstantiateMapSmoothlyAsync(Map.MapInfo, _intervalBetweenSpawnCells);
            SetDeckOnMap(Map);
            return Map;
        }

        internal void SetDeckOnMap(GenerationInfoCallback map) {
            var deckPositions = _gridPlacer.PlaceDeckOnGrid(_templeData.TempleDeck.Count, map);
            foreach (var hero in _templeData.TempleDeck) {
                var position = deckPositions.First();
                var movableComponent = Instantiate(hero.Value.Prefab, position, Quaternion.identity)
                                      .GetComponent<IUnit>().Movable.Value;
                GridContent.HeroesPositions.Add(movableComponent, position);
                deckPositions.Remove(position);
            }
        }
    }
}