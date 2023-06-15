using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InGame {
    public enum InterfaceContext {
        INVENTORY,
        BLUEPRINTS,
        CHIMPLOYEE
    }

    public class Uihud : MonoBehaviour {
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject blueprintsButton;
        [SerializeField] private GameObject chimployeeButton;
    
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        
        public InterfaceContext interfaceContext;
        private CanvasGroup chimployeeCanvasGroup;

        [SerializeField] private GameObject firstInventoryItem;
        [SerializeField] private GameObject firstBlueprintItem;
        
        private void Start() {
            interfaceContext = InterfaceContext.INVENTORY;
            chimployeeCanvasGroup = chimployeeButton.GetComponent<CanvasGroup>();
        }

        public void Switch_To_Left_Tab() {
            switch (interfaceContext) {
                case InterfaceContext.INVENTORY:
                    if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_MONKEYMAN_IA)) Switch_To_Chimployee();
                    else {
                        Switch_To_Blueprints();
                    }
                    break;
                case InterfaceContext.BLUEPRINTS:
                    Switch_To_Inventory();
                    break;
                case InterfaceContext.CHIMPLOYEE:
                    Switch_To_Blueprints();
                    break;
            }
        }

        public void Switch_To_Right_Tab() {
            switch (interfaceContext) {
                case InterfaceContext.INVENTORY:
                    Switch_To_Blueprints();
                    break;
                case InterfaceContext.BLUEPRINTS:
                    if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_MONKEYMAN_IA)) Switch_To_Chimployee();
                    else {
                        Switch_To_Inventory();
                    }
                    break;
                case InterfaceContext.CHIMPLOYEE:
                    Switch_To_Inventory();
                    break;
            }
        }

        public void Switch_To_Inventory() {
            EventSystem.current.SetSelectedGameObject(firstInventoryItem);
            
            interfaceContext = InterfaceContext.INVENTORY;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.CHIMPLOYEE, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, true);

            inventoryButton.GetComponent<Image>().color = activatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

            blueprintsButton.GetComponent<Image>().color = unactivatedColor;
            blueprintsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            chimployeeButton.GetComponent<Image>().color = unactivatedColor;
            chimployeeButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.INVENTAIRE);
        }

        public void Switch_To_Blueprints() {
            EventSystem.current.SetSelectedGameObject(firstBlueprintItem);

            interfaceContext = InterfaceContext.BLUEPRINTS;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.CHIMPLOYEE, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, true);
            
            inventoryButton.GetComponent<Image>().color = unactivatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            blueprintsButton.GetComponent<Image>().color = activatedColor;
            blueprintsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

            chimployeeButton.GetComponent<Image>().color = unactivatedColor;
            chimployeeButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.BLUEPRINTS;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.BLUEPRINTS);
        }

        public void Switch_To_Chimployee() {
            interfaceContext = InterfaceContext.CHIMPLOYEE;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.CHIMPLOYEE, true);

            inventoryButton.GetComponent<Image>().color = unactivatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            
            blueprintsButton.GetComponent<Image>().color = unactivatedColor;
            blueprintsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            chimployeeButton.GetComponent<Image>().color = activatedColor;
            chimployeeButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.CHIMPLOYEE;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.CHIMPLOYEE);
        }

        public void Activate_Chimployee_Tab() {
            chimployeeCanvasGroup.alpha = 1;
            chimployeeCanvasGroup.interactable = true;
            chimployeeCanvasGroup.blocksRaycasts = true;
        }

        public static void AuthorizeTp() {
            ObjectsReference.Instance.uiChimployee.TpButton.SetActive(true);
        }
    }
}
