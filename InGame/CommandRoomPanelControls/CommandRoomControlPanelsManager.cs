using InGame.MiniGames.MarketingCampaignMiniGame;
using InGame.Monkeys.Chimployees;
using InGame.Monkeys.Minichimps;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
        public Light[] gardensLight;

        public Color activatedKeybard;
        public Color activablePanelKeybard;
        public Color desactivatedKeybard;

        [SerializeField] private GenericDictionary<CommandRoomPanelType, CommandRoomPanel> commandRoomPanels;
        
        public Assembler assembler;
        public UIMarketingPanel uIMarketingPanel;
        public GestionPanel gestionPanel;
        public MarketingCampaignManager marketingCampaignManager;

        public ChimployeeCommandRoom chimployeeCommandRoom;
        public Transform apeResourcesChimployeeTransform;

        public MiniChimp miniChimp;
        
        public void ShowPanel(CommandRoomPanelType commandRoomPanelType) {
            commandRoomPanels[commandRoomPanelType].Activate();
        }
        
        public void ShowHidePanel(CommandRoomPanelType commandRoomPanelType) {
            if (commandRoomPanels[commandRoomPanelType].isVisible()) 
                commandRoomPanels[commandRoomPanelType].Desactivate();
            else {
                commandRoomPanels[commandRoomPanelType].Activate();
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.BUTTON_INTERACTION, 0);
        }
    }
}
