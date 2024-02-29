using InGame.Items.ItemsProperties.Bananas;
using UnityEngine;

namespace InGame.Items.ItemsProperties.Regimes {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/RegimePropertiesScriptableObject", order = 4)]
    public class RegimePropertiesScriptableObject : ItemScriptableObject {
        public BananasPropertiesScriptableObject associatedBananasPropertiesScriptableObject;
        
        public int regimeQuantity;
    }
}
