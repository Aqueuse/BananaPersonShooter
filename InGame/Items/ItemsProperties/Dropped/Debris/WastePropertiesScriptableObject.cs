using UnityEngine;

namespace InGame.Items.ItemsProperties.Dropped.Debris {
    [CreateAssetMenu (fileName = "wastePropertiesScriptableObject", menuName = "ScriptableObjects/wastePropertiesScriptableObject", order = 3)]
    public class WastePropertiesScriptableObject : ItemScriptableObject {
        public GenericDictionary<ItemScriptableObject, int> rawMaterialsWithQuantity;
    }
}