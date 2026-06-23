using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
    public class PlayOneShotWithRandomPitch : MonoBehaviour {
        [SerializeField] private AudioSource _audioSource;
        private bool hasBeenplayed;

        private void OnCollisionEnter() {
            if (hasBeenplayed) return;
            
            _audioSource.pitch = Random.Range(0.5f, 1.5f);
            _audioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel; 
            _audioSource.Play();

            hasBeenplayed = true;
        }
    }
}
