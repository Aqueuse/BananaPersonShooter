using System;
using System.Collections.Generic;
using InGame.Items.ItemsData.BuildablesData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BananasDryerBehaviour : BuildableBehaviour {
        [SerializeField] private List<BananaDryerSlot> bananasDryerSlots;

        [Range(0, 20)] public int bananaPeelsQuantity;
        [Range(0, 20)] public int fabricQuantity;

        private void Init() {
            foreach (var bananasDryerSlot in bananasDryerSlots) {
                bananasDryerSlot.Init();
            }
        }
        
        public void RetrieveRawMaterials() {
            ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.FABRIC, fabricQuantity);
            ObjectsReference.Instance.rawMaterialsInventory.AddQuantity(RawMaterialType.BANANA_PEEL, bananaPeelsQuantity);
        }
        
        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(buildableGuid)) {
                buildableGuid = Guid.NewGuid().ToString();
            }
            
            BananasDryerData bananasDryerData = new BananasDryerData {
                buildableGuid = buildableGuid,
                buildableType = buildableType,
                isBreaked = isBreaked,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation)
            };

            ObjectsReference.Instance.gameSave.buildablesSave.AddBuildableToBuildableDictionnary(BuildableType.BANANA_DRYER, JsonConvert.SerializeObject(bananasDryerData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            BananasDryerData bananasDryerData = JsonConvert.DeserializeObject<BananasDryerData>(stringifiedJson);

            buildableGuid = bananasDryerData.buildableGuid;
            buildableType = bananasDryerData.buildableType;
            isBreaked = bananasDryerData.isBreaked;
            transform.position = JsonHelper.FromStringToVector3( bananasDryerData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(bananasDryerData.buildableRotation);
            
            Init();
        }
    }
}
