using Cameras;
using MiniChimps;
using UI;

namespace Dialogues {
    public class SimpleInteraction : MonoSingleton<SimpleInteraction> {
        private int miniChimpDialoguesLength;
        private int dialogueIndex;
        
        private void Start() {
                dialogueIndex = 0;
        }

        public void Validate() {
            PlayMiniChimpDialogue();
        }

        public void StartSimpleInteraction() {
            miniChimpDialoguesLength = DialoguesManager.Instance.activeMiniChimp.GetComponent<MiniChimp>()
                .subtitlesDataScriptableObject.dialogue.Count;
            dialogueIndex = 0;
            
            DialoguesManager.Instance.StartDialogue();

            MainCamera.Instance.SwitchToDialogueCamera(DialoguesManager.Instance.activeMiniChimp.transform);
            PlayMiniChimpDialogue();
        }
        
        private void PlayMiniChimpDialogue() {
            if (dialogueIndex >= miniChimpDialoguesLength) {
                QuitDialogue();
            }

            else {
                DialoguesManager.Instance.PlayMiniChimpDialogue(dialogueIndex);
                dialogueIndex++;
            }
        }

        private void QuitDialogue() {
            UIManager.Instance.Show_HUD();
            
            DialoguesManager.Instance.QuitDialogue();
        }
    }
}
