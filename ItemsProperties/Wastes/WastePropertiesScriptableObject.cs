using UnityEngine;

namespace ItemsProperties.Wastes {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/wastePropertiesScriptableObject", order = 3)]
    public class WastePropertiesScriptableObject : ItemScriptableObject {
        public GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        public GenericDictionary<RawMaterialType, int> GetRawMaterialsWithQuantity() {
            if (itemCategory == ItemCategory.DEBRIS) {
                if (Random.Range(0, 5) == 4) {
                    rawMaterialsWithQuantity[RawMaterialType.BATTERY] = 5;
                }
                else {
                    rawMaterialsWithQuantity[RawMaterialType.BATTERY] = 0;
                }
            }

            return rawMaterialsWithQuantity;
        }
    }
}
