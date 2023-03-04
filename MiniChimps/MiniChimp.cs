using Data;
using Dialogues;
using Enums;
using Settings;
using UnityEngine;

namespace MiniChimps {
    public class MiniChimp : MonoBehaviour {
        public DialogueDataScriptableObject subtitlesDataScriptableObject;
        public AudioClip[] dialoguesAudioClips;

        private MiniChimpSpeak miniChimpSpeak;
        public MiniChimpType miniChimpType;

        private void Start() {
            miniChimpSpeak = GetComponent<MiniChimpSpeak>();
        }

        public void StopSpeak() {
            miniChimpSpeak.StopMiniChimpVoice();
        }

        public void Speak(int index) {
            miniChimpSpeak.PlayMiniChimpVoice(dialoguesAudioClips[index]);
            Subtitles.Instance.Show_dialogue(subtitlesDataScriptableObject.dialogue[index][GameSettings.Instance.languageIndexSelected]);
        }
    }
}
