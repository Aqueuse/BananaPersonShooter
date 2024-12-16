using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using InGame.MiniGames.SpaceTrafficControlMiniGame.Spaceships;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class BananaAbsorbatron : MonoBehaviour {
        private CannonsManager cannonsManager;

        private void Start() {
            cannonsManager = ObjectsReference.Instance.cannonsManager;
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 7) return;

            if (other.gameObject.TryGetComponent<BananaBehaviour>(out var bananaBehaviour)) {
                cannonsManager.bananaGoopInventory.AddQuantity(
                    bananaBehaviour.bananasPropertiesScriptableObject.bananaEffect, 1);
                ObjectsReference.Instance.uiCannons.RefreshBananaGoopsQuantity();
                
                Destroy(other.gameObject);
            }
        }
    }
}
