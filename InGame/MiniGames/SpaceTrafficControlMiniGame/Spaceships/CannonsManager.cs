using Cinemachine;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class CannonsManager : MonoBehaviour {
        [Header("mini game camera")]
        [SerializeField] private CinemachineVirtualCamera miniGameVirtualCamera;
        [SerializeField] private Transform cameraCannonTransform;
        [SerializeField] private Camera cameraCannon;

        [Header("game")]
        [SerializeField] private GenericDictionary<RegionType, Cannon> cannonsByRegionType;

        private RegionType _activeRegion = RegionType.MAP01;
        [HideInInspector] public Cannon activeCannon;
        public BananaType activeBananaType;
        
        public float minRotationY;
        public float maxRotationY;

        ///////////////////

        private readonly RegionType[] scenesToRotateBeetween = {
            RegionType.MAP01,
            RegionType.MAP02,
            RegionType.MAP03,
            RegionType.MAP04,
            RegionType.MAP05,
            RegionType.MAP06,
            RegionType.MAP07,
            RegionType.MAP08
        };

        private int rotationIndex;
    
        private Vector3 _targetPosition;
        
        public void ShowLaserOnActivatedRegion() {
            cannonsByRegionType[_activeRegion].ShowLaser();
        }
        
        public void HideLaserOnActivatedRegion() {
            cannonsByRegionType[_activeRegion].HideLaser();
        }
        
        private void ActivateCannon(RegionType regionType) {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
            
            _activeRegion = regionType;
            activeCannon = cannonsByRegionType[_activeRegion];
            
            activeCannon.PositionneCamera(cameraCannonTransform);
            
            miniGameVirtualCamera.Priority = 100;
            
            activeCannon.SetProjectileType(ObjectsReference.Instance.bananaMan.bananaManData.activeBanana);
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void SwitchToLeftCannon() {
            rotationIndex = (rotationIndex - 1) % scenesToRotateBeetween.Length;
            if (rotationIndex < 0) rotationIndex = scenesToRotateBeetween.Length-1;

            _activeRegion = scenesToRotateBeetween[rotationIndex];

            ActivateCannon(_activeRegion);
        }

        public void SwitchToRightCannon() {
            rotationIndex = (rotationIndex + 1) % scenesToRotateBeetween.Length;
            _activeRegion = scenesToRotateBeetween[rotationIndex];
            
            ActivateCannon(_activeRegion);
        }

        public void SwitchToCannon(RegionType regionType) {
            _activeRegion = regionType;
            ActivateCannon(_activeRegion);
        }

        public void SwitchToLastCannon() {
            var activeBanana = ObjectsReference.Instance.bananaMan.bananaManData.activeBanana;
            activeBananaType = activeBanana.bananaType;
            
            ObjectsReference.Instance.uiCannons.SetBananaType(
                activeBanana.itemSprite, 
                ObjectsReference.Instance.meshReferenceScriptableObject.bananaGoopColorByEffectType[activeBanana.bananaEffect]
            );
            
            ObjectsReference.Instance.uiCannons.SetBananaQuantity(
                ObjectsReference.Instance.bananasInventory.GetQuantity(activeBananaType),
                activeBananaType
            );
            
            SwitchToCannon(_activeRegion);
        }

        public void UnfocusCannons() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        public void ZoomCamera(float zoomLevel) {
            if (zoomLevel > 0) {
                cameraCannon.fieldOfView -= 10;
            }

            if (zoomLevel < 0) {
                cameraCannon.fieldOfView += 10;
            }

            if (cameraCannon.fieldOfView <= 20) {
                cameraCannon.fieldOfView = 20;
            }

            if (cameraCannon.fieldOfView >= 80) {
                cameraCannon.fieldOfView = 80;
            }
        }

        public float GetCameraCannonFieldOfView() {
            return cameraCannon.fieldOfView;
        }

        public Color GetColorByBananaEffect(BananaEffect bananaEffect) {
            return ObjectsReference.Instance.meshReferenceScriptableObject.bananaGoopColorByEffectType[bananaEffect];
        }
    }
}