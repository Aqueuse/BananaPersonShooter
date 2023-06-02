using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        public Color activatedKeybard;
        public Color desactivatedKeybard;
        
        [SerializeField] private GenericDictionary<CommandRoomPanelType, CommandRoomPanel> panels;


        private void Start() {
            if (!ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_BANANAGUN)) {
                ShowHidePanel(CommandRoomPanelType.GOALS);
            }
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
