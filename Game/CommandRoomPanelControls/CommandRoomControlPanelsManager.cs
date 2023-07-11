using Enums;
using Monkeys.MiniChimps;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        public Color activatedKeybard;
        public Color desactivatedKeybard;

        [SerializeField] private GenericDictionary<CommandRoomPanelType, CommandRoomPanel> panels;

        [SerializeField] private BoxCollider door1BoxCollider;
        [SerializeField] private BoxCollider door2BoxCollider;
        [SerializeField] private GameObject accessDeniedDoor1;
        [SerializeField] private GameObject accessDeniedDoor2;

        [SerializeField] private GameObject bananaCannonMiniGameAccessDenied;
        [SerializeField] private BoxCollider miniGameCanonBananaInteractionCollider;
        [SerializeField] private CanvasGroup miniGameCannonBananaInteractionCanvas;

        [SerializeField] private Assembler assembler;

        public MiniChimpDialogue miniChimpCommandRoomMiniChimpDialogue;
        [SerializeField] private LocalizeStringEvent _miniChimpLocalizeStringEvent;
        [SerializeField] private GenericDictionary<AdvancementState, LocalizedString> miniChimpTextByAdvancement;

        private void Start() {
            ShowHidePanel(CommandRoomPanelType.JOURNAL);
            
            if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.FEED_MONKEY)) {
                AuthorizeBananaCannonMiniGameAccess();
            }

            if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.ASPIRE_SOMETHING)) {
                AuthorizeDoorsAccess();
            }
            
            SetMiniChimpDialogue(ObjectsReference.Instance.advancements.GetBestAdvancement());
        }

        public void ShowHidePanel(CommandRoomPanelType commandRoomPanelType) {
            foreach (var commandRoomPanel in panels) {
                if (commandRoomPanel.Value.commandRoomPanelType == commandRoomPanelType) {
                    if (commandRoomPanel.Value.isVisible()) commandRoomPanel.Value.Desactivate();
                    else {
                        commandRoomPanel.Value.Activate();
                    }
                }
                else {
                    commandRoomPanel.Value.Desactivate();
                }
            }

            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
        }

        public void SetMiniChimpDialogue(AdvancementState advancementState) {
            _miniChimpLocalizeStringEvent.StringReference = miniChimpTextByAdvancement[advancementState];
        }
        
        public void AuthorizeDoorsAccess() {
            door1BoxCollider.enabled = true;
            door2BoxCollider.enabled = true;
            accessDeniedDoor1.SetActive(false);
            accessDeniedDoor2.SetActive(false);
        }
        
        private void AuthorizeBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(false);
            miniGameCanonBananaInteractionCollider.enabled = true;
            miniGameCannonBananaInteractionCanvas.alpha = 1;
        }

        public void SetAssemblerVolume(float level) {
            assembler.SetAssemblerAudioVolume(level);
        }
    }
}
