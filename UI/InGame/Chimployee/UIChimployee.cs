using System.Collections.Generic;
using Game.CommandRoomPanelControls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Chimployee {
    public enum ChimployeeDialogue {
        chimployee_first_interaction
    }

    public class UIChimployee : MonoBehaviour {
        [SerializeField] private GenericDictionary<ChimployeeDialogue, List<GameObject>>  questDialogues;
        [SerializeField] private GameObject chimployeeTabDialogueContent;
        [SerializeField] private Scrollbar scrollbar;

        [SerializeField] private GameObject NextButton;
        [SerializeField] private GameObject SkipButton;
        public GameObject TpButton;

        private ChimployeeDialogue _chimployeeDialogue;

        private int dialogueIndex;

        public bool dialogueShown;

        public void InitDialogue(ChimployeeDialogue chimployeeDialogue) {
            // flush last dialogues
            var lastDialogues = chimployeeTabDialogueContent.GetComponentsInChildren<TextMeshProUGUI>();
            
            foreach (var dialogue in lastDialogues) {
                Destroy(dialogue.gameObject);
            }

            _chimployeeDialogue = chimployeeDialogue;
            dialogueIndex = 0;
            dialogueShown = false;

            NextButton.SetActive(true);
            SkipButton.SetActive(true);
            
            Next();
        }

        public void Next() {
            if (!dialogueShown && dialogueIndex < questDialogues[_chimployeeDialogue].Count) {
                Instantiate(questDialogues[_chimployeeDialogue][dialogueIndex], chimployeeTabDialogueContent.transform);
                dialogueIndex += 1;
                
                LayoutRebuilder.ForceRebuildLayoutImmediate(chimployeeTabDialogueContent.GetComponent<RectTransform>());
                scrollbar.value = 0;
            }

            else {
                FinishDialogue();
            }
        }

        public void FinishDialogue() {
            if (!dialogueShown) {
                dialogueShown = true;

                for (int i = dialogueIndex; i < questDialogues[_chimployeeDialogue].Count; i++) {
                    Instantiate(questDialogues[_chimployeeDialogue][dialogueIndex], chimployeeTabDialogueContent.transform);
                    dialogueIndex += 1;
                }
            }
            
            // hide next and skip buttons
            NextButton.SetActive(false);
            SkipButton.SetActive(false);
        }

        public void AuthorizeDoorsAccess() {
            CommandRoomControlPanelsManager.Instance.AuthorizeDoorsAccess();
            TpButton.SetActive(true);
        }
    }
}
