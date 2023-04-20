using Enums;
using UnityEngine;

namespace UI.InGame.Blueprints {
    public class UIBlueprintSlot : MonoBehaviour {
        public ItemCategory itemCategory = ItemCategory.BUILDABLE;
        public BuildableType buildableType;
        
        public void AssignToSlot() {
            SetDescription();
            
            ObjectsReference.Instance.uInventory.lastselectedInventoryItem = gameObject;

            ObjectsReference.Instance.uiSlotsManager.AssignToSelectedSlot(ItemCategory.BUILDABLE, buildableType:buildableType);
        }
        
        private void SetDescription() {
            ObjectsReference.Instance.uInventory.itemDescription.text = ObjectsReference.Instance.scriptableObjectManager.GetDescription(itemCategory, ObjectsReference.Instance.gameSettings.languageIndexSelected, buildableType:buildableType);
        }
    }
}