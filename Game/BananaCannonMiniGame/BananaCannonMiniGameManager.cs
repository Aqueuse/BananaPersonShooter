using Cinemachine;
using Enums;
using Items.ItemsActions;
using TMPro;
using UI.InGame;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class BananaCannonMiniGameManager : MonoSingleton<BananaCannonMiniGameManager> {
        [SerializeField] private CinemachineVirtualCamera bananaCannonVirtualCamera;
        [SerializeField] private ProjectilesManager projectilesManager;
        [SerializeField] private ItemInteraction playItemInteraction;        
        
        public string _mapName = "MAP01";

        public SpaceshipsSpawner spaceshipsSpawner;
        
        ///////////////////

        [SerializeField] private TextMeshProUGUI debrisQuantityText;
        [SerializeField] private TextMeshProUGUI bottomBananasQuantityText;
        
        public TextMeshProUGUI spaceshipQuantityText;

        [SerializeField] private Transform cannon;
        [SerializeField] private RectTransform target;

        private Vector3 _targetPosition;
        
        private int _debrisQuantity;
        private int _debrisToSpawn;
        private int _spaceshipsQuantity;

        private ItemType _bananaType;

        private void Start() {
            _bananaType = ItemType.CAVENDISH;
            bottomBananasQuantityText.text = ObjectsReference.Instance.inventory.GetQuantity(ItemType.CAVENDISH).ToString();
            ObjectsReference.Instance.uIbananaCannonMiniGame.playButtonBananasQuantityText.text = ObjectsReference.Instance.inventory.GetQuantity(ItemType.CAVENDISH).ToString();
            
            _debrisQuantity = ObjectsReference.Instance.mapsManager.mapBySceneName[_mapName].GetDebrisQuantity();
            debrisQuantityText.text = _debrisQuantity.ToString();
            _debrisToSpawn = _debrisQuantity;

            _spaceshipsQuantity = Random.Range(1, 7);
            spaceshipQuantityText.text = _spaceshipsQuantity.ToString();
        }
        
        public void SwitchToMiniGame() {
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.BANANA_CANNON_MINI_GAME;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            playItemInteraction.HideUI();

            bananaCannonVirtualCamera.Priority = 20;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.uIbananaCannonMiniGame.ShowGameUI();
            ObjectsReference.Instance.uIbananaCannonMiniGame.ShowStartMenu();
        }

        public void PlayMiniGame() {
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_MINI_GAME;

            ObjectsReference.Instance.uIbananaCannonMiniGame.HideStartMenu();
            
            spaceshipsSpawner.SpawnSpaceships(_spaceshipsQuantity);
        }

        public void Teleport() {
            GetComponent<PortalDestinationItemAction>().Activate();
        }

        public static void PauseMiniGame() {
            Time.timeScale = 0;

            ObjectsReference.Instance.uIbananaCannonMiniGame.ShowPauseMenu();
        }

        public static void UnpauseMiniGame() {
            Time.timeScale = 1;

            ObjectsReference.Instance.uIbananaCannonMiniGame.HidePauseMenu();
        }

        public void QuitMiniGame() {
            Time.timeScale = 1;

            playItemInteraction.ShowUI();
            bananaCannonVirtualCamera.Priority = 1;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            ObjectsReference.Instance.uIbananaCannonMiniGame.HideStartMenu();
            ObjectsReference.Instance.uIbananaCannonMiniGame.HidePauseMenu();
            ObjectsReference.Instance.uIbananaCannonMiniGame.HideGameUI();
            
            ObjectsReference.Instance.mapsManager.mapBySceneName[_mapName].debrisToSpawn = _debrisToSpawn;

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
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
            if (ObjectsReference.Instance.inventory.GetQuantity(_bananaType) == 0) {
                projectilesManager.AlertNoBanana();
                ObjectsReference.Instance.audioManager.PlayEffect(EffectType.NO_BANANA, 0);
            }

            if (ObjectsReference.Instance.inventory.GetQuantity(_bananaType) > 0) {
                projectilesManager.Shoot();
                ObjectsReference.Instance.audioManager.PlayEffect(EffectType.CANNON_SHOOT, 0);
                ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.BANANA, _bananaType, 1);
            }
        }

        public void RefreshDebrisQuantity() {
            _debrisToSpawn += 1;
            ObjectsReference.Instance.mapsManager.mapBySceneName[_mapName].debrisToSpawn = _debrisToSpawn;

            debrisQuantityText.text = (_debrisQuantity+_debrisToSpawn).ToString();
        }

        public void DecrementeSpaceshipQuantity() {
            _spaceshipsQuantity -= 1;
            spaceshipQuantityText.text = _spaceshipsQuantity.ToString();
        }
    }
}
