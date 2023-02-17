using Enums;
using Game;
using Player;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;

namespace Monkeys {
    public class Monkey : MonoBehaviour {
        public UIMonkey associatedUI;
        
        private float _maxHappiness;
        public float happiness;
        public MonkeyState monkeyState;

        public float sasiety;

        private bool _isRunningToPlayer;
        private bool _isAttackingPlayer;
        private bool _isCatchingPlayer;

        private bool _isNearPlayer;

        private void Start() {
            sasiety = 20;
            MapsManager.Instance.currentMap.RecalculateHapiness();
            associatedUI.SetSliderValue(happiness, monkeyState);
        }

        public void Feed(float addedBananaValue) {
            if (happiness <= 50) {
                GetComponent<NavMeshAgent>().SetDestination(BananaMan.Instance.transform.position);
                sasiety += addedBananaValue;
                MapsManager.Instance.currentMap.RecalculateHapiness();
                associatedUI.SetSliderValue(happiness, monkeyState);
            }
        }
    }
}