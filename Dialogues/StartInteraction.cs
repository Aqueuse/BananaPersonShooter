using Audio;
using Building;
using Cameras;
using Enums;
using Game;
using MiniChimps;
using Save;
using UI;
using UnityEngine;
using VFX;

namespace Dialogues {
    public class StartInteraction : MonoSingleton<StartInteraction> {
        [SerializeField] private CanvasGroup rotatingBananaGunCanvasGroup;
        [SerializeField] private SlowlyRotate slowlyRotate;
        
        [SerializeField] private GameObject startInteractionMiniChimp;
        [SerializeField] private GameObject commandRoomMiniChimp;
        
        private int startInteractionMiniChimpDialoguesLength;

        public bool isShowingBananaGun;

        private int dialogueIndex;
        
        private void Start() {
            if (GameData.Instance.BananaManSavedData.advancementState == AdvancementState.NEW_GAME) {
                dialogueIndex = 0;
                isShowingBananaGun = false;
                
                startInteractionMiniChimpDialoguesLength = startInteractionMiniChimp.GetComponent<MiniChimp>()
                    .subtitlesDataScriptableObject.dialogue.Count;
                
                Invoke(nameof(StartFirstInteraction), 3f);
            }
        }

        public void Validate() {
            if (isShowingBananaGun) {
                HideRotatingBananaGunPanel();
            }

            else {
                PlayMiniChimpDialogue();
            }
        }

        private void StartFirstInteraction() {
            DialoguesManager.Instance.SetActiveMiniChimp(startInteractionMiniChimp);
            DialoguesManager.Instance.StartDialogue();

            MainCamera.Instance.SwitchToDialogueCamera();
            PlayMiniChimpDialogue();
        }

        private void ShowRotatingBananaGunPanel() {
            isShowingBananaGun = true;
            slowlyRotate.isRotating = true;
            UIManager.Instance.Set_active(rotatingBananaGunCanvasGroup, true);
            DialoguesManager.Instance.dialoguePanel.SetActive(false);
        }

        public void HideRotatingBananaGunPanel() {
            isShowingBananaGun = false;
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION);
            UIManager.Instance.Set_active(rotatingBananaGunCanvasGroup, false);
            
            UIManager.Instance.Show_HUD();
            BananaGun.Instance.bananaGunInBack.SetActive(true);
            MainCamera.Instance.SwitchToFreeLookCamera();
            
            DialoguesManager.Instance.dialoguePanel.SetActive(true);
            
            dialogueIndex++;
            DialoguesManager.Instance.PlayMiniChimpDialogue(dialogueIndex);
        }
        
        private void PlayMiniChimpDialogue() {
            if (dialogueIndex == 3) {
                ShowRotatingBananaGunPanel();
                return;
            }

            if (dialogueIndex >= startInteractionMiniChimpDialoguesLength) {
                QuitDialogue();
            }

            else {
                DialoguesManager.Instance.PlayMiniChimpDialogue(dialogueIndex);
                dialogueIndex++;
            }
        }

        private void QuitDialogue() {
            GameData.Instance.BananaManSavedData.advancementState = AdvancementState.GET_BANANAGUN;

            DialoguesManager.Instance.QuitDialogue();
            
            DialoguesManager.Instance.SetActiveMiniChimp(commandRoomMiniChimp);
        }
    }
}
