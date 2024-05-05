using KeyboardInputs;
using SharedInputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Player.BananaGunActions {
    public class BananaGunActionsSwitch : MonoBehaviour {
        [SerializeField] private BananaGunShootActions bananaGunShootActions;
        [SerializeField] private BananaGunScanActions bananaGunScanActions;
        [SerializeField] private BananaGunBuildActions bananaGunBuildActions;
        
        [SerializeField] private InputActionReference shootActionReference;
        [SerializeField] private InputActionReference scanObjectActionReference;
        [SerializeField] private InputActionReference buildActionReference;

        private Shoot shoot;
        private Scan scan;
        private Build build;

        private void Start() {
            shoot = ObjectsReference.Instance.shoot;
            scan = ObjectsReference.Instance.scan;
            build = ObjectsReference.Instance.build;
        }

        private void OnEnable() {
            shootActionReference.action.Enable();
            shootActionReference.action.performed += SwitchToShootMode;
            shootActionReference.action.canceled += SwitchToShootMode;

            scanObjectActionReference.action.Enable();
            scanObjectActionReference.action.performed += SwitchToScanMode;
            scanObjectActionReference.action.canceled += SwitchToScanMode;

            buildActionReference.action.Enable();
            buildActionReference.action.performed += SwitchToBuildMode;
            buildActionReference.action.canceled += SwitchToBuildMode;
        }

        private void OnDisable() {
            shootActionReference.action.Disable();
            shootActionReference.action.performed -= SwitchToShootMode;
            shootActionReference.action.canceled -= SwitchToShootMode;

            scanObjectActionReference.action.Disable();
            scanObjectActionReference.action.performed -= SwitchToScanMode;
            scanObjectActionReference.action.canceled -= SwitchToScanMode;

            buildActionReference.action.Disable();
            buildActionReference.action.performed -= SwitchToBuildMode;
            buildActionReference.action.canceled -= SwitchToBuildMode;
        }

        public void SwitchToBananaGunMode(BananaGunMode bananaGunMode) {
            ObjectsReference.Instance.bananaMan.bananaGunMode = bananaGunMode;

            switch (bananaGunMode) {
                case BananaGunMode.SHOOT:
                    bananaGunShootActions.enabled = true;
                    bananaGunScanActions.enabled = false;
                    bananaGunBuildActions.enabled = false;
                    
                    shoot.enabled = true;
                    scan.enabled = false;
                    build.enabled = false;

                    ObjectsReference.Instance.uiFlippers.UpLeftFlipper();
                    break;
                case BananaGunMode.SCAN:
                    bananaGunShootActions.enabled = false;
                    bananaGunScanActions.enabled = true;
                    bananaGunBuildActions.enabled = false;

                    shoot.enabled = false;
                    scan.enabled = false; // do not directly active scan, wait until banana gun is grabbed to raycast
                    build.enabled = false;

                    ObjectsReference.Instance.uiFlippers.UpMiddleFlipper();
                    break;
                case BananaGunMode.BUILD:
                    bananaGunShootActions.enabled = false;
                    bananaGunScanActions.enabled = false;
                    bananaGunBuildActions.enabled = true;

                    shoot.enabled = false;
                    scan.enabled = false;
                    build.enabled = true;
                    
                    ObjectsReference.Instance.uiFlippers.UpRightFlipper();
                    break;
            }
        }

        public void DesactiveBananaGun() {
            bananaGunShootActions.enabled = false;
            bananaGunScanActions.enabled = false;
            bananaGunBuildActions.enabled = false;

            shoot.enabled = false;
            scan.enabled = false;
            build.enabled = false;
        }
        
        private void SwitchToShootMode(InputAction.CallbackContext context) {
            SwitchToBananaGunMode(BananaGunMode.SHOOT);
        }

        private void SwitchToScanMode(InputAction.CallbackContext context) {
            SwitchToBananaGunMode(BananaGunMode.SCAN);
        }

        private void SwitchToBuildMode(InputAction.CallbackContext context) {
            SwitchToBananaGunMode(BananaGunMode.BUILD);
        }
    }
}
