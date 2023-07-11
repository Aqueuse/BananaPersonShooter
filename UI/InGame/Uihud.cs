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
        [SerializeField] private Image inventoryButtonImage;
        [SerializeField] private Image blueprintsButtonImage;
        [SerializeField] private Image chimployeeButtonImage;

        [SerializeField] private TextMeshProUGUI inventoryButtonInteractionText;
        [SerializeField] private TextMeshProUGUI blueprintsButtonInteractionText;
        [SerializeField] private TextMeshProUGUI chimployeeButtonInteractionText;

        [SerializeField] private TextMeshProUGUI inventoryButtonActionText;
        [SerializeField] private TextMeshProUGUI blueprintsButtonActionText;
        [SerializeField] private TextMeshProUGUI chimployeeButtonActionText;
        
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        
        public InterfaceContext interfaceContext;
        private CanvasGroup chimployeeButtonCanvasGroup;

        [SerializeField] private GameObject firstInventoryItem;
        [SerializeField] private GameObject firstBlueprintItem;
        
        private void Start() {
            interfaceContext = InterfaceContext.INVENTORY;
            chimployeeButtonCanvasGroup = chimployeeButtonImage.GetComponent<CanvasGroup>();
        }

        public void Switch_To_Left_Tab() {
            switch (interfaceContext) {
                case InterfaceContext.INVENTORY:
                    if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.ASPIRE_SOMETHING)) Switch_To_Chimployee();
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
                    if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.ASPIRE_SOMETHING)) Switch_To_Chimployee();
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

            inventoryButtonImage.color = activatedColor;
            inventoryButtonInteractionText.color = Color.black;
            inventoryButtonActionText.color = Color.black;

            blueprintsButtonImage.color = unactivatedColor;
            blueprintsButtonInteractionText.color = activatedColor;
            blueprintsButtonActionText.color = activatedColor;
            
            chimployeeButtonImage.color = unactivatedColor;
            chimployeeButtonInteractionText.color = activatedColor;
            chimployeeButtonActionText.color = activatedColor;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.INVENTAIRE);
        }

        public void Switch_To_Blueprints() {
            EventSystem.current.SetSelectedGameObject(firstBlueprintItem);

            interfaceContext = InterfaceContext.BLUEPRINTS;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.CHIMPLOYEE, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, true);
            
            inventoryButtonImage.color = unactivatedColor;
            inventoryButtonInteractionText.color = activatedColor;
            inventoryButtonActionText.color = activatedColor;

            blueprintsButtonImage.color = activatedColor;
            blueprintsButtonInteractionText.color = Color.black;
            blueprintsButtonActionText.color = Color.black;
            
            chimployeeButtonImage.color = unactivatedColor;
            chimployeeButtonInteractionText.color = activatedColor;
            chimployeeButtonActionText.color = activatedColor;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.BLUEPRINTS;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.BLUEPRINTS);
        }

        public void Switch_To_Chimployee() {
            if (chimployeeButtonCanvasGroup.alpha < 1f) return;
            
            interfaceContext = InterfaceContext.CHIMPLOYEE;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.CHIMPLOYEE, true);

            inventoryButtonImage.color = unactivatedColor;
            inventoryButtonInteractionText.color = activatedColor;
            inventoryButtonActionText.color = activatedColor;

            blueprintsButtonImage.color = unactivatedColor;
            blueprintsButtonInteractionText.color = activatedColor;
            blueprintsButtonActionText.color = activatedColor;
            
            chimployeeButtonImage.color = activatedColor;
            chimployeeButtonInteractionText.color = Color.black;
            chimployeeButtonActionText.color = Color.black;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.CHIMPLOYEE;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.CHIMPLOYEE);
        }

        public void Activate_Chimployee_Tab() {
            chimployeeButtonCanvasGroup.alpha = 1;
            chimployeeButtonCanvasGroup.interactable = true;
            chimployeeButtonCanvasGroup.blocksRaycasts = true;
        }

        public void Unactivate_Chimployee_Tab() {
            chimployeeButtonCanvasGroup.alpha = 0;
            chimployeeButtonCanvasGroup.interactable = false;
            chimployeeButtonCanvasGroup.blocksRaycasts = false;
        }

        public static void AuthorizeTp() {
            ObjectsReference.Instance.chimployee.TpButton.SetActive(true);
        }
    }
}
