using System.Collections.Generic;
using Enums;
using Interactions;
using UnityEngine;

namespace Building.Buildables {
    public class BananasDryer : MonoBehaviour {
        [SerializeField] private List<BananaDryerSlot> bananasDryerSlots;

        [SerializeField] private Interaction placeInteractionUI;
        public Interaction takeInteractionUI;
        
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
            if (ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory[RawMaterialType.BANANA_PEEL] == 0) return;
            
            if (peelsQuantity < 20) {
                GetEmptySlot().AddBananaPeel();

                ObjectsReference.Instance.rawMaterialsInventory.RemoveQuantity(RawMaterialType.BANANA_PEEL, 1);
                peelsQuantity += 1;
            }
        }

        public void GiveFabric() {
            fabricQuantity = GetFabricQuantity();
            
            if (fabricQuantity == 0) return;

            ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.FABRIC, fabricQuantity);
            
            foreach (var bananasDryerSlot in bananasDryerSlots) {
                bananasDryerSlot.Fabric.enabled = false;
            }
            
            fabricQuantity = 0;
            
            takeInteractionUI.HideUI();
            placeInteractionUI.ShowUI();
        }

        public void RetrieveRawMaterials() {
            ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.FABRIC, fabricQuantity);
            ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.BANANA_PEEL, peelsQuantity);
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
