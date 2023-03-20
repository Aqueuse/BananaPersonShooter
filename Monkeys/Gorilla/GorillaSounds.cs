using Audio;
using UnityEngine;

namespace Monkeys.Gorilla {
    public class GorillaSounds : MonoBehaviour {
        [SerializeField] private AudioDataScriptableObject gorillaQuickRoarSounds;
        [SerializeField] private AudioDataScriptableObject boomSound;

        [SerializeField] private AudioClip[] gorillaSadSounds;

        [SerializeField] private AudioSource _audioVocalSource;
        [SerializeField] private AudioSource _audioFootstepsSource;
        
        public void PlayQuickRoarSound() {
            _audioVocalSource.clip = gorillaQuickRoarSounds.clip[Random.Range(0, gorillaQuickRoarSounds.clip.Length)];
            _audioVocalSource.volume = gorillaQuickRoarSounds.volume;
            _audioVocalSource.Play();
        }

        public void PlayBoomSound() {
            _audioVocalSource.clip = boomSound.clip[0];
            _audioVocalSource.volume = boomSound.volume;
            _audioVocalSource.Play();
        }

        public void PlaySadMonkeySounds() {
            Invoke(nameof(GorillaRitournelle), Random.Range(5, 15));
        }
        
        private void GorillaRitournelle() {
            float randomTime = Random.Range(10, 20);
        
            foreach (var gorillaSadSound in gorillaSadSounds) {
                _audioVocalSource.clip = gorillaSadSounds[Random.Range(0, gorillaSadSounds.Length)];
                _audioVocalSource.Play();
            }
        
            Invoke(nameof(GorillaRitournelle), randomTime);
        }

        public void GorillaFootstep() {
            
        }
    }
}
