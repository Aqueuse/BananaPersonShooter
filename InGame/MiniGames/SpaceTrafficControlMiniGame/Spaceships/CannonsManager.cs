using Cinemachine;
using InGame.Inventory;
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

        public BananaEffect activeBananaGoop = BananaEffect.ATTRACTION;
        public BananaGoopInventory bananaGoopInventory;
        
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
            activeCannon.SetCannon(activeBananaGoop);
            
            miniGameVirtualCamera.Priority = 100;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
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
    }
}
