using Enums;
using Settings;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoSingleton<ScriptableObjectManager> {
        [SerializeField] private GenericDictionary<ItemThrowableType, BananasDataScriptableObject> bananasDataScriptableObject;
        [SerializeField] private GenericDictionary<ItemThrowableType, PlateformDataScriptableObject> plateformDataScriptableObject;
        [SerializeField] private RocketDataScriptableObject rocketDataScriptableObject;

        public string GetDescription(ItemThrowableType itemThrowableType, int langageIndex) {
            if (itemThrowableType == ItemThrowableType.ROCKET) {
                return rocketDataScriptableObject.description[langageIndex];
            }

            if (itemThrowableType.ToString().Contains("PLATEFORM")) {
                return plateformDataScriptableObject[itemThrowableType].description[langageIndex];
            }

            return bananasDataScriptableObject[itemThrowableType].description[langageIndex];
        }

        public BananasDataScriptableObject GetBananaScriptableObject(ItemThrowableType itemThrowableType) {
            return bananasDataScriptableObject[itemThrowableType];
        }

        public string GetPlateformName(ItemThrowableType itemThrowableType) {
            return plateformDataScriptableObject[itemThrowableType].plateformName[GameSettings.Instance.languageIndexSelected];
        }
        
        public GenericDictionary<ItemThrowableType, int> GetPlateformCost(ItemThrowableType itemThrowableType) {
            return plateformDataScriptableObject[itemThrowableType].Cost;
        }

    }
}
