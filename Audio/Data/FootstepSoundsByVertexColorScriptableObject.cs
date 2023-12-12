using UnityEngine;

namespace ItemsProperties {
    [CreateAssetMenu (fileName = "Footsteps", menuName = "ScriptableObjects/FootstepSoundsByVertexColorScriptableObject", order = 4)]
    public class FootstepSoundsByVertexColorScriptableObject : ScriptableObject {
        private GenericDictionary<Color, FootStepType> _footStepTypeByVertexColor;
    }
}