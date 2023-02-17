using Audio;
using UnityEngine;

namespace MiniChimps {
    public class MiniChimpSpeak : MonoBehaviour {
        private AudioSource miniChimpAudioSource;

        private void Start() {
            miniChimpAudioSource = GetComponentInChildren<AudioSource>();
        }

        public void PlayMiniChimpVoice(AudioClip audioClip) {
            miniChimpAudioSource.volume = AudioManager.Instance.effectsLevel-0.2f;
            miniChimpAudioSource.clip = audioClip;
            miniChimpAudioSource.loop = false;

            miniChimpAudioSource.Play();
        }

        public void StopMiniChimpVoice() {
            miniChimpAudioSource.Stop();
        }

    }
}
