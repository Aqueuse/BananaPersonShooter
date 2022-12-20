using UnityEngine;

namespace Bosses {
    public class Boss : MonoBehaviour {
        public float maxSatiety;
        private float _satiety;

        private bool _isRunningToPlayer;
        private bool _isAttackingPlayer;
        private bool _isCatchingPlayer;
        public bool isSatieted;

        private bool _isNearPlayer;

        private int _combatPhase = 1;
        float _combatPhaseFactor;
        
        private void Start() {
            _combatPhaseFactor = 3 / maxSatiety;
            BossManager.Instance.associatedUI.SetMaxSatiety(maxSatiety);
        }

        public void AddSatiety(float addedSatietyValue) {
            Debug.Log(_satiety);
            
            _satiety += addedSatietyValue;
            BossManager.Instance.associatedUI.Add_Satiety(_satiety);
            
            if (_satiety >= maxSatiety) {
                BossManager.Instance.Win();
            }
            
            _combatPhase = Mathf.CeilToInt(_combatPhaseFactor * _satiety);
        }
    }
}