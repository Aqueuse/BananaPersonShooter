using Data.Bananas;
using Data.Craftables;
using Enums;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoSingleton<ScriptableObjectManager> {
        [SerializeField] private GenericDictionary<ItemThrowableType, BananasDataScriptableObject> bananasDataScriptableObject;
        [SerializeField] private GenericDictionary<ItemThrowableType, CraftableDataScriptableObject> craftablesDataScriptableObject;
        
        public string GetDescription(ItemThrowableCategory itemThrowableCategory, ItemThrowableType itemThrowableType, int langageIndex) {
            if (itemThrowableCategory == ItemThrowableCategory.CRAFTABLE) {
                return craftablesDataScriptableObject[itemThrowableType].description[langageIndex];
            }
            
            return bananasDataScriptableObject[itemThrowableType].description[langageIndex];
        }

        public BananasDataScriptableObject GetBananaScriptableObject(ItemThrowableType itemThrowableType) {
            return bananasDataScriptableObject[itemThrowableType];
        }
        
        public int GetCraftCost(ItemThrowableType itemThrowableType, int quantity) {
            return craftablesDataScriptableObject[itemThrowableType].cost*quantity;
        }

        public ItemThrowableType GetCraftIngredient(ItemThrowableType itemThrowableType) {
            return craftablesDataScriptableObject[itemThrowableType].rawMaterial;
        }
    }
}
