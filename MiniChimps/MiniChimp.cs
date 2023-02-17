using System.Linq;
using Dialogues;
using Player;
using UnityEngine;

namespace MiniChimps {
    public class MiniChimp : MonoBehaviour {
        [SerializeField] private GenericDictionary<AudioClip, DialogueDataScriptableObject> miniChimpSpeaksDictionnary;

        private MiniChimpSpeak miniChimpSpeak;
        private MiniChimpSubtitles miniChimpSubtitles;

        private int ritournelleIndex;

        private void Start() {
            miniChimpSpeak = GetComponent<MiniChimpSpeak>();
            miniChimpSubtitles = GetComponent<MiniChimpSubtitles>();

            ritournelleIndex = 0;
        }

        public void StopSpeak() {
            miniChimpSpeak.StopMiniChimpVoice();
            miniChimpSubtitles.Hide_Dialogue();

            BananaMan.Instance.GetComponent<PlayerController>().canMove = true;
        }

        public void Speak() {
            miniChimpSpeak.PlayMiniChimpVoice(miniChimpSpeaksDictionnary.ElementAt(ritournelleIndex).Key);
            MiniChimpSubtitles.Instance.Show_dialogue(miniChimpSpeaksDictionnary.ElementAt(ritournelleIndex).Value);
        
            ritournelleIndex++;
            if (ritournelleIndex >= miniChimpSpeaksDictionnary.Count) ritournelleIndex = 0;
        }
    }
}
