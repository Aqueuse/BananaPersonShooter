using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControl.Spaceships {
    public class CannonsManager : MonoBehaviour {
        [Header("mini game camera")]
        [SerializeField] private Transform cameraCannonTransform;
        [SerializeField] private Camera cameraCannon;

        [Header("game")]
        public GenericDictionary<RegionType, Cannon> cannonsByRegionType;
        
        public RegionType activeCannonRegion = RegionType.MAP01;
        [HideInInspector] public Cannon activeCannon;

        public int bananaGoopQuantity;
        
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
            cannonsByRegionType[activeCannonRegion].ShowLaser();
        }

        public void HideLaserOnActivatedRegion() {
            cannonsByRegionType[activeCannonRegion].HideLaser();
        }
        
        public void ActivateCannon(RegionType regionType) {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
            
            activeCannonRegion = regionType;
            activeCannon = cannonsByRegionType[activeCannonRegion];
            
            activeCannon.PositionneCamera(cameraCannonTransform);
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
        }

        public void SwitchToLeftCannon() {
            rotationIndex = (rotationIndex - 1) % scenesToRotateBeetween.Length;
            if (rotationIndex < 0) rotationIndex = scenesToRotateBeetween.Length-1;

            activeCannonRegion = scenesToRotateBeetween[rotationIndex];

            ActivateCannon(activeCannonRegion);
        }

        public void SwitchToRightCannon() {
            rotationIndex = (rotationIndex + 1) % scenesToRotateBeetween.Length;
            activeCannonRegion = scenesToRotateBeetween[rotationIndex];
            
            ActivateCannon(activeCannonRegion);
        }

        public void SwitchToCannon(RegionType regionType) {
            activeCannonRegion = regionType;
            ActivateCannon(activeCannonRegion);
        }

        public void SwitchToLastCannon() {
            SwitchToCannon(activeCannonRegion);
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
