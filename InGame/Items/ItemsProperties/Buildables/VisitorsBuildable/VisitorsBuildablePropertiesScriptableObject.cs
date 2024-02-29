using UnityEngine;

namespace InGame.Items.ItemsProperties.Buildables.VisitorsBuildable {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/visitorsBuildablePropertiesScriptableObject", order = 3)]
    public class VisitorsBuildablePropertiesScriptableObject : BuildablePropertiesScriptableObject {
        public NeedType needType;
        public int needValue;
        
        public bool isAnimationLooping;
        public string animatorParameterToActivate;
    }
}