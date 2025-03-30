using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.MainPanel.Inventories {
    public class UIBlueprintsInventory : MonoBehaviour {
        public GenericDictionary<BuildableTabType, GenericDictionary<BuildableType, UIBlueprintSlot>> inventorySlotsByTabType;
        [SerializeField] private GenericDictionary<BuildableTabType, CanvasGroup> canvasGroupsByTabType;

        [SerializeField] private GenericDictionary<BuildableTabType, GameObject> buttonsTabByTabType;
        
        // global slot reference for save and give blueprint (because we don't care of the tab then)
        public GenericDictionary<BuildableType, UIBlueprintSlot> inventorySlotsByBuildableType;

        [SerializeField] private Transform buildablesContentTransform;

        [SerializeField] private CanvasGroup buildablePanelCanvasGroup;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private Color activatedColor;

        public void Allow(BuildableType buildableType) {
            var buildableTabType = ObjectsReference.Instance.meshReferenceScriptableObject
                .buildablePropertiesScriptableObjects[buildableType].buildableTabType;

            // buttons are hidden by default, active them if there is at least one buildable slot allowed
            buttonsTabByTabType[buildableTabType].SetActive(true);
            
            inventorySlotsByBuildableType[buildableType].gameObject.SetActive(true);
        }
        
        public void RefreshUInventory(BuildableTabType buildableTabType) {
            foreach (var blueprintItem in inventorySlotsByTabType[buildableTabType]) {
                inventorySlotsByTabType [buildableTabType][blueprintItem.Key].SetAvailability();
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