using UnityEngine;

namespace InGame.Items.ItemsProperties.Food {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/FoodPropertiesScriptableObject", order = 3)]
    public class FoodPropertiesScriptableObject : ItemScriptableObject {
        public GenericDictionary<IngredientsType, int> ingredientsWithQuantity;
    }
}
