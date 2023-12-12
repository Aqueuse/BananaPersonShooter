using System;
using System.Collections.Generic;
using Data.BuildablesData;
using Interactions;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace Gestion.BuildablesBehaviours {
    public class BananasDryerBehaviour : BuildableBehaviour {
        [SerializeField] private List<BananaDryerSlot> bananasDryerSlots;
        [SerializeField] private Interaction placeInteractionUI;
        
        public Interaction takeInteractionUI;

        public int peelsQuantity;
        public int fabricQuantity;

        private void Start() {
            peelsQuantity = 0;
            fabricQuantity = 0;
            
            if(string.IsNullOrEmpty(buildableGuid)) {
                buildableGuid = Guid.NewGuid().ToString();
            }
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

        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(buildableGuid)) {
                buildableGuid = Guid.NewGuid().ToString();
            }
            
            BananasDryerData bananasDryerData = new BananasDryerData {
                buildableGuid = buildableGuid,
                buildableType = buildableType,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation),
            };

            ObjectsReference.Instance.gameData.currentMapData.AddBuildableToBuildableDictionnary(BuildableType.BANANA_DRYER, JsonConvert.SerializeObject(bananasDryerData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            BananasDryerData bananasDryerData = JsonConvert.DeserializeObject<BananasDryerData>(stringifiedJson);

            buildableGuid = bananasDryerData.buildableGuid;
            buildableType = bananasDryerData.buildableType;
            transform.position = JsonHelper.FromStringToVector3( bananasDryerData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(bananasDryerData.buildableRotation);
        }
    }
}
