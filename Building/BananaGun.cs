using Cameras;
using Enums;
using Player;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class BananaGun : MonoSingleton<BananaGun> {
        public GameObject bananaGun;
        public GameObject bananaGunInBack;
        
        private PlayerController _playerController;

        public bool wasFocus;
        
        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
            BananaMan.Instance.isGrabingBananaGun = false;
        }
        
        public void GrabBananaGun() {
            bananaGun.SetActive(true);
            bananaGunInBack.SetActive(false);

            BananaMan.Instance.isGrabingBananaGun = true;

            BananaMan.Instance.tpsPlayerAnimator.GrabBananaGun();
            wasFocus = _playerController.isFocusCamera;
            MainCamera.Instance.Switch_To_Shoot_Target();
            
            UICrosshair.Instance.SetCrosshair(ItemThrowableType.CAVENDISH);
        }

        public void CancelMover() {
            BananaGunPut.Instance.CancelThrow();
            
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            BananaMan.Instance.isGrabingBananaGun = false;

            BananaMan.Instance.tpsPlayerAnimator.FocusCamera(wasFocus);
            MainCamera.Instance.Switch_To_TPS_Target();
        }
    }
}