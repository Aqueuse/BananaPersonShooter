using InGame.MiniGames.SpaceTrafficControl.projectiles;
using InGame.Pools;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControl {
    public class CannonsManager : MonoBehaviour {
        [Header("mini game camera")]
        [SerializeField] private Transform cameraCannonTransform;
        [SerializeField] private Camera cameraCannon;

        [Header("game")]
        public GenericDictionary<RegionType, Cannon> cannonsByRegionType;
        
        public RegionType activeCannonRegion = RegionType.MAP01;
        [HideInInspector] public Cannon activeCannon;

        [SerializeField] private LaserPool laserPool;
        public int bananaGoopQuantity;
        
        public float minRotationY;
        public float maxRotationY;

        [SerializeField] private GameObject aspirationCone;
        [SerializeField] private GameObject spirale;
        [SerializeField] private GameObject mireImage;
        
        ///////////////////

        private readonly RegionType[] regionsToRotateBeetween = {
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

        private GameObject laser;

        public void ShootLaserOnActivatedRegion() {
            laser = laserPool.GetPooledLaser();

            laser.transform.position = cannonsByRegionType[activeCannonRegion].launcherTransform.position;
            laser.transform.rotation = cannonsByRegionType[activeCannonRegion].launcherTransform.rotation;
            laser.GetComponent<Laser>().enabled = true;
            
            ObjectsReference.Instance.cannonsManager.bananaGoopQuantity -= 1;
            ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
        }
        
        public void AspireDebris() {
            spirale.SetActive(true);
            aspirationCone.SetActive(true);
        }

        public void StopAspireDebris() {
            spirale.SetActive(false);
            aspirationCone.SetActive(false);
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
            rotationIndex = (rotationIndex - 1) % regionsToRotateBeetween.Length;
            if (rotationIndex < 0) rotationIndex = regionsToRotateBeetween.Length-1;

            activeCannonRegion = regionsToRotateBeetween[rotationIndex];

            ActivateCannon(activeCannonRegion);
        }

        public void SwitchToRightCannon() {
            rotationIndex = (rotationIndex + 1) % regionsToRotateBeetween.Length;
            activeCannonRegion = regionsToRotateBeetween[rotationIndex];
            
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
