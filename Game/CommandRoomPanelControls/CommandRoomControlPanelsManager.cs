using Enums;
using Monkeys.Chimployees;
using Monkeys.MiniChimps;
using UI.InGame;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        [SerializeField] private BoxCollider door1BoxCollider;
        [SerializeField] private BoxCollider door2BoxCollider;
        [SerializeField] private GameObject accessDeniedDoor1;
        [SerializeField] private GameObject accessDeniedDoor2;
        
        [SerializeField] private GameObject mapAccessDoor1;
        [SerializeField] private GameObject mapAccessDoor2;

        [SerializeField] private GameObject bananaCannonMiniGameAccessDenied;
        [SerializeField] private BoxCollider miniGameCanonBananaInteractionCollider;
        [SerializeField] private ItemInteraction miniGameCannonBananaPlayInteraction;

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

        public void ForbidDoorsAccess() {
            door1BoxCollider.enabled = false;
            door2BoxCollider.enabled = false;
            
            accessDeniedDoor1.SetActive(true);
            accessDeniedDoor2.SetActive(true);
            
            mapAccessDoor1.SetActive(false);
            mapAccessDoor2.SetActive(false);
        }
        
        public void ForbidBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(true);
            miniGameCanonBananaInteractionCollider.enabled = false;
            miniGameCannonBananaPlayInteraction.HideUI();
        }

        public void AuthorizeDoorsAccess() {
            door1BoxCollider.enabled = true;
            door2BoxCollider.enabled = true;
            
            accessDeniedDoor1.SetActive(false);
            accessDeniedDoor2.SetActive(false);
            
            mapAccessDoor1.SetActive(true);
            mapAccessDoor2.SetActive(true);
        }
        
        public void AuthorizeBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(false);
            miniGameCanonBananaInteractionCollider.enabled = true;
            miniGameCannonBananaPlayInteraction.ShowUI();
        }
    }
}
