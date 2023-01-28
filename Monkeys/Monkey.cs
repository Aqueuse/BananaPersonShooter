using Enums;
using Player;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;

namespace Monkeys {
    public class Monkey : MonoBehaviour {
        public UIMonkey associatedUI;
        
        private float _maxHappiness;
        public float happiness = 50;
        public MonkeyState monkeyState;

        public float sasiety;

        private bool _isRunningToPlayer;
        private bool _isAttackingPlayer;
        private bool _isCatchingPlayer;

        private bool _isNearPlayer;

        private void Start() {
            monkeyState = MonkeyState.SAD;
            MapManager.Instance.RecalculateHapiness();
            associatedUI.SetSliderValue(happiness, monkeyState);
        }

        public void Feed(float addedBananaValue) {
            if (happiness <= 50) {
                GetComponent<NavMeshAgent>().SetDestination(BananaMan.Instance.transform.position);
                sasiety += addedBananaValue;
                MapManager.Instance.RecalculateHapiness();
                associatedUI.SetSliderValue(happiness, monkeyState);
            }
        }
    }
}