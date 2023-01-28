using Cameras;
using Player;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class Mover : MonoSingleton<Mover> {
        public GameObject bananaGun;
        [SerializeField] private GameObject bananaGunInBack;
        
        private PlayerController _playerController;

        public bool wasFocus;
        
        private void Start() {
            _playerController = BananaMan.Instance.GetComponent<PlayerController>();
            BananaMan.Instance.isGrabingMover = false;
        }
        
        public void GrabMover() {
            bananaGun.SetActive(true);
            bananaGunInBack.SetActive(false);

            BananaMan.Instance.isGrabingMover = true;

            BananaMan.Instance.tpsPlayerAnimator.GrabMover();
            wasFocus = _playerController.isFocusCamera;
            MainCamera.Instance.Switch_To_Shoot_Target();
            
            UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
        }

        public void CancelMover() {
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            BananaMan.Instance.isGrabingMover = false;

            BananaMan.Instance.tpsPlayerAnimator.FocusCamera(wasFocus);
            MainCamera.Instance.Switch_To_TPS_Target();
            
            UICrosshair.Instance.SetCrosshair(BananaMan.Instance.activeItemThrowableType, BananaMan.Instance.activeItemThrowableCategory);
        }
    }
}