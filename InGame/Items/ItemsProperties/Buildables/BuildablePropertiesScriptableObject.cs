using UnityEngine;

namespace InGame.Items.ItemsProperties.Buildables {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/buildablePropertiesScriptableObject", order = 2)]
    public class BuildablePropertiesScriptableObject : ItemScriptableObject {
        [Header("UI")] public BuildableTabType buildableTabType;
        
        [Header("craft")]
        public GenericDictionary<ItemScriptableObject, int> rawMaterialsWithQuantity;
        public GameObject buildablePrefab;
        public Sprite blueprintSprite;
    }
}