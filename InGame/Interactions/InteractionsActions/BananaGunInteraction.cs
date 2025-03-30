using Tags;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions {
    public class BananaGunInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
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
            
            ObjectsReference.Instance.bananaMan.tutorialFinished = true;
            
            RenderSettings.ambientLight = Color.white; 
            
            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceByRegion(RegionType.COROLLE);
            
            foreach (var accessManagedGameObject in TagsManager.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.ACCESS_MANAGED)) {
                accessManagedGameObject.GetComponent<ManageAccess>().AuthorizeUsage();
            }

            ObjectsReference.Instance.gameSave.StartAutoSave();

            ObjectsReference.Instance.bananaGun.GrabBananaGun();

            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.SCAN);
            ObjectsReference.Instance.uiFlippers.Init();
        }
    }
}
