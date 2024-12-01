using SharedInputs;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class BananaGunActionsSwitch : MonoBehaviour {
        [SerializeField] private BananaGunShootActions bananaGunShootActions;
        [SerializeField] private BananaGunScanActions bananaGunScanActions;
        [SerializeField] private BananaGunBuildActions bananaGunBuildActions;
        
        private Shoot shoot;
        private Scan scan;
        private Build build;

        private BananaMan bananaMan;

        private void Start() {
            shoot = ObjectsReference.Instance.shoot;
            scan = ObjectsReference.Instance.scan;
            build = ObjectsReference.Instance.build;

            bananaMan = ObjectsReference.Instance.bananaMan;
        }
        
        public void SwitchToBananaGunMode(BananaGunMode bananaGunMode) {
            ObjectsReference.Instance.bananaMan.bananaGunMode = bananaGunMode;

            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            ObjectsReference.Instance.build.CancelGhost();
            
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

        public void SwitchToLeftMode() {
            switch (bananaMan.bananaGunMode) {
                case BananaGunMode.BUILD:
                    SwitchToBananaGunMode(BananaGunMode.SCAN);
                    break;
                case BananaGunMode.SCAN:
                    SwitchToBananaGunMode(BananaGunMode.SHOOT);
                    break;
                case BananaGunMode.SHOOT:
                    SwitchToBananaGunMode(BananaGunMode.BUILD);
                    break;
                case BananaGunMode.IDLE:
                    SwitchToBananaGunMode(BananaGunMode.SHOOT);
                    break;
            }
        }
        
        public void SwitchToRightMode() {
            switch (bananaMan.bananaGunMode) {
                case BananaGunMode.BUILD:
                    SwitchToBananaGunMode(BananaGunMode.SHOOT);
                    break;
                case BananaGunMode.SHOOT:
                    SwitchToBananaGunMode(BananaGunMode.SCAN);
                    break;
                case BananaGunMode.SCAN:
                    SwitchToBananaGunMode(BananaGunMode.BUILD);
                    break;
                case BananaGunMode.IDLE:
                    SwitchToBananaGunMode(BananaGunMode.SHOOT);
                    break;
            }
        }
    }
}
