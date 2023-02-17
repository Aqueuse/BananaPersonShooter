using Audio;
using Enums;
using Player;
using Settings;
using UI.InGame;
using UnityEngine;

namespace Dialogues {
    public class MiniChimpSubtitles : MonoSingleton<MiniChimpSubtitles> {
        [SerializeField] private UIMiniChimpSubtitles uiMiniChimpSubtitles;
        
        public void Show_dialogue(DialogueDataScriptableObject dialogueDataScriptableObject) {
            uiMiniChimpSubtitles.show_dialogue(dialogueDataScriptableObject.dialogue[GameSettings.Instance.languageIndexSelected]);
        }
        
        public void Hide_Dialogue() {
            uiMiniChimpSubtitles.hide_dialogue();
        }
    }
}
