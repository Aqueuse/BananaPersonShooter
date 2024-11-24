using UnityEngine;

namespace InGame.Items.ItemsProperties.Dropped.Raw_Materials {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/droppedPropertiesScriptableObject", order = 3)]
    public class DroppedPropertiesScriptableObject : ItemScriptableObject {
        public GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        public GenericDictionary<RawMaterialType, int> GetRawMaterialsWithQuantity() {
            return rawMaterialsWithQuantity;
        }
    }
}
