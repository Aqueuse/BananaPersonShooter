using Data;
using Gestion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class QuickSlotsManager : MonoBehaviour {
        private int bananaQuantity;

        public ItemScriptableObject bananaSlotItemScriptableObject;

        public Image bananaIconImage;
        public TextMeshProUGUI bananaQuantityText;

        private Color availableColor;
        private Color unavailableColor;
        
        public Image platformImage;

        private void Start() {
            availableColor = ObjectsReference.Instance.ghostsReference.GetColorByGhostState(GhostState.VALID);
            unavailableColor = ObjectsReference.Instance.ghostsReference.GetColorByGhostState(GhostState.NOT_ENOUGH_MATERIALS);
        }

        public void SetBananaSlot(ItemScriptableObject itemScriptableObject) {
            if (itemScriptableObject == null) return;

            bananaSlotItemScriptableObject = itemScriptableObject;

            bananaIconImage.sprite = itemScriptableObject.GetSprite();

            SetBananaQuantity(ObjectsReference.Instance.bananasInventory.GetQuantity(itemScriptableObject.bananaType));
        }

        public void SetBananaQuantity(int quantity) {
            bananaQuantityText.text = quantity > 999 ? "999+" : quantity.ToString();
        }
        
        public void SetPlateformSlotAvailability() {
            if (ObjectsReference.Instance.rawMaterialsInventory.HasCraftingIngredientsForPlateform()) {
                SetPlateformSlotAvailable();
            }
            else {
                SetPlateformSlotUnavailable();
            }
        }

        private void SetPlateformSlotAvailable() {
            platformImage.color = availableColor;
        }

        private void SetPlateformSlotUnavailable() {
            platformImage.color = unavailableColor;
        }
    }
}