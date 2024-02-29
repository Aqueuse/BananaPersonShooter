using UnityEngine;

namespace InGame.Items.ItemsProperties.Characters {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/InventoryPropertiesScriptableObject", order = 2)]
    public class InventoryScriptableObject : ScriptableObject {
        public GenericDictionary<BananaType, int> bananasInventory;
        public GenericDictionary<RawMaterialType, int> rawMaterialsInventory;
    
        public GenericDictionary<IngredientsType, int> ingredientsInventory;
        public GenericDictionary<ManufacturedItemsType, int> manufacturedItemsInventory;

        public int bitKongQuantity;
    }
}
