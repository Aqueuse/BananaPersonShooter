using Audio;
using Building;
using Cameras;
using Enums;
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
        
        private int _startInteractionMiniChimpDialoguesLength;

        public bool isShowingBananaGun;

        private int _dialogueIndex;
        
        private void Start() {
            if (GameData.Instance.bananaManSavedData.advancementState == AdvancementState.NEW_GAME) {
                _dialogueIndex = 0;
                isShowingBananaGun = false;
                
                _startInteractionMiniChimpDialoguesLength = startInteractionMiniChimp.GetComponent<MiniChimp>()
                    .subtitlesDataScriptableObject.dialogue.Count;
                
                BeginInteraction();
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

        private void BeginInteraction() {
            DialoguesManager.Instance.SetActiveMiniChimp(startInteractionMiniChimp);
            DialoguesManager.Instance.StartDialogue();

            MainCamera.Instance.SwitchToDialogueCamera(startInteractionMiniChimp.transform);
            
            Invoke(nameof(PlayMiniChimpDialogue), 1);
        }

        private void ShowRotatingBananaGunPanel() {
            isShowingBananaGun = true;
            slowlyRotate.isRotating = true;
            UIManager.Instance.Set_active(rotatingBananaGunCanvasGroup, true);
            DialoguesManager.Instance.dialoguePanel.SetActive(false);
        }

        public void HideRotatingBananaGunPanel() {
            isShowingBananaGun = false;
            AudioManager.Instance.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
            UIManager.Instance.Set_active(rotatingBananaGunCanvasGroup, false);
            
            UIManager.Instance.Show_HUD();
            BananaGun.Instance.bananaGunInBack.SetActive(true);
            MainCamera.Instance.SwitchToFreeLookCamera();
            
            DialoguesManager.Instance.dialoguePanel.SetActive(true);
            
            _dialogueIndex++;
            DialoguesManager.Instance.PlayMiniChimpDialogue(_dialogueIndex);
        }
        
        private void PlayMiniChimpDialogue() {
            if (_dialogueIndex == 3) {
                ShowRotatingBananaGunPanel();
                return;
            }

            if (_dialogueIndex >= _startInteractionMiniChimpDialoguesLength) {
                QuitDialogue();
            }

            else {
                DialoguesManager.Instance.PlayMiniChimpDialogue(_dialogueIndex);
                _dialogueIndex++;
            }
        }

        private void QuitDialogue() {
            GameData.Instance.bananaManSavedData.advancementState = AdvancementState.GET_BANANAGUN;

            DialoguesManager.Instance.QuitDialogue();
            
            DialoguesManager.Instance.SetActiveMiniChimp(commandRoomMiniChimp);
        }
    }
}
