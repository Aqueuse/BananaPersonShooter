using Audio;
using UnityEngine;

namespace Monkeys.Gorilla {
    public class GorillaSounds : MonoBehaviour {
        [SerializeField] private AudioDataScriptableObject gorillaQuickRoarSounds;
        [SerializeField] private AudioDataScriptableObject boomSound;

        [SerializeField] private AudioClip[] gorillaSadSounds;

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

        public void PlaySadMonkeySounds() {
            Invoke(nameof(GorillaRitournelle), Random.Range(5, 15));
        }
        
        private void GorillaRitournelle() {
            float randomTime = Random.Range(10, 20);
        
            foreach (var gorillaSadSound in gorillaSadSounds) {
                _audioSource.clip = gorillaSadSounds[Random.Range(0, gorillaSadSounds.Length)];
                _audioSource.Play();
            }
        
            Invoke(nameof(GorillaRitournelle), randomTime);
        }


    }
}
