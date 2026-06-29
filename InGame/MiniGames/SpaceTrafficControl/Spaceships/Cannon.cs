using InGame.MiniGames.SpaceTrafficControl.projectiles;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControl.Spaceships {
    public class Cannon : MonoBehaviour {
        [SerializeField] private Transform socleTransform;
        [SerializeField] private Transform cannonTransform;
        [SerializeField] private Transform launcherTransform;

        [SerializeField] private Transform initialCannonTransform;

        public Transform cameraTargetTransform;

        private Vector3 socleRotation;
        private Vector3 cannonRotation;
    
        public Laser _laser;

        public void Rotate(float x, float y) {
            cannonRotation.x -= y;

            cannonRotation.x = Mathf.Clamp(
                cannonRotation.x,
                ObjectsReference.Instance.cannonsManager.minRotationY,
                ObjectsReference.Instance.cannonsManager.maxRotationY
            );

            cannonTransform.localRotation = Quaternion.Euler(cannonRotation);

            socleRotation.y = x;
            socleTransform.Rotate(socleRotation * ObjectsReference.Instance.cannonsManager.GetCameraCannonFieldOfView()/20);
        }

        public void ShowLaser() {
            _laser.gameObject.SetActive(true);
            
            InvokeRepeating(nameof(ConsumeBanana), 0, 2);
        }

        private void ConsumeBanana() {
            ObjectsReference.Instance.cannonsManager.bananaGoopQuantity -= 1;
            ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
        }

        public void HideLaser() {
            _laser.gameObject.SetActive(false);
            
            CancelInvoke(nameof(ConsumeBanana));
        }
        
        public void PositionneCamera(Transform cameraTransform) {
            cameraTransform.SetParent(cannonTransform);
            cameraTransform.position = cameraTargetTransform.position;
            cameraTransform.rotation = cameraTargetTransform.rotation;
        }

        public void SetRotation(float socleYRotation, float cannonXRotation) {
            socleTransform.localRotation = Quaternion.Euler(new Vector3(0f, socleYRotation, 0f));
            cannonTransform.localRotation = Quaternion.Euler(new Vector3(cannonXRotation, 0f, 0f));
        }

        public (float, float) GetRotation() {
            return (socleTransform.localRotation.eulerAngles.y, cannonTransform.localRotation.eulerAngles.x);
        }
        
        public void ResetRotation() {
            socleTransform.localRotation = Quaternion.identity;

            var spaceDirection = socleTransform.position - ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.SPACESHIP_HANGARS].position;
            cannonTransform.rotation = Quaternion.Euler(spaceDirection);
        }
    }
}
