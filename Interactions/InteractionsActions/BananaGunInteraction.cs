using Game.CommandRoomPanelControls;
using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananaGunInteraction : MonoBehaviour {
        public static void Activate() {
            CommandRoomControlPanelsManager.Instance.assembler.HideBananaGunInteractableGameObject();
            ObjectsReference.Instance.tutorial.FinishTutorial();
        }
    }
}
