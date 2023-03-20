using Cameras;
using Enums;
using Game;
using Input;
using Player;
using UnityEngine;

namespace UI.InGame {
    public class UICanvasItemsStatic : MonoBehaviour {
        [SerializeField] private GameObject uIinGame;
        [SerializeField] private bool canRotate;
        
        private bool _isPlayerInZone;

        private void Start() {
            _isPlayerInZone = false;
        }

        private void Update() {
            if (!_isPlayerInZone || !canRotate) return;
            
            var mainCameraPosition = MainCamera.Instance.transform.position;
            transform.LookAt(mainCameraPosition);
        }

        public void ShowUI() {
            if (GameManager.Instance.gameContext != GameContext.IN_DIALOGUE &&
                InputManager.Instance.uiSchemaContext != UISchemaSwitchType.GAME_MENU) {
                uIinGame.SetActive(true);
                _isPlayerInZone = true;
            }
        }

        public void HideUI() {
            uIinGame.SetActive(false);
            _isPlayerInZone = false;
        }
    }
}
