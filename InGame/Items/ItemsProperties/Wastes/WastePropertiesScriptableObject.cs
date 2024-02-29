using UnityEngine;

namespace InGame.Items.ItemsProperties.Wastes {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/wastePropertiesScriptableObject", order = 3)]
    public class WastePropertiesScriptableObject : ItemScriptableObject {
        public GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        public GenericDictionary<RawMaterialType, int> GetRawMaterialsWithQuantity() {
            return rawMaterialsWithQuantity;
        }
    }
}
