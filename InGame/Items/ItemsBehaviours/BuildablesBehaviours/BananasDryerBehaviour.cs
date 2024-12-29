using System;
using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using InGame.Items.ItemsData.BuildablesData;
using InGame.Items.ItemsProperties;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BananasDryerBehaviour : BuildableBehaviour {
        [SerializeField] private GameObject fabricPrefab;
        [SerializeField] private int conversionDuration;

        [SerializeField] private ItemScriptableObject fabricScriptableObject;
        [SerializeField] private ItemScriptableObject bananaPeelScriptableObject;
        
        public List<BananaDryerSlot> slots;
        
        [SerializeField] private GameObject[] bananaPeel;
        [SerializeField] private GameObject[] fabric;
        
        private readonly BananaEffect[] colorants = new BananaEffect[20];

        private int bananaPeelsQuantity;
        
        private void Init() {
            for (var i = 0; i < 20; i++) {
                if (slots[i].hasBananaPeel) {
                    PutBanana(i, slots[i].bananaEffect);
                    colorants[i] = slots[i].bananaEffect;
                    
                    bananaPeelsQuantity += 1;
                }
                
                else if (slots[i].hasFabric) {
                    SetFabric(i);
                    colorants[i] = slots[i].bananaEffect;
                }
            }
        }
        
        private void Update() {
            if (bananaPeelsQuantity <= 0) return;
            
            for (var i = 0; i < 20; i++) {
                if (slots[i].hasBananaPeel) {
                    slots[i].timer -= 1;
                
                    if (slots[i].timer <= 0) {
                        slots[i].hasFabric = true;
                        
                        SetFabric(i);
                        
                        slots[i].timer = conversionDuration;
                    }
                }
            }
        }

        private bool TryToPutBananaPeel(BananaEffect bananaEffect) {
            for (var i = 0; i < 20; i++) {
                if (!slots[i].hasFabric && !slots[i].hasBananaPeel) {
                    PutBanana(i, bananaEffect);
                    return true;
                }
            }

            return false;
        }

        private void PutBanana(int slotIndex, BananaEffect bananaColor) {
            slots[slotIndex].hasBananaPeel = true;
            
            bananaPeel[slotIndex].SetActive(true);
            
            slots[slotIndex].bananaEffect = bananaColor;
            slots[slotIndex].timer = conversionDuration;
            colorants[slotIndex] = slots[slotIndex].bananaEffect;
            
            bananaPeelsQuantity += 1;
        }

        private void SetFabric(int slotIndex) {
            slots[slotIndex].hasBananaPeel = false;
            slots[slotIndex].hasFabric = true;
            
            bananaPeel[slotIndex].SetActive(false);
            fabric[slotIndex].SetActive(true);
            
            bananaPeelsQuantity -= 1;
        }

        public void TakeFabric() {
            for (var i = 0; i < 20; i++) {
                if (slots[i].hasFabric) {
                    slots[i].hasFabric = false;
                    
                    fabric[i].SetActive(false);
                    bananaPeel[i].SetActive(false);

                    Instantiate(
                        fabricPrefab,
                        fabric[i].transform.position - Vector3.forward,
                        fabric[i].transform.rotation,
                        ObjectsReference.Instance.gameSave.savablesItemsContainer
                    );
                    
                    Instantiate(
                        ObjectsReference.Instance.meshReferenceScriptableObject.DyePrefabByBananaEffect[colorants[i]],
                        fabric[i].transform.position - Vector3.forward,
                        fabric[i].transform.rotation,
                        ObjectsReference.Instance.gameSave.savablesItemsContainer
                    );
                    
                    break;
                }
            }
        }
        
        public override void RetrieveRawMaterials() {
            base.RetrieveRawMaterials();
            
            for (var i = 0; i < 20; i++) {
                if (slots[i].hasFabric) {
                    Instantiate(
                        fabricScriptableObject.prefab,
                        fabric[i].transform.position - Vector3.forward,
                        fabric[i].transform.rotation,
                        ObjectsReference.Instance.gameSave.savablesItemsContainer
                    );
                    
                    Instantiate(
                        ObjectsReference.Instance.meshReferenceScriptableObject.DyePrefabByBananaEffect[colorants[i]],
                        fabric[i].transform.position - Vector3.forward,
                        fabric[i].transform.rotation,
                        ObjectsReference.Instance.gameSave.savablesItemsContainer
                    );
                }

                else {
                    if (slots[i].hasBananaPeel) {
                        Instantiate(
                            bananaPeelScriptableObject.prefab,
                            fabric[i].transform.position - Vector3.forward,
                            fabric[i].transform.rotation,
                            ObjectsReference.Instance.gameSave.savablesItemsContainer
                        );
                    }
                }
            }

            bananaPeelsQuantity = 0;
        }
        
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 7) return;

            if (other.gameObject.TryGetComponent<BananaBehaviour>(out var bananaBehaviour)) {
                var bananaPeelPlaced = TryToPutBananaPeel(bananaBehaviour.bananasPropertiesScriptableObject.bananaEffect);
                
                if (bananaPeelPlaced) Destroy(other.gameObject);
            }
        }
        
        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(buildableGuid)) {
                buildableGuid = Guid.NewGuid().ToString();
            }
            
            var bananasDryerData = new BananasDryerData {
                buildableGuid = buildableGuid,
                buildableType = buildableType,
                isBreaked = isBreaked,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                bananaDryerSlots = slots
            };

            ObjectsReference.Instance.gameSave.buildablesSave.AddBuildableToBuildableDictionnary(BuildableType.BANANA_DRYER, JsonConvert.SerializeObject(bananasDryerData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            var bananasDryerData = JsonConvert.DeserializeObject<BananasDryerData>(stringifiedJson);

            buildableGuid = bananasDryerData.buildableGuid;
            buildableType = bananasDryerData.buildableType;
            isBreaked = bananasDryerData.isBreaked;
            transform.position = JsonHelper.FromStringToVector3( bananasDryerData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(bananasDryerData.buildableRotation);

            slots = bananasDryerData.bananaDryerSlots;
            
            Init();
        }
    }
}
