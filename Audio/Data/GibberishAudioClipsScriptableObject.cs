using System;
using UnityEngine;

namespace Audio.Data {
    [CreateAssetMenu (fileName = "Gibberish", menuName = "ScriptableObjects/GibberishAudioClipsScriptableObject", order = 4)]
    public class GibberishAudioClipsScriptableObject : ScriptableObject {
        public GenericDictionary<Char, AudioClip> characterToClip;
    }
}