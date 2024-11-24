using InGame.Items.ItemsProperties;

namespace InGame.Inventory {
    public class InventoriesHelper {
        public void AddQuantity(DroppedType droppedType) {
            
        }

        public int GetQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.droppedType) {
                case DroppedType.BANANA:
                    return ObjectsReference.Instance.BananaManBananasInventory.GetQuantity(itemScriptableObject.bananaType);
                case DroppedType.INGREDIENTS:
                    return ObjectsReference.Instance.ingredientsInventory.GetQuantity(itemScriptableObject.ingredientsType);
                case DroppedType.RAW_MATERIAL:
                    return ObjectsReference.Instance.rawMaterialInventory.GetQuantity(itemScriptableObject.rawMaterialType);
                case DroppedType.MANUFACTURED_ITEMS:
                    return ObjectsReference.Instance.manufacturedItemsInventory.GetQuantity(itemScriptableObject.manufacturedItemsType);
            }

            return 0;
        }
        
        public void RemoveQuantity() {
            
        }
    }
}