using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Eat : MonoBehaviour {
        public void EatAction(InputAction.CallbackContext context) {
            BananaMan.Instance.health += BananaMan.Instance.activeBanana.healthBonus;
            BananaMan.Instance.resistance += BananaMan.Instance.activeBanana.resistanceBonus;
            
            UIVitals.Instance.Set_Health(BananaMan.Instance.health);
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        }
    }
}
