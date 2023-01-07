using Enums;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    class BananaMan : MonoSingleton<BananaMan> {
        public TpsPlayerAnimator tpsPlayerAnimator;

        public BananasDataScriptableObject activeItem;
        public ItemThrowableType activeItemThrowableType = ItemThrowableType.ROCKET;
        public ItemThrowableCategory activeItemThrowableCategory = ItemThrowableCategory.ROCKET;
        
        public bool isInAir;
        public bool isInWater;
        public bool isGrabingMover;

        public float health;
        public float resistance;
        public bool hasMover;
        public AdvancementType advancementType = AdvancementType.FIRST_MINICHIMP_INTERACT;

        private void Start() {
            tpsPlayerAnimator = GetComponentInChildren<Animator>().GetComponent<TpsPlayerAnimator>();
        }

        public void GainHealth(InputAction.CallbackContext context) {
            if (activeItemThrowableCategory == ItemThrowableCategory.BANANA && context.performed) {
                if (Inventory.Instance.bananaManInventory[activeItemThrowableType] > 0) {
                    health += activeItem.healthBonus;
                    resistance += activeItem.resistanceBonus;

                    Inventory.Instance.RemoveQuantity(activeItemThrowableType, 1);
            
                    UIVitals.Instance.Set_Health(health);
                    UIVitals.Instance.Set_Resistance(resistance);
                }
            }
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