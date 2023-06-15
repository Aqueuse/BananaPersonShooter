using System.Collections.Generic;
using Enums;
using Game.CommandRoomPanelControls;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace UI.InGame.Chimployee {
    public enum ChimployeeDialogue {
        chimployee_first_interaction,  // integre le chimployee au banana gun
        chimployee_second_interaction, // nourri un singe
        chimployee_third_interaction // clean a map
    }

    public class UIChimployee : MonoBehaviour {
        [SerializeField] private GenericDictionary<ChimployeeDialogue, List<GameObject>>  questDialogues;
        [SerializeField] private GameObject chimployeeTabDialogueContent;
        [SerializeField] private Scrollbar scrollbar;
        
        [SerializeField] private GameObject SkipButton;
        public GameObject TpButton;

        private ChimployeeDialogue _chimployeeDialogue;

        private int dialogueIndex;

        public bool dialogueShown;

        public void InitDialogue(ChimployeeDialogue chimployeeDialogue) {
            ObjectsReference.Instance.uiManager.Show_Hide_interface();
            ObjectsReference.Instance.uihud.Switch_To_Chimployee();
            
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.CHIMPLOYEE);
            
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.BANANAGUN_HELPER].alpha = 0;

            // flush last dialogues
            var lastDialogues = chimployeeTabDialogueContent.GetComponentsInChildren<TextMeshProUGUI>();
            
            foreach (var dialogue in lastDialogues) {
                Destroy(dialogue.gameObject);
            }

            _chimployeeDialogue = chimployeeDialogue;
            dialogueIndex = 0;
            dialogueShown = false;

            SkipButton.SetActive(true);
            
            Next();
        }

        public void Next() {
            if (!dialogueShown && dialogueIndex < questDialogues[_chimployeeDialogue].Count) {
                Instantiate(questDialogues[_chimployeeDialogue][dialogueIndex], chimployeeTabDialogueContent.transform);
                
                var dialogueTime = questDialogues[_chimployeeDialogue][dialogueIndex].GetComponent<LocalizeStringEvent>().StringReference
                    .GetLocalizedString().Length/20;

                dialogueIndex += 1;
                
                LayoutRebuilder.ForceRebuildLayoutImmediate(chimployeeTabDialogueContent.GetComponent<RectTransform>());
                scrollbar.value = 0;
                
                ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);

                Invoke(nameof(Next), dialogueTime);
            }

            else {
                CancelInvoke();
                FinishDialogue();
            }
        }
        
        public void FinishDialogue() {
            if (!dialogueShown) {
                dialogueShown = true;

                for (var i = dialogueIndex; i < questDialogues[_chimployeeDialogue].Count; i++) {
                    Instantiate(questDialogues[_chimployeeDialogue][dialogueIndex], chimployeeTabDialogueContent.transform);
                    dialogueIndex += 1;
                }
            }
            
            // hide next and skip buttons
            SkipButton.SetActive(false);
        }

        public static void AuthorizeDoorsAccess() {
            CommandRoomControlPanelsManager.Instance.AuthorizeDoorsAccess();
            Uihud.AuthorizeTp();
        }
    }
}
