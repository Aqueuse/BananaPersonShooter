using Player;
using UnityEngine;

namespace UI.InGame {
    public class UICanvasItemsStatic : MonoBehaviour {
        [SerializeField] private GameObject uIinGame;
        
        private bool isPlayerInZone;
        
        private void Start() {
            isPlayerInZone = false;
        }

        private void Update() {
            if (!isPlayerInZone) return;
            
            var bananaManPosition = BananaMan.Instance.transform.position;
            uIinGame.transform.LookAt(new Vector3(bananaManPosition.x, transform.position.y, bananaManPosition.z));
        }

        public void ShowUI() {
            uIinGame.SetActive(true);
            isPlayerInZone = true;
        }

        public void HideUI() {
            uIinGame.SetActive(false);
            isPlayerInZone = false;
        }
    }
}
