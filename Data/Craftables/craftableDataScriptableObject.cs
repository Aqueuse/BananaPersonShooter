using Enums;
using UnityEngine;

namespace Data.Craftables {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/craftableDataScriptableObject", order = 2)]
    public class CraftableDataScriptableObject : ScriptableObject {
        public ItemThrowableType itemThrowableType;
        public ItemThrowableCategory itemThrowableCategory;

        [Header("UI")]
        public Sprite sprite;
        [Multiline] public string[] description;
        
        // craft
        [Header("craft")]
        public ItemThrowableType rawMaterial;
        public int cost;
    }
}