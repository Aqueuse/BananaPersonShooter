using Enums;
using Player;
using UnityEngine;

namespace Building {
    public class BananaGun : MonoBehaviour {
        public GameObject bananaGun;
        public GameObject bananaGunInBack;
        
        private PlayerController _playerController;

        public bool wasFocus;
        
        private void Start() {
            _playerController = ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>();
            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = false;
        }
        
        public void GrabBananaGun() {
            bananaGun.SetActive(true);
            bananaGunInBack.SetActive(false);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = true;

            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.GrabBananaGun();
            wasFocus = _playerController.isFocusCamera;
            ObjectsReference.Instance.mainCamera.Switch_To_Shoot_Target();
        }

        public void CancelMover() {
            ObjectsReference.Instance.bananaGunPut.CancelThrow();
            
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = false;

            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.FocusCamera(wasFocus);
            ObjectsReference.Instance.mainCamera.Switch_To_TPS_Target();
        }
    }
}