using UnityEngine;

namespace ItemsProperties.Buildables.VisitorsBuildable {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/visitorsBuildablePropertiesScriptableObject", order = 3)]
    public class VisitorsBuildablePropertiesScriptableObject : BuildablePropertiesScriptableObject {
        public NeedType needType;
        public int needValue;
        public string animatorTriggerToSet;
    }
}