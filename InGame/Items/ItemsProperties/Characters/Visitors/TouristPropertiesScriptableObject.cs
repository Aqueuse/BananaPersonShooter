using UnityEngine;

namespace InGame.Items.ItemsProperties.Characters.Visitors {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/TouristPropertiesScriptableObject", order = 2)]
    public class TouristPropertiesScriptableObject : CharacterPropertiesScriptableObject {
        public string visitorGuid;
        public GenericDictionary<NeedType, int> visitorNeeds;
    }
}
