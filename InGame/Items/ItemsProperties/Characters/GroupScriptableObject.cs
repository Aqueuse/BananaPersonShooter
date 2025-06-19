using System.Collections.Generic;
using InGame.Items.ItemsData.Characters;
using UnityEngine;

namespace InGame.Items.ItemsProperties.Characters {
    [CreateAssetMenu]
    public class GroupScriptableObject : ScriptableObject {
        public List<MonkeyMenData> members;
    }
}
