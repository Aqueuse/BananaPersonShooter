using UnityEngine;

namespace UI.InGame.Blueprints {
    public class UIBlueprintSlot : MonoBehaviour {
        public ItemCategory itemCategory = ItemCategory.BUILDABLE;
        public BuildableType buildableType;
        
        public void AssignToSlot() {
            SetDescriptionAndName();
            
            ObjectsReference.Instance.uInventory.lastselectedInventoryItem = gameObject;

            ObjectsReference.Instance.uiSlotsManager.AssignToSelectedSlot(ItemCategory.BUILDABLE, buildableType:buildableType);
        }
        
        private void SetDescriptionAndName() {
            ObjectsReference.Instance.uiBlueprints.itemName.text = ObjectsReference.Instance.scriptableObjectManager.GetName(itemCategory, ObjectsReference.Instance.gameSettings.languageIndexSelected, buildableType:buildableType);
            ObjectsReference.Instance.uiBlueprints.itemDescription.text = ObjectsReference.Instance.scriptableObjectManager.GetDescription(itemCategory, ObjectsReference.Instance.gameSettings.languageIndexSelected, buildableType:buildableType);
        }
    }
}