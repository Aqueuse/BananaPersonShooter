using E7.Introloop;
using UnityEngine;

namespace Audio {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/audioDataScriptableObject", order = 1)]
    public class AudioDataScriptableObject : ScriptableObject {
        public AudioClip[] clip;
        public IntroloopAudio clipWithIntro;
        public bool IsLooping;
        
        [Range(0.0f, 1.0f)] public float volume;
    }
}