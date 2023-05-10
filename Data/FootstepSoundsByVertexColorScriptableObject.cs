using Enums;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/FootstepSoundsByVertexColorScriptableObject", order = 4)]
    public class FootstepSoundsByVertexColorScriptableObject : ScriptableObject {
        private GenericDictionary<Color, FootStepType> _footStepTypeByVertexColor;
    }
}