using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using Newtonsoft.Json;
using UnityEngine;

namespace Save.Dropped {
    public class DroppedFoodSave : MonoBehaviour {
        private string _savePath;

        private GameObject foodToSpawn;
        private GameObject foodInstance;

        public GenericDictionary<FoodType, List<string>> foodDataDictionaryByFoodType;
        
        public void LoadFoodDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "food.json");

            if (!File.Exists(loadfilePath)) return;
            
            var json = File.ReadAllText(loadfilePath);
            
            var foodDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            
            foodDictionnary.Clear();
            
            foreach (var foodList in foodDictionnary) {
                var foodType = Enum.Parse<FoodType>(foodList.Key);

                foreach (var food in foodList.Value) {
                    if (foodDataDictionaryByFoodType.ContainsKey(foodType)) {
                        foodDataDictionaryByFoodType[foodType].Add(food);
                    }
                    else {
                        foodDataDictionaryByFoodType.Add(foodType, new List<string>{ food });
                    }
                }

                RespawnFoodOnWorld();
            }
        }

        private void RespawnFoodOnWorld() {
            foreach (var foodList in foodDataDictionaryByFoodType) {
                foreach (var foodString in foodList.Value) {
                    foodToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.foodPrefabByFoodType[foodList.Key];
                    
                    foodInstance = Instantiate(foodToSpawn, ObjectsReference.Instance.gameSave.savablesItemsContainer, true);
                    foodInstance.GetComponent<FoodBehaviour>().LoadSavedData(foodString);
                }
            }
        }

        public void SaveFoodData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "food.json");
            
            var foodBehaviours = ObjectsReference.Instance.gameSave.savablesItemsContainer.GetComponentsInChildren<FoodBehaviour>();
            
            foodDataDictionaryByFoodType.Clear();

            foreach (var foodBehaviour in foodBehaviours) foodBehaviour.GenerateDroppedData();
            
            var json = JsonConvert.SerializeObject(foodDataDictionaryByFoodType);
            File.WriteAllText(savefilePath, json);
        }
        
        public void AddFoodToFoodDictionnary(FoodType foodType, string foodData) {
            if (foodDataDictionaryByFoodType.ContainsKey(foodType)) {
                foodDataDictionaryByFoodType[foodType].Add(foodData);
            }
            else {
                foodDataDictionaryByFoodType.Add(foodType, new List<string>{ foodData });
            }
        }
    }
}
