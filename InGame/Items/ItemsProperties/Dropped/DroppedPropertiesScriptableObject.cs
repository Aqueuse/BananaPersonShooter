using UnityEngine;

namespace InGame.Items.ItemsProperties.Dropped {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/droppedPropertiesScriptableObject", order = 3)]
    public class DroppedPropertiesScriptableObject : ItemScriptableObject {
        public GenericDictionary<DroppedType, int> droppedMaterialsWithQuantity;

        public GenericDictionary<DroppedType, int> GetDroppedMaterialsWithQuantity() {
            return droppedMaterialsWithQuantity;
        }
    }
}
