using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class BananaGunInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            interactedGameObject.transform.parent.gameObject.SetActive(false);
            
            ObjectsReference.Instance.commandRoomControlPanelsManager.blueprinter.CreateBlueprint(
                new [] {
                    BuildableType.TELEPORTER_COROLLE, 
                    BuildableType.TELEPORTER_HANGARS, 
                    BuildableType.TELEPORTER_COMMAND_ROOM
                }
            );

            ObjectsReference.Instance.uInventoriesManager.ShowCurrentUIHelper();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);
            
            ObjectsReference.Instance.bananaGunActionsSwitch.enabled = true;
            
            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceByRegion(RegionType.COROLLE);
            
            ObjectsReference.Instance.gameSave.StartAutoSave();

            ObjectsReference.Instance.bananaGun.GrabBananaGun();

            ObjectsReference.Instance.bottomSlots.Init();
        }
    }
}
