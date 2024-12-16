using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.MainPanel.Inventories {
    public class UIBlueprintSlot : MonoBehaviour {
        public BuildablePropertiesScriptableObject buildableScriptableObject;

        public void Activate() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GESTION_VIEW) {
                ObjectsReference.Instance.gestionViewMode.ActivateGhostByScriptableObject(buildableScriptableObject);
            }

            else {
                ObjectsReference.Instance.bananaMan.SetActiveBuildable(buildableScriptableObject);
            }
        }
        
        public void SelectInventorySlot() {
            ObjectsReference.Instance.uInventoriesManager.SetLastSelectedItem(buildableScriptableObject.droppedType, gameObject);
        }
        
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.uInfobulle.SetDescriptionAndName(
                buildableScriptableObject.GetName(),
                buildableScriptableObject.GetDescription(),
                GetComponent<RectTransform>());
        }

        public void SetAvailability() {
            var isAvailable =
                ObjectsReference.Instance.bananaManRawMaterialInventory.HasCraftingIngredients(buildableScriptableObject);
            
            GetComponentsInChildren<Image>()[1].sprite = isAvailable ? 
                buildableScriptableObject.itemSprite : 
                buildableScriptableObject.blueprintSprite;
            
            GetComponent<Image>().color = ObjectsReference.Instance.ghostsReference.GetUIColorByAvailability(isAvailable);
        }
    }
}
