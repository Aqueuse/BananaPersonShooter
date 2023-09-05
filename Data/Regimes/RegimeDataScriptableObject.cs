using Data.Bananas;
using UnityEngine;

namespace Data.Regimes {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/RegimeDataScriptableObject", order = 4)]
    public class RegimeDataScriptableObject : ItemScriptableObject {
        public BananasDataScriptableObject associatedBananasDataScriptableObject;
        
        public int regimeQuantity;
    }
}
