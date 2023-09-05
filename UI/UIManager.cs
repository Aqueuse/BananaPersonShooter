using Enums;
using UI.Bananapedia;
using UI.Save;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class UIManager : MonoBehaviour {
        [SerializeField] private UIBananapedia uiBananapedia;
        
        public GenericDictionary<UICanvasGroupType, CanvasGroup> canvasGroupsByUICanvasType;
        public bool isOnSubMenus;
        
        private void Start() {
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.HOME_MENU;
            isOnSubMenus = false;
        }

        public void Return_To_Main_Menu() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) Show_game_menu();
            else Show_Home_Menu();
        }

        public void Show_Home_Menu() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, true);
            Set_active(UICanvasGroupType.GAME_MENU, false);

            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.HOME_MENU);
            isOnSubMenus = false;
        }
        
        public void Hide_home_menu() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);

            Set_active(UICanvasGroupType.HOME_MENU, false);
            isOnSubMenus = false;
        }

        public void Show_Load_Menu() {
            Set_active(UICanvasGroupType.LOAD, true);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, false);

            isOnSubMenus = true;
            Hide_Game_Menu();

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                ObjectsReference.Instance.uiSave.newSaveButton.SetActive(true);
                EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiSave.newSaveButton);
            }

            else {
                ObjectsReference.Instance.uiSave.newSaveButton.SetActive(false);
                if (ObjectsReference.Instance.uiSave.GetComponentInChildren<UISaveSlot>() != null)
                    EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiSave.GetComponentsInChildren<UISaveSlot>()[0].gameObject);
            }
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.LOAD);
        }

        public void Show_Options() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, true);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, false);
            
            isOnSubMenus = true;
            Hide_Game_Menu();
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.AUDIOVIDEO_TAB);
            ObjectsReference.Instance.uiOptionsMenu.Switch_To_Audio_Video_Tab();
            
        }

        public void Show_Bananapedia() {
            uiBananapedia.SelectFirstBananapediaEntry();

            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, true);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, false);

            isOnSubMenus = true;
            Hide_Game_Menu();
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.BANANAPEDIA);
        }
        
        public void Show_Credits() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, true);
            Set_active(UICanvasGroupType.HOME_MENU, false);
            
            EventSystem.current.SetSelectedGameObject(ObjectsReference.Instance.uiCredits.scrollbar.gameObject);
            
            isOnSubMenus = true;
            Hide_Game_Menu();
            
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.CREDITS);
        }
        
        public void Show_game_menu() {
            Set_active(UICanvasGroupType.LOAD, false);
            Set_active(UICanvasGroupType.OPTIONS, false);
            Set_active(UICanvasGroupType.BANANAPEDIA, false);
            Set_active(UICanvasGroupType.CREDITS, false);
            Set_active(UICanvasGroupType.HOME_MENU, false);
            
            Set_active(UICanvasGroupType.GAME_MENU, true);
            isOnSubMenus = false;
        }

        public void Hide_Game_Menu() {
            Set_active(UICanvasGroupType.GAME_MENU, false);
        }

        /// IN GAME ///

        public void Show_Interface() {
            Set_active(UICanvasGroupType.INVENTORY_AND_INFORMATION, true);

            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.INVENTORIES;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
            
            ObjectsReference.Instance.uInventoriesManager.Focus_interface();
        }
        
        public void Hide_Interface() {
            Set_active(UICanvasGroupType.INVENTORY_AND_INFORMATION, false);

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            ObjectsReference.Instance.gameManager.UnpauseGame();
        }

        public bool Is_Interface_Visible() {
            return canvasGroupsByUICanvasType[UICanvasGroupType.INVENTORY_AND_INFORMATION].alpha > 0;
        }
        
        public void Set_active(UICanvasGroupType uiCanvasGroupType, bool visible) {
            var canvasGroup = canvasGroupsByUICanvasType[uiCanvasGroupType];
            
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
