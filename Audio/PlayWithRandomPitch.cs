using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
    public class PlayWithRandomPitch : MonoBehaviour {
        [SerializeField] private AudioSource _audioSource;
        private bool isSplashed;

        private void OnCollisionEnter() {
            if (isSplashed) return;
            
            _audioSource.pitch = Random.Range(0.5f, 1.5f);
            _audioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel; 
            _audioSource.Play();

            isSplashed = true;
        }
    }
}
