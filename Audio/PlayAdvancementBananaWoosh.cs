using UnityEngine;

namespace Audio {
    public class PlayAdvancementBananaWoosh : MonoBehaviour {
        [SerializeField] private AudioSource _audioSource;

        void Play(float pitch) {
            _audioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel;
            _audioSource.pitch = pitch;
            _audioSource.Play();
        }
    }
}
