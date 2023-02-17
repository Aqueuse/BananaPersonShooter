using Building;
using Cameras;
using Enums;
using Input;
using MiniChimps;
using UI;
using UI.InGame.Tutorials;
using UnityEngine;
using VFX;

namespace Tutorials {
    public class StartInteraction : MonoBehaviour {
        [SerializeField] private CanvasGroup firstTutorialCanvasGroup;
        [SerializeField] private GameObject rotatingBananaGunPanel;
        
        [SerializeField] private GameObject startInteractionMiniChimp;
        [SerializeField] private GameObject commandRoomMiniChimp;
        
        [SerializeField] private FirstTutorialVideos firstTutorialVideos;


        private bool firstInteractionStarted;
        private int dialogueIndex;

        private void Start() {
            firstInteractionStarted = false;
        }

        private void Update() {
            if (Teleportation.Instance.teleportState == TeleportState.VISIBLE && !firstInteractionStarted) {
                firstInteractionStarted = true;
                
                StartFirstInteraction();
            }
        }

        private void StartFirstInteraction() {
            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.DIALOGUES;
            InputManager.Instance.SwitchContext(GameContext.UI);
            MainCamera.Instance.SwitchToDialogueCamera();

            // mini chimp speak and subtitles
            PlayMiniChimpDialogue();
        }

        private void ShowRotatingBananaGunPanel() {
            rotatingBananaGunPanel.SetActive(true);
        }

        public void HideRotatingBananaGunPanel() {
            rotatingBananaGunPanel.SetActive(false);
            AcquireBananaGun();
            
            // mini chimp speak and subtitles
            PlayMiniChimpDialogue();
        }

        private void AcquireBananaGun() {
            BananaGun.Instance.bananaGunInBack.SetActive(false);
            UIManager.Instance.Hide_HUD();
        
            UIManager.Instance.Set_active(firstTutorialCanvasGroup, true);
            Invoke(nameof(firstTutorialVideos.Show), 1f);
        }

        public void NextDialogue() {
            dialogueIndex++;
            PlayMiniChimpDialogue();
        }

        private void PlayMiniChimpDialogue() {
            startInteractionMiniChimp.GetComponent<MiniChimp>().Speak();
            
            if (dialogueIndex == 4) {
                ShowRotatingBananaGunPanel();
            }
        }

        public void ValidateTutorial() {
            InputManager.Instance.SwitchContext(GameContext.GAME);
            MainCamera.Instance.SwitchToFreeLookCamera();
            
            startInteractionMiniChimp.SetActive(false);
            commandRoomMiniChimp.SetActive(true);
        }
    }
}
