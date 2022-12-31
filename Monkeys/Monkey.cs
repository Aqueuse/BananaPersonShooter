using System;
using Enums;
using UI.InGame;
using UnityEngine;

namespace Monkeys {
    public class Monkey : MonoBehaviour {
        [SerializeField] private UIMonkey associatedUI;
        
        public float maxSatiety;
        private float _satiety;

        private bool _isRunningToPlayer;
        private bool _isAttackingPlayer;
        private bool _isCatchingPlayer;

        private bool _isNearPlayer;

        private int _combatPhase = 1;
        float _combatPhaseFactor;
        
        private void Start() {
            _combatPhaseFactor = 3 / maxSatiety;
            
            associatedUI.SetMaxSatiety(maxSatiety);

            _satiety = maxSatiety;
        }

        private void Update() {
            if (_satiety < 20) {
                MonkeyManager.Instance.monkeyState = MonkeyState.STARVED;
            }
        }

        public void AddSatiety(float addedSatietyValue) {
            _satiety += addedSatietyValue;
            associatedUI.Add_Satiety(_satiety);
            
            if (_satiety >= maxSatiety) {
                MonkeyManager.Instance.GetHappy();
            }
            
            _combatPhase = Mathf.CeilToInt(_combatPhaseFactor * _satiety);
        }
    }
}