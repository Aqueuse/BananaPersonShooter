using Audio;
using UnityEngine;

namespace Monkeys.Gorilla {
    public class GorillaSounds : MonoBehaviour {
        [SerializeField] private AudioDataScriptableObject gorillaQuickRoarSounds;
        [SerializeField] private AudioDataScriptableObject boomSound;

        [SerializeField] private AudioClip[] gorillaSadSounds;

        [SerializeField] private AudioSource audioVocalSource;
        [SerializeField] private AudioSource audioFootstepsSource;
        
        public void PlayQuickRoarSound() {
            audioVocalSource.clip = gorillaQuickRoarSounds.clip[Random.Range(0, gorillaQuickRoarSounds.clip.Length)];
            audioVocalSource.volume = gorillaQuickRoarSounds.volume;
            audioVocalSource.Play();
        }

        public void PlayBoomSound() {
            audioVocalSource.clip = boomSound.clip[0];
            audioVocalSource.volume = boomSound.volume;
            audioVocalSource.Play();
        }

        public void PlaySadMonkeySounds() {
            Invoke(nameof(GorillaRitournelle), Random.Range(5, 15));
        }
        
        private void GorillaRitournelle() {
            float randomTime = Random.Range(10, 20);
        
            foreach (var gorillaSadSound in gorillaSadSounds) {
                audioVocalSource.clip = gorillaSadSounds[Random.Range(0, gorillaSadSounds.Length)];
                audioVocalSource.Play();
            }
        
            Invoke(nameof(GorillaRitournelle), randomTime);
        }

        public void GorillaFootstep() {
            
        }
    }
}
