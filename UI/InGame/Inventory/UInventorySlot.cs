using Data;
using Enums;
using TMPro;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour {
        public ItemScriptableObject itemScriptableObject;

        [SerializeField] private TextMeshProUGUI quantityText;

        public void AssignToSlot() {
            SetDescriptionAndName();

            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(itemScriptableObject.bananaType, itemScriptableObject.itemCategory, itemScriptableObject.buildableType);
            ObjectsReference.Instance.uInventoriesManager.SetLastSelectedItem(itemScriptableObject.itemCategory, gameObject);

            if (itemScriptableObject.itemCategory == ItemCategory.BANANA) {
                ObjectsReference.Instance.bananaMan.activeItem = ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(itemScriptableObject.bananaType);
            }
        }

        public void SetQuantity(int quantity) {
            quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
        }

        public void SetDescriptionAndName() {
            ObjectsReference.Instance.descriptionsManager.ShowPanel(itemScriptableObject.gameObjectTag);
            ObjectsReference.Instance.descriptionsManager.inventoryGestionPanel.SetDescription(itemScriptableObject);
        }
    }
}
