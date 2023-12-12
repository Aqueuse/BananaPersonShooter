using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UI;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class BananaCannonMiniGameManager : MonoSingleton<BananaCannonMiniGameManager> {
        [SerializeField] private CinemachineVirtualCamera bananaCannonVirtualCamera;
        [SerializeField] private ProjectilesManager projectilesManager;
        [SerializeField] private GameObject playItemInteractionGameObject;

        public SceneType _mapName = SceneType.MAP01;

        public SpaceshipsSpawner spaceshipsSpawner;

       ///////////////////
        [SerializeField] private UICanvasFadeInOut visitorsUICanvasFadeInOut;
        [SerializeField] private TextMeshProUGUI visitorsInCorolleText;

        [SerializeField] private UICanvasFadeInOut piratesUICanvasFadeInOut;
        [SerializeField] private TextMeshProUGUI piratesInCorolleText;
        
        [SerializeField] private UICanvasFadeInOut debrisUICanvasFadeInOut;
        [SerializeField] private TextMeshProUGUI debrisQuantityText;
        
        [SerializeField] private Transform cannon;
        [SerializeField] private RectTransform target;

        private Vector3 _targetPosition;
        
        private int _debrisQuantity;
        public int _spaceshipsQuantity;
        
        public int piratesInCorolleQuantity;
        public int visitorsInCorolleQuantity;

        private BananaType _bananaType;

        [SerializeField] private UIbananaCannonMiniGame uIbananaCannonMiniGame;
        
        private void Start() {
            _bananaType = BananaType.CAVENDISH;
            uIbananaCannonMiniGame.RefreshBananasQuantity(_bananaType);
        }
        
        public void PlayMiniGame() {
            _bananaType = ObjectsReference.Instance.bananaMan.activeItem.bananaType;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.BANANA_CANNON_MINI_GAME);

            playItemInteractionGameObject.SetActive(false);

            bananaCannonVirtualCamera.Priority = 20;
            
            uIbananaCannonMiniGame.RefreshBananasQuantity(_bananaType);
            
            uIbananaCannonMiniGame.ShowGameUI();

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_MINI_GAME;
        }
        
        public void QuitMiniGame() {
            Time.timeScale = 1;

            playItemInteractionGameObject.SetActive(true);
            bananaCannonVirtualCamera.Priority = 1;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            uIbananaCannonMiniGame.HideGameUI();
            
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
        }

        ///  MINIGAME

        public void AddNewWave(List<CharacterType> spaceshipsToSpawn) {
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.LAUNCH_SIGNAL, 0.1f);

            _spaceshipsQuantity = spaceshipsToSpawn.Count;
            spaceshipsSpawner.SpawnSpaceships(spaceshipsToSpawn);
        }
        
        public void MoveTarget(float xQuantity, float yQuantity) {
            _targetPosition = target.localPosition;

            _targetPosition.x += xQuantity/10;
            _targetPosition.y += yQuantity/10;

            // prevent target to escape gamezone
            _targetPosition = _targetPosition.normalized;

            target.localPosition = _targetPosition * 0.5f;

            cannon.transform.LookAt(target, cannon.transform.up);
        }

        public void Shoot() {
            if (ObjectsReference.Instance.bananasInventory.GetQuantity(_bananaType) == 0) {
                projectilesManager.AlertNoBanana();
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.NO_BANANA, 0);
            }

            if (ObjectsReference.Instance.bananasInventory.GetQuantity(_bananaType) > 0) {
                projectilesManager.Shoot();
                ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.CANNON_SHOOT, 0);
                ObjectsReference.Instance.bananasInventory.RemoveQuantity(_bananaType, 1);
            }
        }

        public void AddPirateDebris() {
            _debrisQuantity += 1;

            ObjectsReference.Instance.gameData.mapBySceneName[_mapName].piratesDebrisToSpawn += 1;
            ObjectsReference.Instance.gameData.mapBySceneName[_mapName].debrisToSPawnByCharacterType.Add(CharacterType.PIRATE);

            debrisQuantityText.text = _debrisQuantity.ToString();
            debrisUICanvasFadeInOut.enabled = true;
        }

        public void AddVisitorDebris() {
            _debrisQuantity += 1;
            
            ObjectsReference.Instance.gameData.mapBySceneName[_mapName].visitorsDebrisToSpawn += 1;
            ObjectsReference.Instance.gameData.mapBySceneName[_mapName].debrisToSPawnByCharacterType.Add(CharacterType.VISITOR);

            debrisQuantityText.text = _debrisQuantity.ToString();
            debrisUICanvasFadeInOut.enabled = true;
        }

        public void AddVisitor() {
            visitorsInCorolleQuantity += 1;
            visitorsInCorolleText.text = visitorsInCorolleQuantity.ToString();

            visitorsUICanvasFadeInOut.enabled = true;
        }
        
        public void AddPirate() {
            piratesInCorolleQuantity += 1;
            piratesInCorolleText.text = piratesInCorolleQuantity.ToString();

            piratesUICanvasFadeInOut.enabled = true;
        }
    }
}
