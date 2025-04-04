using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
    public class PlayWithRandomPitch : MonoBehaviour {
        [SerializeField] private AudioSource _audioSource;

        private void OnCollisionEnter() {
            _audioSource.pitch = Random.Range(0.5f, 1.5f);
            _audioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel; 
            _audioSource.Play();
        }
    }
}
