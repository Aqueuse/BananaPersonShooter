using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class Cannon : MonoBehaviour {
        [SerializeField] private Transform socleTransform;
        [SerializeField] private Transform cannonTransform;
        [SerializeField] private Transform launcherTransform;
        public Transform cameraTargetTransform;
        
        private Vector3 socleRotation;
        private Vector3 cannonRotation;
    
        private BananaEffect activeBananaGoop;
        private Color _laserColor;
        public Laser _laser;
        
        public void Rotate(float x, float y) {
            socleRotation.y = x;
            cannonRotation.x -= y;
            
            cannonRotation.x = Mathf.Clamp(
                cannonRotation.x,
                ObjectsReference.Instance.cannonsManager.minRotationY,
                ObjectsReference.Instance.cannonsManager.maxRotationY
            );

            cannonTransform.localRotation = Quaternion.Euler(cannonRotation);

            socleTransform.Rotate(socleRotation * ObjectsReference.Instance.cannonsManager.GetCameraCannonFieldOfView()/20);
        }
        
        public void ShowLaser() {
            _laser.gameObject.SetActive(true);
    
            _laser.SetColor(_laserColor);
            _laser.bananaEffect = activeBananaGoop; 
            
            InvokeRepeating(nameof(ConsumeBanana), 0, 2);
        }

        private void ConsumeBanana() {
            ObjectsReference.Instance.cannonsManager.bananaGoopInventory.RemoveQuantity(activeBananaGoop, 1);
            ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
        }

        public void HideLaser() {
            _laser.gameObject.SetActive(false);
            
            CancelInvoke(nameof(ConsumeBanana));
        }

        public void SetCannon(BananaEffect bananaEffect) {
            activeBananaGoop = bananaEffect;
            _laserColor = ObjectsReference.Instance.meshReferenceScriptableObject.bananaGoopColorByEffectType[activeBananaGoop];
        }

        public void PositionneCamera(Transform cameraTransform) {
            cameraTransform.SetParent(cannonTransform);
            cameraTransform.position = cameraTargetTransform.position;
            cameraTransform.rotation = cameraTargetTransform.rotation;
        }
    }
}
