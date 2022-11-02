using UnityEngine;

namespace Bosses {
    public class Boss : MonoBehaviour {
        private float _satiety;

        public void AddSatiety(float addedSatietyValue) {
            _satiety += addedSatietyValue;
            BossManager.Instance.associatedUI.Add_Satiety(_satiety);
            if (_satiety >= 10) {
                BossManager.Instance.EndBossFight(true);
            }
        }
    }
}
