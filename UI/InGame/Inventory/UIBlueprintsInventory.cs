using System.Linq;
using InGame.Items.ItemsProperties.Buildables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIBlueprintsInventory : MonoBehaviour {
        public GenericDictionary<NeedType, GenericDictionary<BuildableType, UIBlueprintSlot>> inventorySlotsByNeedType;

        [SerializeField] private GenericDictionary<NeedType, CanvasGroup> canvasGroupsByNeedType;

        [SerializeField] private Transform buildablesContentTransform;

        [SerializeField] private CanvasGroup buildablePanelCanvasGroup;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private Color activatedColor;

        private BuildablePropertiesScriptableObject buildableDataScriptableObject;

        public void RefreshUInventory(NeedType needType) {
            foreach (var blueprintItem in inventorySlotsByNeedType[needType]) {
                var uiBlueprintSlot = inventorySlotsByNeedType [needType][blueprintItem.Key];
                buildableDataScriptableObject = blueprintItem.Value.buildableScriptableObject;

                if (buildableDataScriptableObject.buildableType == BuildableType.EMPTY) continue;

                uiBlueprintSlot.SetColor(ObjectsReference.Instance.ghostsReference.GetUIColorByAvailability(buildableDataScriptableObject));
            }
        }

        public void SwitchToFunInventory() { SwitchToInventoryTab(NeedType.FUN); }
        public void SwitchToHungerInventory() { SwitchToInventoryTab(NeedType.HUNGER); }
        public void SwitchToRestInventory() { SwitchToInventoryTab(NeedType.REST); }
        public void SwitchToKnowledgeInventory() { SwitchToInventoryTab(NeedType.KNOWLEDGE); }
        public void SwitchToSouvenirInventory() { SwitchToInventoryTab(NeedType.SOUVENIR); }

        private void SwitchToInventoryTab(NeedType needType) {
            ObjectsReference.Instance.uInventoriesManager.lastFocusedBlueprintInventory = needType;

            foreach (var canvasGroup in canvasGroupsByNeedType) {
                SetActive(canvasGroup.Value, false);
            }
            
            SetActive(canvasGroupsByNeedType[needType], true);
            
            SelectFirstSlot(needType);
        }

        public void Activate() {
            ObjectsReference.Instance.uInventoriesManager.lastFocusedInventory = DroppedType.BLUEPRINT;
        
            buildablePanelCanvasGroup.alpha = 1;
            buildablePanelCanvasGroup.interactable = true;
            buildablePanelCanvasGroup.blocksRaycasts = true;
                
            buttonImage.color = activatedColor;
            buttonText.color = Color.black;
        }
        
        public void SelectFirstSlot(NeedType needType) {
            inventorySlotsByNeedType[needType].First().Value.SelectInventorySlot();
        }
        
        public void SetActive(CanvasGroup canvasGroup, bool visible) {
            canvasGroup.alpha = visible ? 1 : 0;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}