using Audio;
using UnityEngine;

namespace MiniChimps {
    public class MiniChimpSpeak : MonoBehaviour {
        private AudioSource _miniChimpAudioSource;

        private void Start() {
            _miniChimpAudioSource = GetComponentInChildren<AudioSource>();
        }

        public void PlayMiniChimpVoice(AudioClip audioClip) {
            _miniChimpAudioSource.volume = AudioManager.Instance.voicesLevel;
            _miniChimpAudioSource.clip = audioClip;
            _miniChimpAudioSource.loop = false;

            _miniChimpAudioSource.Play();
        }

        public void StopMiniChimpVoice() {
            _miniChimpAudioSource.Stop();
        }

    }
}
