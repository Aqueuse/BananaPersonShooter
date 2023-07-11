using UnityEngine;

namespace UI.InGame.Blueprints {
    public class UIBlueprintSlot : MonoBehaviour {
        public ItemCategory itemCategory = ItemCategory.BUILDABLE;
        public BuildableType buildableType;
        
        public void AssignToSlot() {
            SetDescriptionAndName();
            
            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(ItemType.EMPTY, itemCategory, buildableType);
            ObjectsReference.Instance.uInventory.lastselectedInventoryItem = gameObject;
            
            ObjectsReference.Instance.uiSlotsManager.AssignToSelectedSlot(ItemCategory.BUILDABLE, buildableType:buildableType);
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
        }
        
        private void SetDescriptionAndName() {
            ObjectsReference.Instance.uiBlueprints.itemName.text = ObjectsReference.Instance.scriptableObjectManager.GetName(itemCategory, buildableType:buildableType);
            ObjectsReference.Instance.uiBlueprints.itemDescription.text = ObjectsReference.Instance.scriptableObjectManager.GetDescription(itemCategory, buildableType:buildableType);
        }
    }
}