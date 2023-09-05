using Enums;
using Game.CommandRoomPanelControls;
using Game.Steam;
using UnityEngine;

namespace Game {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private Color penumbraAmbientLightColor;
        
        public void StartTutorial() {
            ObjectsReference.Instance.gameReset.ResetGameData();
            ObjectsReference.Instance.uiSave.CreateNewSave();

            Cursor.visible = true;  
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0f;
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, BananaType.EMPTY);

            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.CINEMATIQUE;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.cinematiques.Play(CinematiqueType.NEW_GAME);

            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, false);
            ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);

            ObjectsReference.Instance.bananaMan.tutorialFinished = false;

            CommandRoomControlPanelsManager.Instance.HidePanel(CommandRoomPanelType.GESTION);
            CommandRoomControlPanelsManager.Instance.HidePanel(CommandRoomPanelType.JOURNAL);
            CommandRoomControlPanelsManager.Instance.HidePanel(CommandRoomPanelType.BANANA_CANNON);

            CommandRoomControlPanelsManager.Instance.ForbidDoorsAccess();
            CommandRoomControlPanelsManager.Instance.ForbidBananaCannonMiniGameAccess();

            CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.SetBubbleDialogue(dialogueSet.REPAIR_BANANA_GUN);
            CommandRoomControlPanelsManager.Instance.chimployeeCommandRoom.SetInitialChimployeeConfiguration();

            RenderSettings.ambientLight = penumbraAmbientLightColor; 
        }
        
        public void FinishTutorial() {
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.HUD, true);
            ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
            // TODO : animation take banana gun

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, BananaType.EMPTY);

            ObjectsReference.Instance.bananaMan.tutorialFinished = true;
            ObjectsReference.Instance.steamIntegration.UnlockAchievement(SteamAchievement.STEAM_ACHIEVEMENT_BANAGUN_RECONSTRUCTED);
            
            CommandRoomControlPanelsManager.Instance.AuthorizeDoorsAccess();
            CommandRoomControlPanelsManager.Instance.AuthorizeBananaCannonMiniGameAccess();
            
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.GESTION);
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.JOURNAL);
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.BANANA_CANNON);
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.SetBubbleDialogue(dialogueSet.REPAIRED_BANANA_GUN);
            
            CommandRoomControlPanelsManager.Instance.assembler.HideAssemblerActivatedZone();
            CommandRoomControlPanelsManager.Instance.assembler.blueprintsDataInteraction.ShowBlueprintDataIfAvailable();
            
            RenderSettings.ambientLight = Color.white; 
            
            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceBySceneName("COMMANDROOM");
        }
    }
}
