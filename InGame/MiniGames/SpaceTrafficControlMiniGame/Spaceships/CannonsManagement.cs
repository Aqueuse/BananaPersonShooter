using Cinemachine;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships {
    public class CannonsManagement : MonoBehaviour {
        [Header("mini game camera")]
        [SerializeField] private CinemachineVirtualCamera miniGameVirtualCamera;
        [SerializeField] private GenericDictionary<RegionType, Transform> cameraTransformByRegionType;

        [Header("game")]
        [SerializeField] private Transform cannonsCameraTransform;
        [SerializeField] private GenericDictionary<RegionType, Cannon> cannonsByRegionType;

        private Transform _activeCannonTransform;
        private RegionType _activeRegion;
    
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
        
        public void ShootOnActivatedRegion() {
            cannonsByRegionType[_activeRegion].Shoot();
        }
        
        public void ActivateCannon(RegionType regionType) {
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            _activeRegion = regionType;
            _activeCannonTransform = cannonsByRegionType[regionType].transform;

            miniGameVirtualCamera.Priority = 100;

            cannonsCameraTransform.position = cameraTransformByRegionType[_activeRegion].position;
            cannonsCameraTransform.rotation = cameraTransformByRegionType[_activeRegion].rotation;
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.CANNONS);
        }

        public void StopCannonControl() {
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                    
            ObjectsReference.Instance.playerController.canMove = true;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;

            miniGameVirtualCamera.Priority = 0;
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
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

        public void RotateCannon(float xQuantity, float yQuantity) {
            _activeCannonTransform.Rotate(xQuantity, yQuantity, 0);
        }
    }
}
