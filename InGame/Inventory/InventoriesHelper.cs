using InGame.Items.ItemsProperties;
using InGame.Player;
using UnityEngine;

namespace InGame.Inventory {
    public class InventoriesHelper : MonoBehaviour {
        private BananaManData bananaManData;

        private void Start() {
            bananaManData = ObjectsReference.Instance.bananaMan.bananaManData;
        }

        public void AddQuantity(DroppedType droppedType) {

        }

        public int GetQuantity(ItemScriptableObject itemScriptableObject) {
            switch (itemScriptableObject.droppedType) {
                case DroppedType.BANANA:
                    return ObjectsReference.Instance.BananaManBananasInventory.GetQuantity(itemScriptableObject
                        .bananaType);
                case DroppedType.INGREDIENTS:
                    return ObjectsReference.Instance.bananaManIngredientsInventory.GetQuantity(itemScriptableObject
                        .ingredientsType);
                case DroppedType.RAW_MATERIAL:
                    return ObjectsReference.Instance.bananaManRawMaterialInventory.GetQuantity(itemScriptableObject
                        .rawMaterialType);
                case DroppedType.MANUFACTURED_ITEMS:
                    return ObjectsReference.Instance.bananaManManufacturedItemsInventory.GetQuantity(
                        itemScriptableObject.manufacturedItemsType);
            }

            return 0;
        }

        public int GetActiveDroppedQuantity() {
            switch (bananaManData.activeDropped) {
                case DroppedType.BANANA:
                    return ObjectsReference.Instance.BananaManBananasInventory.GetQuantity(bananaManData.activeBanana);
                case DroppedType.INGREDIENTS:
                    return ObjectsReference.Instance.bananaManIngredientsInventory.GetQuantity(bananaManData
                        .activeIngredient);
                case DroppedType.RAW_MATERIAL:
                    return ObjectsReference.Instance.bananaManRawMaterialInventory.GetQuantity(bananaManData
                        .activeRawMaterial);
                case DroppedType.MANUFACTURED_ITEMS:
                    return ObjectsReference.Instance.bananaManManufacturedItemsInventory.GetQuantity(bananaManData
                        .activeManufacturedItem);
            }

            return 0;
        }

        public int RemoveActiveDroppedQuantity() {
            switch (bananaManData.activeDropped) {
                case DroppedType.BANANA:
                    return ObjectsReference.Instance.BananaManBananasInventory.RemoveQuantity(bananaManData.activeBanana, 1);
                case DroppedType.INGREDIENTS:
                    return ObjectsReference.Instance.bananaManIngredientsInventory.RemoveQuantity(bananaManData.activeIngredient, 1);
                case DroppedType.RAW_MATERIAL:
                    return ObjectsReference.Instance.bananaManRawMaterialInventory.RemoveQuantity(bananaManData.activeRawMaterial, 1);
                case DroppedType.MANUFACTURED_ITEMS:
                    return ObjectsReference.Instance.bananaManManufacturedItemsInventory.RemoveQuantity(bananaManData.activeManufacturedItem, 1);
            }

            return 0;
        }
    }
}