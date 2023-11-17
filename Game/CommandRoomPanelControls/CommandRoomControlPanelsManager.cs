using System;
using Enums;
using Interactions;
using Monkeys.Chimployees;
using Monkeys.MiniChimps;
using Tags;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoSingleton<CommandRoomControlPanelsManager> {
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
        
        public void ShowHidePanel(CommandRoomPanelType commandRoomPanelType) {
            if (commandRoomPanels[commandRoomPanelType].isVisible()) commandRoomPanels[commandRoomPanelType].Desactivate();
            else {
                commandRoomPanels[commandRoomPanelType].Activate();
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(EffectType.BUTTON_INTERACTION, 0);
        }
        
        public void SetAssemblerVolume(float level) {
            assembler.SetAssemblerAudioVolume(level);
        }
    }
}
