using UnityEngine;

namespace Data.Wastes {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/wasteDataScriptableObject", order = 3)]
    public class WasteDataScriptableObject : ItemScriptableObject {
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
