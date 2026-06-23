using UnityEngine;

namespace InGame.Monkeys.Chimpvisitors {
    [CreateAssetMenu(fileName = "VisitorColorPresetScriptableObject", menuName = "ScriptableObjects/VisitorColorPresetScriptableObjectScript")]
    public class VisitorColorPresetScriptableObject : ScriptableObject {
        public Color[] colors;
    }
}
