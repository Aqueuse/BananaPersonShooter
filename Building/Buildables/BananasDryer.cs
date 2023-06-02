using System.Collections.Generic;
using UnityEngine;

namespace Building.Buildables {
    public class BananasDryer : MonoBehaviour {
        [SerializeField] private List<BananaDryerSlot> bananasDryerSlots;

        [SerializeField] private GameObject placeInteractionUI;
        public GameObject takeInteractionUI;
        
        public int peelsQuantity;
        public int fabricQuantity;

        private void Start() {
            peelsQuantity = 0;
            fabricQuantity = 0;
        }

        private BananaDryerSlot GetEmptySlot() {
            foreach (var slot in bananasDryerSlots) {
                if (slot.BananaPeel.enabled == false && slot.Fabric.enabled == false) {
                    return slot;
                }
            }

            return null;
        }

        public void AddBananaPeel() {
            if (ObjectsReference.Instance.inventory.bananaManInventory[ItemType.BANANA_PEEL] == 0) return;
            
            if (peelsQuantity < 20) {
                GetEmptySlot().AddBananaPeel();

                ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.RAW_MATERIAL, ItemType.BANANA_PEEL, 1);
                peelsQuantity += 1;
            }
        }

        public void GiveFabric() {
            fabricQuantity = GetFabricQuantity();
            
            if (fabricQuantity == 0) return;

            ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.FABRIC, fabricQuantity);
            
            foreach (var bananasDryerSlot in bananasDryerSlots) {
                bananasDryerSlot.Fabric.enabled = false;
            }
            
            fabricQuantity = 0;
            
            takeInteractionUI.SetActive(false);
            placeInteractionUI.SetActive(true);
        }

        public void RetrieveRawMaterials() {
            ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.FABRIC, fabricQuantity);
            ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.BANANA_PEEL, peelsQuantity);
        }

        private int GetFabricQuantity() {
            fabricQuantity = 0;
            
            foreach (var slot in bananasDryerSlots) {
                if (slot.Fabric.enabled) fabricQuantity += 1;
            }

            return fabricQuantity;
        }
    }
}
