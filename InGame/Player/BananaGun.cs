using UnityEngine;

namespace InGame.Player {
    public class BananaGun : MonoBehaviour {
        [SerializeField] private Transform bananaGunTargetTransform;

        public GameObject bananaGunGameObject;
        
        private Vector3 bananaManRotation;
        private Vector3 targetPosition;
        
        public void GrabBananaGun() {
            bananaGunGameObject.SetActive(true);

            ObjectsReference.Instance.bananaMan.GetComponent<PlayerIK>().SetAimConstraint(true);
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.SetGrabbedBananaGunRigWeight(true);
            
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(true);
        }
        
        public void UngrabBananaGun() {
            bananaGunGameObject.SetActive(false);

            ObjectsReference.Instance.bananaMan.GetComponent<PlayerIK>().SetAimConstraint(false);
            ObjectsReference.Instance.bananaMan.tpsPlayerAnimator.SetGrabbedBananaGunRigWeight(false);
            
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();
            
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(false);
        }
    }
}