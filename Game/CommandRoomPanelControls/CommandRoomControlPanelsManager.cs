using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        public Color activatedKeybard;
        public Color desactivatedKeybard;
        
        [SerializeField] private GenericDictionary<CommandRoomPanelType, CommandRoomPanel> panels;

        [SerializeField] private BoxCollider door1BoxCollider;
        [SerializeField] private BoxCollider door2BoxCollider;
        
        [SerializeField] private GameObject bananaCannonMiniGameAccessDenied;
        [SerializeField] private BoxCollider miniGameCanonBananaInteractionCollider;
        [SerializeField] private CanvasGroup miniGameCannonBananaInteractionCanvas;

        private void Start() {
            ShowHidePanel(CommandRoomPanelType.JOURNAL);
        }

        public void ShowHidePanel(CommandRoomPanelType commandRoomPanelType) {
            foreach (var commandRoomPanel in panels) {
                commandRoomPanel.Value.Desactivate();
            }

            if (panels[commandRoomPanelType].isVisible) {
                panels[commandRoomPanelType].Desactivate();
            }
            else {
                panels[commandRoomPanelType].Activate();
            }
        }

        public void AuthorizeDoorsAccess() {
            door1BoxCollider.enabled = true;
            door2BoxCollider.enabled = true;
        }
        
        public void AuthorizeBananaCannonMiniGameAccess() {
            bananaCannonMiniGameAccessDenied.SetActive(false);
            miniGameCanonBananaInteractionCollider.enabled = true;
            miniGameCannonBananaInteractionCanvas.alpha = 1;
        }

    }
}
