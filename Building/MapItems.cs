using Game.BananaCannonMiniGame;
using Monkeys;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class MapItems : MonoSingleton<MapItems> {
        public GameObject debrisContainer;
        public GameObject plateformsContainer;

        public UICanvasItemsHiddableManager uiCanvasItemsHiddableManager;
        public DebrisSpawner debrisSpawner;

        public Monkey[] monkeys;
    }
}
