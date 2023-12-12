using Game.CommandRoomPanelControls;
using Tags;
using UnityEngine;

namespace Game {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private Color penumbraAmbientLightColor;
        
        public void StartTutorial() {
            ObjectsReference.Instance.gameReset.ResetGameData();
            
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);

            ObjectsReference.Instance.cinematiques.Play(CinematiqueType.NEW_GAME);

            ObjectsReference.Instance.uInventoriesManager.HideUIHelpers();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.QUICKSLOTS, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANAGUN_HELPER_KEYBOARD, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANAGUN_HELPER_GAMEPAD, false);
            
            ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(false);

            ObjectsReference.Instance.bananaMan.tutorialFinished = false;
            
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.GESTION);
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.JOURNAL);
            CommandRoomControlPanelsManager.Instance.ShowPanel(CommandRoomPanelType.BANANA_CANNON);

            foreach (var accessManagedGameObject in TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.ACCESS_MANAGED)) {
                accessManagedGameObject.GetComponent<ManageAccess>().ForbidUsage();
            }

            CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.SetBubbleDialogue(dialogueSet.REPAIR_BANANA_GUN);
            CommandRoomControlPanelsManager.Instance.chimployeeCommandRoom.SetTutorialChimployeeConfiguration();

            RenderSettings.ambientLight = penumbraAmbientLightColor;
            
            Invoke(nameof(SaveInitialState), 20);
        }

        private void SaveInitialState() {
            ObjectsReference.Instance.uiSave.CreateNewSave();
        }
        
        public void FinishTutorial() {
            ObjectsReference.Instance.uInventoriesManager.ShowCurrentUIHelper();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.QUICKSLOTS, true);

            ObjectsReference.Instance.bananaGun.bananaGunInBack.SetActive(true);
            // TODO : animation take banana gun

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1f;
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);

            ObjectsReference.Instance.gameLoad.LoadBananasInventory();
            ObjectsReference.Instance.gameLoad.LoadRawMaterialsInventory();
            ObjectsReference.Instance.gameLoad.LoadIngredientsInventory();
            
            ObjectsReference.Instance.bananaMan.tutorialFinished = true;
            
            RenderSettings.ambientLight = Color.white; 
            
            ObjectsReference.Instance.audioManager.SetMusiqueAndAmbianceBySceneName(SceneType.COROLLE);
            
            foreach (var accessManagedGameObject in TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.ACCESS_MANAGED)) {
                accessManagedGameObject.GetComponent<ManageAccess>().AuthorizeUsage();
            }
        }
    }
}
