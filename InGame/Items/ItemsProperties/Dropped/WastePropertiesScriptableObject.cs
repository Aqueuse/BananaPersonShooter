using UnityEngine;

namespace InGame.Items.ItemsProperties.Dropped {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/wastePropertiesScriptableObject", order = 3)]
    public class WastePropertiesScriptableObject : ItemScriptableObject {
        public GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        public GenericDictionary<RawMaterialType, int> GetRawMaterialsWithQuantity() {
            return rawMaterialsWithQuantity;
        }
    }
}
