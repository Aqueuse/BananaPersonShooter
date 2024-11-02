using InGame.Items.ItemsProperties.Bananas;
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
    
        private BananaType _laserType;
        private Color _laserColor;
        public Laser _laser;

        private BananasPropertiesScriptableObject activeBanana;

        public Transform attractionPointTransform;
        
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
            activeBanana = ObjectsReference.Instance.meshReferenceScriptableObject
                .bananasPropertiesScriptableObjects[_laserType];

            var bananaEffects = activeBanana.bananaEffects;

            _laser.gameObject.SetActive(true);

            _laser.SetColor(_laserColor);
            _laser.bananaEffects = bananaEffects; 
            
            InvokeRepeating(nameof(ConsumeBanana), 0, 2);
        }

        private void ConsumeBanana() {
            ObjectsReference.Instance.bananasInventory.RemoveQuantity(_laserType, 1);

            ObjectsReference.Instance.uiCannons.SetBananaQuantity(
                ObjectsReference.Instance.bananasInventory.GetQuantity(_laserType),
                _laserType
            );
        }

        public void HideLaser() {
            _laser.gameObject.SetActive(false);
            
            CancelInvoke(nameof(ConsumeBanana));
        }

        public void SetProjectileType(BananasPropertiesScriptableObject bananaData) {
            activeBanana = bananaData;
            _laserType = bananaData.bananaType;
            _laserColor = ObjectsReference.Instance.cannonsManager.GetColorByBananaEffects(bananaData.bananaEffects);
        }

        public void PositionneCamera(Transform cameraTransform) {
            cameraTransform.SetParent(cannonTransform);
            cameraTransform.position = cameraTargetTransform.position;
            cameraTransform.rotation = cameraTargetTransform.rotation;
        }
    }
}
