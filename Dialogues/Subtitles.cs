using UI.InGame;
using UnityEngine;

namespace Dialogues {
    public class Subtitles : MonoSingleton<Subtitles> {
        [SerializeField] private UISubtitles uiSubtitles;
        
        public void Show_dialogue(string dialogue) {
            uiSubtitles.show_dialogue(dialogue);
        }
    }
}
