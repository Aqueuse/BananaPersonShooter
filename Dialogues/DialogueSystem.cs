using System.Collections.Generic;
using Audio;
using Cameras;
using Enums;
using Player;
using Settings;
using UI.InGame;
using UnityEngine;

namespace Dialogues {
    public class DialogueSystem : MonoSingleton<DialogueSystem> {
        [SerializeField] GenericDictionary<AdvancementType, DialogueDataScriptableObject> dialoguesData;
        [SerializeField] List<DialogueDataScriptableObject> ritournellesData;

        [SerializeField] private UIDialogueSystem uiDialogueSystem;

        private int ritournelleCount;
    
        public void interact_with_minichimp(GameObject miniChimp) {
            BananaMan.Instance.GetComponent<PlayerController>().canMove = false;
            AudioManager.Instance.PlayVoice(VoiceType.MINICHIMP);
            ThirdPersonOrbitCamBasic.Instance.canRotate = false;
            
            if (BananaMan.Instance.advancementType == AdvancementType.FIRST_MINICHIMP_INTERACT) {
                BananaMan.Instance.advancementType = AdvancementType.OTHER;
                Show_dialogue(dialoguesData[AdvancementType.FIRST_MINICHIMP_INTERACT]);
                return;
            }

            if (BananaMan.Instance.advancementType == AdvancementType.OTHER) {
                Show_ritournelle();
            }
        }

        public void Show_dialogue(DialogueDataScriptableObject dialogueDataScriptableObject) {
            uiDialogueSystem.show_dialogue(dialogueDataScriptableObject.dialogue[GameSettings.Instance.languageIndexSelected]);
        }
        
        public void Show_ritournelle() {
            uiDialogueSystem.show_dialogue(ritournellesData[ritournelleCount].dialogue[GameSettings.Instance.languageIndexSelected]);
            ritournelleCount++;
            if (ritournelleCount >= ritournellesData.Count) ritournelleCount = 0;
        }

        public void Hide_Dialogue() {
            AudioManager.Instance.StopAudioSource(AudioSourcesType.VOICE);
            uiDialogueSystem.hide_dialogue();
            BananaMan.Instance.GetComponent<PlayerController>().canMove = true;
            ThirdPersonOrbitCamBasic.Instance.canRotate = true;
        }
    }
}
