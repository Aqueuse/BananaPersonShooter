using System;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/GibberishAudioDataScriptableObject", order = 4)]
    public class GibberishAudioDataScriptableObject : ScriptableObject {
        public GenericDictionary<Char, AudioClip> characterToClip;
    }
}