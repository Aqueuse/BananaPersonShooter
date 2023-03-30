using Enums;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/FootstepSoundsByMaterialScriptableObject", order = 4)]
    public class FootstepSoundsByMaterialScriptableObject : ScriptableObject {
        public GenericDictionary<Material, FootStepType> basicFootStepTypesByMaterial;
        public GenericDictionary<Material, FootStepType> terrainFootStepTypeByMaterial;
    }
}