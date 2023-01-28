using Enums;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoSingleton<ScriptableObjectManager> {
        [SerializeField] private GenericDictionary<ItemThrowableType, BananasDataScriptableObject> bananasDataScriptableObject;
        [SerializeField] private PlateformDataScriptableObject plateformDataScriptableObject;
        [SerializeField] private DebrisDataScriptableObject debrisDataScriptableObject;
        
        public string GetDescription(ItemThrowableCategory itemThrowableCategory, ItemThrowableType itemThrowableType, int langageIndex) {
            if (itemThrowableType == ItemThrowableType.DEBRIS) {
                return debrisDataScriptableObject.description[langageIndex];
            }

            if (itemThrowableCategory == ItemThrowableCategory.PLATEFORM) {
                return plateformDataScriptableObject.description[langageIndex];
            }

            return bananasDataScriptableObject[itemThrowableType].description[langageIndex];
        }

        public BananasDataScriptableObject GetBananaScriptableObject(ItemThrowableType itemThrowableType) {
            return bananasDataScriptableObject[itemThrowableType];
        }
        
        public GenericDictionary<ItemThrowableType, int> GetPlateformCost() {
            return plateformDataScriptableObject.Cost;
        }
    }
}
