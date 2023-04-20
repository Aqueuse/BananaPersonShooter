using Enums;
using UnityEngine;

namespace Input {
    public class ShootGameActions : MonoBehaviour {
        public bool rightTriggerActivated;
        
        private void Start() {
            rightTriggerActivated = false;
        }

        private void Update() {
            Shoot();
        }
        
        private void Shoot() {
            if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_BANANAGUN)) return;

            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) {
                    ObjectsReference.Instance.bananaGunPut.LoadingGun();
                    ObjectsReference.Instance.gameActions.leftClickActivated = true;
                }

                if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0)) {
                    ObjectsReference.Instance.bananaGun.CancelMover();
                    ObjectsReference.Instance.gameActions.leftClickActivated = false;
                }

                if (UnityEngine.Input.GetAxis("RightTrigger") != 0 && !rightTriggerActivated) {
                    rightTriggerActivated = true;
                    ObjectsReference.Instance.bananaGunPut.LoadingGun();
                }

                if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated)  {
                    ObjectsReference.Instance.bananaGun.CancelMover();
                    rightTriggerActivated = false;
                }
            }
        }
    }
}
