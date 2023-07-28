using Monkeys.MiniChimps;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        public Color activatedKeybard;
        public Color desactivatedKeybard;

        [SerializeField] private GenericDictionary<CommandRoomPanelType, CommandRoomPanel> commandRoomPanels;

        [SerializeField] private BoxCollider door1BoxCollider;
        [SerializeField] private BoxCollider door2BoxCollider;
        [SerializeField] private GameObject accessDeniedDoor1;
        [SerializeField] private GameObject accessDeniedDoor2;

        [SerializeField] private GameObject bananaCannonMiniGameAccessDenied;
        [SerializeField] private BoxCollider miniGameCanonBananaInteractionCollider;
        [SerializeField] private CanvasGroup miniGameCannonBananaInteractionCanvas;

        public Assembler assembler;

        public MiniChimpDialogue commandRoomMiniChimpDialogue;
        [SerializeField] private LocalizeStringEvent _miniChimpLocalizeStringEvent;
        [SerializeField] private GenericDictionary<miniChimpDialogue, LocalizedString> miniChimpTextByAdvancement;

        private void Start() {
            ShowHidePanel(CommandRoomPanelType.JOURNAL);

            if (!ObjectsReference.Instance.bananaMan.hasRepairedBananaGun) {
                SetMiniChimpDialogue(miniChimpDialogue.REPAIR_BANANA_GUN);
                ForbidDoorsAccess();
                ForbidBananaCannonMiniGameAccess();
            }

            else {
                if (!ObjectsReference.Instance.bananaMan.tutorialFinished) {
                    SetMiniChimpDialogue(miniChimpDialogue.ASPIRE_CHIMPLOYEE);
                    ForbidDoorsAccess();
                    ForbidBananaCannonMiniGameAccess();
                }
                
                else {
                    SetMiniChimpDialogue(miniChimpDialogue.BANANA_ON_PLATEFORM);
                }
            }
        }
        
        public void ShowHidePanel(CommandRoomPanelType commandRoomPanelType) {
            if (commandRoomPanels[commandRoomPanelType].isVisible()) commandRoomPanels[commandRoomPanelType].Desactivate();
            else {
                commandRoomPanels[commandRoomPanelType].Activate();
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
        }

        public void SetMiniChimpDialogue(miniChimpDialogue miniChimpDialogue) {
            _miniChimpLocalizeStringEvent.StringReference = miniChimpTextByAdvancement[miniChimpDialogue];
        }
        
        public void AuthorizeDoorsAccess() {
            door1BoxCollider.enabled = true;
            door2BoxCollider.enabled = true;
            accessDeniedDoor1.SetActive(false);
            accessDeniedDoor2.SetActive(false);
        }
        
        public void AuthorizeBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(false);
            miniGameCanonBananaInteractionCollider.enabled = true;
            miniGameCannonBananaInteractionCanvas.alpha = 1;
        }

        private void ForbidDoorsAccess() {
            door1BoxCollider.enabled = false;
            door2BoxCollider.enabled = false;
            accessDeniedDoor1.SetActive(true);
            accessDeniedDoor2.SetActive(true);
        }

        private void ForbidBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(true);
            miniGameCanonBananaInteractionCollider.enabled = false;
            miniGameCannonBananaInteractionCanvas.alpha = 0;
        }

        public void SetAssemblerVolume(float level) {
            assembler.SetAssemblerAudioVolume(level);
        }
    }
}
