using System.Collections.Generic;
using UnityEngine;

namespace InGame.Items.ItemsProperties.Characters {
    [CreateAssetMenu]
    public class GroupScriptableObject : ScriptableObject {
        public List<MonkeyMenPropertiesScriptableObject> members;
    }
}
