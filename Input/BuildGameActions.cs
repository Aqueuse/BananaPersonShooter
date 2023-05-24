using Enums;
using UnityEngine;

namespace Input {
    public class BuildGameActions : MonoBehaviour {
        private bool _scrolledUp;
        private bool _scrolledDown;
        
        public bool rightTriggerActivated;

        private void Start() {
            rightTriggerActivated = false;
        }

        private void Update() {
            if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_BANANAGUN)) return;
            if (ObjectsReference.Instance.uiManager.Is_Interface_Visible()) return;

            Build();
        }
        
        private void Build() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) {
                    ObjectsReference.Instance.slotSwitch.ValidateBuildable();
                    ObjectsReference.Instance.gameActions.leftClickActivated = true;
                }
                
                if (UnityEngine.Input.GetAxis("RightTrigger") != 0 && !rightTriggerActivated) {
                    rightTriggerActivated = true;
                    ObjectsReference.Instance.slotSwitch.ValidateBuildable();
                    ObjectsReference.Instance.gameActions.leftClickActivated = false;
                }

                if (UnityEngine.Input.GetAxis("RightTrigger") == 0 && rightTriggerActivated) {
                    rightTriggerActivated = false;
                }
            }
        }
    }
}
