using InGame.Items.ItemsProperties;
using UnityEngine;

namespace InGame.Inventories {
    public class Inventory : MonoBehaviour {
        public virtual int AddQuantity(ItemScriptableObject itemScriptableObject, int quantity) { return 0; }

        public virtual int RemoveQuantity(ItemScriptableObject itemScriptableObject, int quantity) { return 0; }

        public virtual int GetQuantity(ItemScriptableObject itemScriptableObject) { return 0; }
        
        public virtual void ResetInventory() { }
    }
}
