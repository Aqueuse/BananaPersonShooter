using UnityEngine;

namespace Audio {
    public class ButtonClick : MonoBehaviour {
        [SerializeField] private AudioSource audioSource;
        private AudioClip _audioClip;
        private AudioDataScriptableObject _audioDataScriptableObject;

        private void Start() {
            _audioDataScriptableObject = ObjectsReference.Instance.audioManager.audioEffectsDictionnary[SoundEffectType.BUTTON_INTERACTION];
            _audioClip = _audioDataScriptableObject.clip[0];
        }

        public void Click() {
            audioSource.clip = _audioClip;
            audioSource.volume = _audioDataScriptableObject.volume * ObjectsReference.Instance.audioManager.effectsLevel;
        
            //audioSource.Play();
        }
    }
}
