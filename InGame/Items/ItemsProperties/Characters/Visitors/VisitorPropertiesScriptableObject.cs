using UnityEngine;

namespace InGame.Items.ItemsProperties.Characters.Visitors {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/VisitorPropertiesScriptableObject", order = 2)]
    public class VisitorPropertiesScriptableObject : CharacterPropertiesScriptableObject {
        public string visitorGuid;
        public GenericDictionary<NeedType, int> visitorNeeds;
    }
}
