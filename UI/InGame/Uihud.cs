using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InGame {
    public enum InterfaceContext {
        INVENTORY,
        BLUEPRINTS
    }

    public enum SchemaContext {
        KEYBOARD,
        GAMEPAD
    }

    public class Uihud : MonoBehaviour {
        [SerializeField] private Image inventoryButtonImage;
        [SerializeField] private Image blueprintsButtonImage;

        [SerializeField] private TextMeshProUGUI inventoryButtonInteractionText;
        [SerializeField] private TextMeshProUGUI blueprintsButtonInteractionText;

        [SerializeField] private TextMeshProUGUI inventoryButtonActionText;
        [SerializeField] private TextMeshProUGUI blueprintsButtonActionText;
        
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        
        public InterfaceContext interfaceContext;
        private CanvasGroup chimployeeButtonCanvasGroup;

        [SerializeField] private GameObject firstInventoryItem;
        [SerializeField] private GameObject firstBlueprintItem;

        [SerializeField] private GenericDictionary<SchemaContext, UIHelper> uiHelpersBySchemaContext;
        
        private void Start() {
            interfaceContext = InterfaceContext.INVENTORY;
        }

        public void Switch_To_Left_Tab() {
            switch (interfaceContext) {
                case InterfaceContext.INVENTORY:
                    Switch_To_Blueprints();
                    break;
                case InterfaceContext.BLUEPRINTS:
                    Switch_To_Inventory();
                    break;
            }
        }

        public void Switch_To_Right_Tab() {
            switch (interfaceContext) {
                case InterfaceContext.INVENTORY:
                    Switch_To_Blueprints();
                    break;
                case InterfaceContext.BLUEPRINTS:
                    Switch_To_Inventory();
                    break;
            }
        }

        public void Switch_To_Inventory() {
            EventSystem.current.SetSelectedGameObject(firstInventoryItem);
            
            interfaceContext = InterfaceContext.INVENTORY;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, true);

            inventoryButtonImage.color = activatedColor;
            inventoryButtonInteractionText.color = Color.black;
            inventoryButtonActionText.color = Color.black;

            blueprintsButtonImage.color = unactivatedColor;
            blueprintsButtonInteractionText.color = activatedColor;
            blueprintsButtonActionText.color = activatedColor;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.INVENTAIRE);
        }

        public void Switch_To_Blueprints() {
            EventSystem.current.SetSelectedGameObject(firstBlueprintItem);

            interfaceContext = InterfaceContext.BLUEPRINTS;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, true);
            
            inventoryButtonImage.color = unactivatedColor;
            inventoryButtonInteractionText.color = activatedColor;
            inventoryButtonActionText.color = activatedColor;

            blueprintsButtonImage.color = activatedColor;
            blueprintsButtonInteractionText.color = Color.black;
            blueprintsButtonActionText.color = Color.black;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.BLUEPRINTS;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.BLUEPRINTS);
        }

        public UIHelper GetCurrentUIHelper() {
            return uiHelpersBySchemaContext[ObjectsReference.Instance.inputManager.schemaContext];
        }
    }
}
