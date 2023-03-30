using Enums;
using Input;
using Settings;
using TMPro;
using UI.InGame.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public enum InterfaceContext {
        INVENTORY,
        STATISTICS
    }

    public class Uihud : MonoSingleton<Uihud> {
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject statisticsButton;
    
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color unactivatedColor;
        
        [SerializeField] private Image debrisNotVisibleImage;
        [SerializeField] private Image bananaTreesNotVisibleImage;

        public InterfaceContext interfaceContext;

        private void Start() {
            interfaceContext = InterfaceContext.INVENTORY;
        }

        public void Switch_To_Inventory() {
            interfaceContext = InterfaceContext.INVENTORY;
            
            UIManager.Instance.Set_active(UICanvasGroupType.STATISTICS, false);
            UIManager.Instance.Set_active(UICanvasGroupType.INVENTORY, true);

            inventoryButton.GetComponent<Image>().color = activatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            statisticsButton.GetComponent<Image>().color = unactivatedColor;
            statisticsButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
            
            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.INVENTAIRE;
            InputManager.Instance.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.INVENTAIRE);
        }

        public void Switch_To_Statistics() {
            interfaceContext = InterfaceContext.STATISTICS;

            UIStatistics.Instance.Refresh_Map_Statistics("MAP01");
            
            UIManager.Instance.Set_active(UICanvasGroupType.INVENTORY, false);
            UIManager.Instance.Set_active(UICanvasGroupType.STATISTICS, true);

            statisticsButton.GetComponent<Image>().color = activatedColor;
            statisticsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            inventoryButton.GetComponent<Image>().color = unactivatedColor;
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;

            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.STATISTIQUES;
            InputManager.Instance.uiSchemaSwitcher.SwitchUISchema(UISchemaSwitchType.STATISTIQUES);

            debrisNotVisibleImage.enabled = GameSettings.Instance.isShowingDebris;
            bananaTreesNotVisibleImage.enabled = GameSettings.Instance.isShowingBananaTrees;
        }
        
        public void SetDebrisCanvasVisibility(bool isVisible) {
            UICanvasItemsHiddableManager.Instance.SetItemsCanvasVisibility(isVisible);

            debrisNotVisibleImage.enabled = isVisible;
        }
        
        public void SetBananaTreeCanvasVisibility(bool isVisible) {
            UICanvasItemBananaTree.Instance.isItemVisible = isVisible;
            UICanvasItemBananaTree.Instance.GetComponent<Canvas>().enabled = isVisible;

            bananaTreesNotVisibleImage.enabled = isVisible;
        }

        public void OnclickDebrisButton() { 
            GameSettings.Instance.isShowingDebris = !GameSettings.Instance.isShowingDebris;
            SetDebrisCanvasVisibility(GameSettings.Instance.isShowingDebris);
        }

        public void OnclickBananaTreesButton() { 
            GameSettings.Instance.isShowingBananaTrees = !GameSettings.Instance.isShowingBananaTrees;
            SetBananaTreeCanvasVisibility(GameSettings.Instance.isShowingBananaTrees);
        }
    }
}
