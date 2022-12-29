using Audio;
using Enums;
using UnityEngine;

namespace UI.InGame {
    public class UIPlayGibberish : MonoBehaviour {
        [SerializeField] private AudioManager audioManager;
    
        public void PlayGibberish() {
            audioManager.PlayVoice(VoiceType.MINICHIMP);
        }
    
        public void StopGibberish() {
            audioManager.StopAudioSource(AudioSourcesType.VOICE);
        }
    }
}
