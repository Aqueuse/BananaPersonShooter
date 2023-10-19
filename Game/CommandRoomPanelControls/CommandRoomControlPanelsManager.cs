using Enums;
using Interactions;
using Monkeys.Chimployees;
using Monkeys.MiniChimps;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        [SerializeField] private GameObject bananaCannonMiniGameAccessDenied;
        [SerializeField] private BoxCollider miniGameCanonBananaInteractionCollider;
        [SerializeField] private Interaction miniGameCannonBananaPlayInteraction;

        public Light[] gardensLight;

        public Color activatedKeybard;
        public Color desactivatedKeybard;

        [SerializeField] private GenericDictionary<CommandRoomPanelType, CommandRoomPanel> commandRoomPanels;
        
        public Assembler assembler;

        public ChimployeeCommandRoom chimployeeCommandRoom;
        public Transform apeResourcesChimployeeTransform;

        public MiniChimp miniChimp;

        public void ShowPanel(CommandRoomPanelType commandRoomPanelType) {
            commandRoomPanels[commandRoomPanelType].Activate();
        }

        public void HidePanel(CommandRoomPanelType commandRoomPanelType) {
            commandRoomPanels[commandRoomPanelType].Desactivate();
        }

        public void ShowHidePanel(CommandRoomPanelType commandRoomPanelType) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) return;
            
            if (commandRoomPanels[commandRoomPanelType].isVisible()) commandRoomPanels[commandRoomPanelType].Desactivate();
            else {
                commandRoomPanels[commandRoomPanelType].Activate();
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
        }
        
        public void SetAssemblerVolume(float level) {
            assembler.SetAssemblerAudioVolume(level);
        }
        
        public void ForbidBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(true);
            miniGameCanonBananaInteractionCollider.enabled = false;
            miniGameCannonBananaPlayInteraction.HideUI();
        }
        
        public void AuthorizeBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(false);
            miniGameCanonBananaInteractionCollider.enabled = true;
            miniGameCannonBananaPlayInteraction.ShowUI();
        }
    }
}
