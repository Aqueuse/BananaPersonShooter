using System.Collections.Generic;
using InGame.Inventories;
using InGame.Items.ItemsProperties;
using UnityEngine;

namespace InGame.Player {
    public class BananaManData : MonoBehaviour {
        public GenericDictionary<DroppedType, Inventory> inventoriesByDroppedType;
        
        public int bitKongQuantity;

        public List<RawMaterialType> discoveredRawMaterials;
        
        public ItemScriptableObject activeItemScriptableObject;
        
        public int GetActiveSlotItemQuantity() {
            activeItemScriptableObject = ObjectsReference.Instance.bottomSlots.GetSelectedSlot(); 
            
            return inventoriesByDroppedType[activeItemScriptableObject.droppedType].GetQuantity(activeItemScriptableObject);
        }

        public void RemoveActiveSlotItemQuantity(int quantity) {
            activeItemScriptableObject = ObjectsReference.Instance.bottomSlots.GetSelectedSlot(); 

            inventoriesByDroppedType[activeItemScriptableObject.droppedType].RemoveQuantity(activeItemScriptableObject, quantity);
        }
    }
}
