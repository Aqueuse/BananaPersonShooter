using Cinemachine;
using TMPro;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class BananaCannonMiniGameManager : MonoSingleton<BananaCannonMiniGameManager> {
        [SerializeField] private CinemachineVirtualCamera bananaCannonVirtualCamera;
        [SerializeField] private ProjectilesManager projectilesManager;
        public SpaceshipsSpawner spaceshipsSpawner;
        
        public CanvasGroup interactionCanvasGroup;
        
        ///////////////////

        [SerializeField] private TextMeshProUGUI debrisQuantityText;
        [SerializeField] private TextMeshProUGUI bananasQuantityText;
        public TextMeshProUGUI spaceshipQuantityText;

        [SerializeField] private Transform cannon;
        [SerializeField] private RectTransform target;

        private Vector3 _targetPosition;

        private string _mapName = "MAP01";
        [SerializeField] private SpawnPoint spawnPoint;
        
        private int _debrisQuantity;
        private int _debrisToSpawn;
        private int _spaceshipsQuantity;

        private ItemType _bananaType;

        private void Start() {
            _debrisToSpawn = 0;
            _bananaType = ItemType.CAVENDISH;
            bananasQuantityText.text = ObjectsReference.Instance.inventory.GetQuantity(ItemType.CAVENDISH).ToString();
            
            _debrisQuantity = ObjectsReference.Instance.mapsManager.mapBySceneName[_mapName].GetDebrisQuantity();
            debrisQuantityText.text = _debrisQuantity.ToString();
            
            _spaceshipsQuantity = 6;
            spaceshipQuantityText.text = _spaceshipsQuantity.ToString();
        }

        public void SwitchToMiniGame() {
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.BANANA_CANNON_MINI_GAME;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            interactionCanvasGroup.alpha = 0;

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
            ObjectsReference.Instance.scenesSwitch.Teleport(spawnPoint);
        }

        public void PauseMiniGame() {
            Time.timeScale = 0;

            ObjectsReference.Instance.uIbananaCannonMiniGame.ShowPauseMenu();
        }

        public void UnpauseMiniGame() {
            Time.timeScale = 1;

            ObjectsReference.Instance.uIbananaCannonMiniGame.HidePauseMenu();
        }

        public void QuitMiniGame() {
            Time.timeScale = 1;

            interactionCanvasGroup.alpha = 1;
            bananaCannonVirtualCamera.Priority = 1;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

            ObjectsReference.Instance.uIbananaCannonMiniGame.HideStartMenu();
            ObjectsReference.Instance.uIbananaCannonMiniGame.HidePauseMenu();
            ObjectsReference.Instance.uIbananaCannonMiniGame.HideGameUI();

            spaceshipsSpawner.DestroyAllSpaceships();

            ObjectsReference.Instance.mapsManager.mapBySceneName[_mapName].debrisToSpawn = _debrisToSpawn;

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            
            // DEBUG TRICHE
            ObjectsReference.Instance.mapsManager.mapBySceneName[_mapName].debrisToSpawn = 5;
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
            if (ObjectsReference.Instance.inventory.GetQuantity(_bananaType) > 0) {
                projectilesManager.Shoot();
                ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.BANANA, _bananaType, 1);
                bananasQuantityText.text = ObjectsReference.Instance.inventory.GetQuantity(_bananaType).ToString();
            }
        }

        public void RefreshDebrisQuantity(Vector3 position) {
            _debrisToSpawn += 1;
            debrisQuantityText.text = (_debrisQuantity+_debrisToSpawn).ToString();

            Debug.Log("spawn debris on the true map");
        }

        public void DecrementeSpaceshipQuantity() {
            _spaceshipsQuantity -= 1;
            spaceshipQuantityText.text = _spaceshipsQuantity.ToString();

            if (_spaceshipsQuantity == 0) {
                Debug.Log("you win !");
            }
        }
    }
}
