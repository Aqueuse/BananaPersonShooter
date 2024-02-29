using System.Linq;
using Cinemachine;
using DG.Tweening;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships;
using TMPro;
using UI;
using UI.InGame.SpaceTrafficControlMiniGame;
using UnityEngine;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame {
    public class SpaceTrafficControlMiniGameManager : MonoBehaviour {
        [SerializeField] private UICanvasFadeInOut debrisUICanvasFadeInOut;
        [SerializeField] private TextMeshProUGUI debrisQuantityText;
        [SerializeField] private SpaceTrafficControlMiniGameUI spaceTrafficControlMiniGameUI;

        [SerializeField] private ProjectilesManager projectilesManager;
        public SpaceshipsSpawner spaceshipsSpawner;
        
        ///////////////////
        [Header("camera")]
        [SerializeField] private CinemachineVirtualCamera miniGameVirtualCamera;
        
        [SerializeField] private Camera miniGameCamera;
        [SerializeField] private Transform miniGameCameraContainer;
        
        [SerializeField] private Transform globalViewCameraTarget;
        [SerializeField] private Transform focusViewCameraTarget;

        [SerializeField] private GenericDictionary<RegionType, Transform> cameraRotationBySceneType;
        
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
        public Ease rotationEase;
        
       ///////////////////
       [Header("game")]
        [SerializeField] private GenericDictionary<RegionType, Transform> cannonsTransformsBySceneType;
        [SerializeField] private RectTransform target;
        [SerializeField] private Transform targetsystemTransform;

        private Vector3 _targetPosition;
        
        private BananaType _bananaType;
        private Transform _activeCannon;
        private RegionType _activeMap;
        
        private int _debrisQuantity;
        
        public void Init() {
            _bananaType = BananaType.CAVENDISH;
            spaceTrafficControlMiniGameUI.RefreshBananasQuantity(_bananaType);
            projectilesManager.Init();
        }
        
        ///  INPUT
        public void ActivateCannon(RegionType regionType) {
            spaceTrafficControlMiniGameUI.ActivateButton(regionType);

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            _activeMap = regionType;
            _activeCannon = cannonsTransformsBySceneType[_activeMap];
            
            miniGameCamera.transform.position = focusViewCameraTarget.position;
            miniGameVirtualCamera.Priority = 100;

            miniGameCameraContainer.transform.DORotate(cameraRotationBySceneType[_activeMap].eulerAngles, 1).SetEase(rotationEase);

            rotationIndex = cameraRotationBySceneType.Keys.ToList().IndexOf(_activeMap);
            
            targetsystemTransform.position = cannonsTransformsBySceneType[_activeMap].position;
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.SPACE_TRAFFIC_CONTROL_MINI_GAME);
        }

        public void StopCannonControl() {
            spaceTrafficControlMiniGameUI.DesactivateButtons();
            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                    
            ObjectsReference.Instance.playerController.canMove = true;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;
            
            miniGameCamera.transform.position = globalViewCameraTarget.position;
            
            miniGameVirtualCamera.Priority = 0;
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
        }

        public void SwitchToLeftCannon() {
            rotationIndex = (rotationIndex - 1) % scenesToRotateBeetween.Length;
            if (rotationIndex < 0) rotationIndex = scenesToRotateBeetween.Length-1;

            _activeMap = scenesToRotateBeetween[rotationIndex];
            
            miniGameCameraContainer.transform.DORotate(cameraRotationBySceneType[_activeMap].eulerAngles, 1).SetEase(rotationEase);
            ActivateCannon(_activeMap);
        }

        public void SwitchToRightCannon() {
            rotationIndex = (rotationIndex + 1) % scenesToRotateBeetween.Length;
            _activeMap = scenesToRotateBeetween[rotationIndex];
            
            miniGameCameraContainer.transform.DORotate(cameraRotationBySceneType[_activeMap].eulerAngles, 1).SetEase(rotationEase);
            ActivateCannon(_activeMap);
        }

        public void SwitchToCannon(RegionType regionType) {
            _activeMap = regionType;
            miniGameCameraContainer.transform.DORotate(cameraRotationBySceneType[_activeMap].eulerAngles, 1).SetEase(rotationEase);
            ActivateCannon(_activeMap);
        }
        
        public void MoveTarget(float xQuantity, float yQuantity) {
            _targetPosition.x += xQuantity / 1000;
            _targetPosition.y += yQuantity / 100;

            // prevent target to escape gamezone
            _targetPosition = _targetPosition.normalized;

            target.localPosition = _targetPosition * 0.5f;

            _activeCannon.transform.LookAt(target, _activeCannon.transform.up);
        }

        public void Shoot() {
            if (ObjectsReference.Instance.bananasInventory.GetQuantity(_bananaType) == 0) {
                projectilesManager.AlertNoBanana();
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.NO_BANANA, 0);
            }

            else {
                projectilesManager.Shoot(_activeMap);
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.CANNON_SHOOT, 0);
                ObjectsReference.Instance.bananasInventory.RemoveQuantity(_bananaType, 1);
            }
        }
        
        /// GAME
        public void AddNewSpaceship(string spaceshipName) {
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.LAUNCH_SIGNAL, 0.1f);

            spaceshipsSpawner.ShowDistantDot(spaceshipName);
        }
        
        public void AddPirateDebris() {
            _debrisQuantity += 1;

            ObjectsReference.Instance.gameData.worldData.piratesDebrisToSpawn += 1;
            ObjectsReference.Instance.gameData.worldData.debrisToSPawnByCharacterType.Add(CharacterType.PIRATE);

            debrisQuantityText.text = _debrisQuantity.ToString();
            debrisUICanvasFadeInOut.enabled = true;
        }

        public void AddVisitorDebris() {
            _debrisQuantity += 1;
            
            ObjectsReference.Instance.gameData.worldData.visitorsDebrisToSpawn += 1;
            ObjectsReference.Instance.gameData.worldData.debrisToSPawnByCharacterType.Add(CharacterType.VISITOR);

            debrisQuantityText.text = _debrisQuantity.ToString();
            debrisUICanvasFadeInOut.enabled = true;
        }

        public void AddMerchantDebris() {
            _debrisQuantity += 1;
            
            ObjectsReference.Instance.gameData.worldData.visitorsDebrisToSpawn += 1;
            ObjectsReference.Instance.gameData.worldData.debrisToSPawnByCharacterType.Add(CharacterType.VISITOR);

            debrisQuantityText.text = _debrisQuantity.ToString();
            debrisUICanvasFadeInOut.enabled = true;
        }
    }
}
