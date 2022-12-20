using Audio;
using UnityEngine;

namespace Bosses.Gorilla {
    public class GorillaSounds : MonoBehaviour {
        [SerializeField] private AudioDataScriptableObject gorillaQuickRoarSounds;
        [SerializeField] private AudioDataScriptableObject boomSound;

        private AudioSource _audioSource;

        void Start() {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayQuickRoarSound() {
            _audioSource.clip = gorillaQuickRoarSounds.clip[Random.Range(0, gorillaQuickRoarSounds.clip.Length)];
            _audioSource.volume = gorillaQuickRoarSounds.volume;
            _audioSource.Play();
        }

        public void PlayBoomSound() {
            _audioSource.clip = boomSound.clip[0];
            _audioSource.volume = boomSound.volume;
            _audioSource.Play();
        }

    }
}
