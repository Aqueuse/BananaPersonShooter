using System.Linq;
using InGame.Items.ItemsProperties.Buildables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.MainPanel.Inventories {
    public class UIBlueprintsInventory : MonoBehaviour {
        public GenericDictionary<BuildableTabType, GenericDictionary<BuildableType, UIBlueprintSlot>> inventorySlotsByTabType;
        [SerializeField] private GenericDictionary<BuildableTabType, CanvasGroup> canvasGroupsByTabType;

        [SerializeField] private Transform buildablesContentTransform;

        [SerializeField] private CanvasGroup buildablePanelCanvasGroup;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private Color activatedColor;

        private BuildablePropertiesScriptableObject buildableDataScriptableObject;

        public void RefreshUInventory(BuildableTabType buildableTabType) {
            foreach (var blueprintItem in inventorySlotsByTabType[buildableTabType]) {
                var uiBlueprintSlot = inventorySlotsByTabType [buildableTabType][blueprintItem.Key];    
                buildableDataScriptableObject = blueprintItem.Value.buildableScriptableObject;
                
                uiBlueprintSlot.SetAvailability();
            }
        }

        public void SwitchToFunInventory() { SwitchToInventoryTab(BuildableTabType.FUN); }
        public void SwitchToHungerInventory() { SwitchToInventoryTab(BuildableTabType.HUNGER); }
        public void SwitchToRestInventory() { SwitchToInventoryTab(BuildableTabType.REST); }
        public void SwitchToKnowledgeInventory() { SwitchToInventoryTab(BuildableTabType.KNOWLEDGE); }
        public void SwitchToSouvenirInventory() { SwitchToInventoryTab(BuildableTabType.SOUVENIR); }
        public void SwitchToOtherInventory() { SwitchToInventoryTab(BuildableTabType.OTHER); }

        private void SwitchToInventoryTab(BuildableTabType buildableTabType) {
            ObjectsReference.Instance.uInventoriesManager.lastFocusedBlueprintInventory = buildableTabType;
            
            foreach (var canvasGroup in canvasGroupsByTabType) {
                SetActive(canvasGroup.Value, false);
            }
            
            RefreshUInventory(buildableTabType);
            
            SetActive(canvasGroupsByTabType[buildableTabType], true);
            
            SelectFirstSlot(buildableTabType);
        }

        public void Activate() {
            buildablePanelCanvasGroup.alpha = 1;
            buildablePanelCanvasGroup.interactable = true;
            buildablePanelCanvasGroup.blocksRaycasts = true;
                
            buttonImage.color = activatedColor;
            buttonText.color = Color.black;
        }
        
        public void SelectFirstSlot(BuildableTabType needType) {
            inventorySlotsByTabType[needType].First().Value.SelectInventorySlot();
        }
        
        public void SetActive(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}