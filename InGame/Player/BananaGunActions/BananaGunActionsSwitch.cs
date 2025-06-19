using System.Collections.Generic;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class BananaGunActionsSwitch : MonoBehaviour {
        [SerializeField] private GenericDictionary<BananaGunMode, List<MonoBehaviour>> actionsByBananaGunMode;
        
        public void SwitchToBananaGunMode(BananaGunMode bananaGunMode) {
            foreach (var action in actionsByBananaGunMode[ObjectsReference.Instance.bananaMan.bananaGunMode]) {
                action.enabled = false;
            }
            
            ObjectsReference.Instance.bananaMan.bananaGunMode = bananaGunMode;

            foreach (var action in actionsByBananaGunMode[bananaGunMode]) {
                action.enabled = true;
            }
        }

        public void DesactiveBananaGun() {
            foreach (var action in actionsByBananaGunMode[ObjectsReference.Instance.bananaMan.bananaGunMode]) {
                action.enabled = false;
            }
        }
    }
}
