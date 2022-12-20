using Enums;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    class BananaMan : MonoSingleton<BananaMan> {
        public BananasDataScriptableObject activeBanana;

        public BananaType activeBananaType = BananaType.EMPTY_HAND;
        public TpsPlayerAnimator tpsPlayerAnimator;
        
        public bool isArmed;
        public bool isInAir;

        public float health;
        public float resistance;
        
        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
        }

        public void GainHealth(InputAction.CallbackContext context) {
            health += activeBanana.healthBonus;
            resistance += activeBanana.resistanceBonus;
            
            UIVitals.Instance.Set_Health(health);
            UIVitals.Instance.Set_Resistance(resistance);
        }

        public void TakeDamage(int damageAmount) {
            if (resistance-damageAmount > 0) {
                resistance -= damageAmount;
            }

            else {
                resistance = 0;
                health -= damageAmount;
            }
            
            UIVitals.Instance.Set_Health(health);
            UIVitals.Instance.Set_Resistance(resistance);

            if (health - damageAmount <= 0) {
                UIVitals.Instance.Set_Health(0);
                GameManager.Instance.Death();
            }
            
            UIFace.Instance.GetHurted(health < 50);
        }
    }
}