using UnityEngine;

namespace InGame.Player {
    public class BananaGun : MonoBehaviour {
        [SerializeField] private Transform bananaGunTargetTransform;

        public GameObject bananaGun;
        public GameObject bananaGunInBack;
        public bool wasFocus;
        
        private Vector3 bananaManRotation;
        private Vector3 targetPosition;
        
        private PlayerController _playerController;
        
        private void Start() {
            _playerController = ObjectsReference.Instance.playerController;
        }

        private void Update() {
            if (!ObjectsReference.Instance.bananaMan.isGrabingBananaGun) return;
            
            bananaGun.gameObject.transform.LookAt(bananaGunTargetTransform, Vector3.up);

            targetPosition = bananaGunTargetTransform.position;

            bananaManRotation.x = targetPosition.x;
            bananaManRotation.y = ObjectsReference.Instance.bananaMan.transform.position.y;
            bananaManRotation.z = targetPosition.z;

            ObjectsReference.Instance.bananaMan.transform.LookAt(bananaManRotation, Vector3.up);
        }

        public void GrabBananaGun() {
            bananaGun.SetActive(true);
            bananaGunInBack.SetActive(false);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = true;
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.GrabBananaGun();
            ObjectsReference.Instance.bananaMan.GetComponent<PlayerIK>().SetAimConstraint(true);
            wasFocus = _playerController.isFocusCamera;
            
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.CAVENDISH);
        }
        
        public void UngrabBananaGun() {
            ObjectsReference.Instance.throwBanana.CancelThrow();
            
            bananaGun.SetActive(false);
            bananaGunInBack.SetActive(true);

            ObjectsReference.Instance.bananaMan.isGrabingBananaGun = false;
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.FocusCamera(wasFocus);
            ObjectsReference.Instance.bananaMan.GetComponent<PlayerIK>().SetAimConstraint(false);
            
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);
        }
    }
}