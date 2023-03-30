using Enums;
using Save;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        [SerializeField] private GameObject bananaGun;
        public Color activatedKeybard;
        public Color desactivatedKeybard;
        
        [SerializeField] private GenericDictionary<CommandRoomPanelType, CommandRoomPanel> panels;


        private void Start() {
            if (GameData.Instance.bananaManSavedData.advancementState == AdvancementState.NEW_GAME) {
                SetBananaGunVisibility(true);
            }
        }

        public void SetBananaGunVisibility(bool isVisible) {
            bananaGun.SetActive(isVisible);
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
    }
}
