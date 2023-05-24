using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public enum InterfaceContext {
        INVENTORY,
        BLUEPRINTS,
        STATISTICS
    }

    public class Uihud : MonoBehaviour {
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject blueprintsButton;
        [SerializeField] private GameObject statisticsButton;
    
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        
        public InterfaceContext interfaceContext;

        private void Start() {
            interfaceContext = InterfaceContext.INVENTORY;
        }

        public void Switch_To_Inventory() {
            interfaceContext = InterfaceContext.INVENTORY;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.STATISTICS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, true);

            inventoryButton.GetComponent<Image>().color = activatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

            blueprintsButton.GetComponent<Image>().color = unactivatedColor;
            blueprintsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            statisticsButton.GetComponent<Image>().color = unactivatedColor;
            statisticsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.INVENTAIRE);
        }

        public void Switch_To_Blueprints() {
            interfaceContext = InterfaceContext.BLUEPRINTS;
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.STATISTICS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, true);
            
            inventoryButton.GetComponent<Image>().color = unactivatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            blueprintsButton.GetComponent<Image>().color = activatedColor;
            blueprintsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

            statisticsButton.GetComponent<Image>().color = unactivatedColor;
            statisticsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.BLUEPRINTS;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.BLUEPRINTS);
        }

        public void Switch_To_Statistics() {
            interfaceContext = InterfaceContext.STATISTICS;

            ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
            ObjectsReference.Instance.uiStatistics.Refresh_Map_Statistics(ObjectsReference.Instance.mapsManager.currentMap.mapName);
            
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.INVENTORY, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.BLUEPRINTS, false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.STATISTICS, true);

            inventoryButton.GetComponent<Image>().color = unactivatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            
            blueprintsButton.GetComponent<Image>().color = unactivatedColor;
            blueprintsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            statisticsButton.GetComponent<Image>().color = activatedColor;
            statisticsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            ObjectsReference.Instance.inputManager.uiSchemaContext = UISchemaSwitchType.STATISTIQUES;
            ObjectsReference.Instance.inputManager.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.STATISTIQUES);
        }
    }
}
