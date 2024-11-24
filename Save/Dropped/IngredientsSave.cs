using System;
using System.Collections.Generic;
using System.IO;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using Newtonsoft.Json;
using UnityEngine;

namespace Save.Dropped {
    public class IngredientsSave : MonoBehaviour {
        public GameObject IngredientsContainer;
    
        private string _savePath;

        private GameObject ingredientToSpawn;
        private GameObject ingredientInstance;

        public GenericDictionary<IngredientsType, List<string>> IngredientsDataDictionaryByIngredientsType;
        
        public void LoadIngredientsDataByUuid(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var saveMapDatasPath = Path.Combine(_savePath, "WORLD_DATA");

            var loadfilePath = Path.Combine(saveMapDatasPath, "ingredients.json");

            if (!File.Exists(loadfilePath)) return;
            
            var json = File.ReadAllText(loadfilePath);
            
            var IngredientsDictionnary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            
            IngredientsDictionnary.Clear();
            
            foreach (var ingredientsList in IngredientsDictionnary) {
                var ingredientsType = Enum.Parse<IngredientsType>(ingredientsList.Key);

                foreach (var ingredient in ingredientsList.Value) {
                    if (IngredientsDataDictionaryByIngredientsType.ContainsKey(ingredientsType)) {
                        IngredientsDataDictionaryByIngredientsType[ingredientsType].Add(ingredient);
                    }
                    else {
                        IngredientsDataDictionaryByIngredientsType.Add(ingredientsType, new List<string>{ ingredient });
                    }
                }

                RespawnIngredientssOnWorld();
            }
        }

        private void RespawnIngredientssOnWorld() {
            DestroyImmediate(IngredientsContainer);

            IngredientsContainer = new GameObject("Ingredients container") {
                transform = {
                    parent = transform.parent
                }
            };

            foreach (var ingredientsList in IngredientsDataDictionaryByIngredientsType) {
                foreach (var ingredientString in ingredientsList.Value) {
                    ingredientToSpawn = ObjectsReference.Instance.meshReferenceScriptableObject.ingredientPrefabByIngredientType[ingredientsList.Key];
                    
                    ingredientInstance = Instantiate(ingredientToSpawn, IngredientsContainer.transform, true);
                    ingredientInstance.GetComponent<IngredientBehaviour>().LoadSavedData(ingredientString);
                }
            }
        }

        public void SaveIngredientsData(string saveUuid) {
            _savePath = Path.Combine(ObjectsReference.Instance.gameSave._savesPath, saveUuid);
            var mapDataSavesPath = Path.Combine(_savePath, "WORLD_DATA");
            
            var savefilePath = Path.Combine(mapDataSavesPath, "ingredients.json");
            
            var ingredientBehaviours = FindObjectsByType<IngredientBehaviour>(FindObjectsSortMode.None);
            
            IngredientsDataDictionaryByIngredientsType.Clear();

            foreach (var ingredientBehaviour in ingredientBehaviours) ingredientBehaviour.GenerateDroppedData();
            
            var json = JsonConvert.SerializeObject(IngredientsDataDictionaryByIngredientsType);
            File.WriteAllText(savefilePath, json);
        }
        
        public void AddIngredientsToIngredientsDictionnary(IngredientsType ingredientsType, string ingredientsData) {
            if (IngredientsDataDictionaryByIngredientsType.ContainsKey(ingredientsType)) {
                IngredientsDataDictionaryByIngredientsType[ingredientsType].Add(ingredientsData);
            }
            else {
                IngredientsDataDictionaryByIngredientsType.Add(ingredientsType, new List<string>{ ingredientsData });
            }
        }
    }
}
