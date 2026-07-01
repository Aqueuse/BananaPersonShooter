using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using InGame.MiniGames.SpaceTrafficControl;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class Absorbatron : MonoBehaviour {
        private CannonsManager cannonsManager;
        
        private void Start() {
            cannonsManager = ObjectsReference.Instance.cannonsManager;
            cannonsManager.bananaGoopQuantity += 1000; // TODO : remove before build
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 7) return;

            if (other.gameObject.TryGetComponent<BananaBehaviour>(out var bananaBehaviour)) {
                cannonsManager.bananaGoopQuantity += 1;
                ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
                
                Destroy(other.gameObject);
            }
        }
    }
}
