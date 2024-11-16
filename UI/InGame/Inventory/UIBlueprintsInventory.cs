using InGame.Items.ItemsProperties.Buildables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIBlueprintsInventory : MonoBehaviour {
        public GenericDictionary<BuildableType, UIBlueprintSlot> inventorySlotsByBuildableType;

        [SerializeField] private Transform buildablesContentTransform; 
        
        [SerializeField] private CanvasGroup buildablePanelCanvasGroup;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private Color activatedColor;
        
        private BuildablePropertiesScriptableObject buildableDataScriptableObject;

        public void RefreshUInventory() {
            foreach (var blueprintItem in inventorySlotsByBuildableType) {
                var uiBlueprintSlot = inventorySlotsByBuildableType[blueprintItem.Key];
                buildableDataScriptableObject = blueprintItem.Value.buildableScriptableObject;

                if (buildableDataScriptableObject.buildableType == BuildableType.EMPTY) continue;
                
                uiBlueprintSlot.SetColor(ObjectsReference.Instance.ghostsReference.GetUIColorByAvailability(buildableDataScriptableObject));
            }
        }
        
        public void Activate() {
            ObjectsReference.Instance.uInventoriesManager.lastFocusedInventory = ItemCategory.BUILDABLE;
        
            buildablePanelCanvasGroup.alpha = 1;
            buildablePanelCanvasGroup.interactable = true;
            buildablePanelCanvasGroup.blocksRaycasts = true;
                
            buttonImage.color = activatedColor;
            buttonText.color = Color.black;
        }
    }
}