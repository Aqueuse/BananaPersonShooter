using Enums;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomPanel : MonoBehaviour {
        [SerializeField] private MeshRenderer keyboardMeshRenderer;
        [SerializeField] private GameObject panel; 
        
        public CommandRoomPanelType commandRoomPanelType;
        
        private static readonly int KeyboardColorPropertie = Shader.PropertyToID("_emission_color");
        private static readonly int KeyboardEmissionStrengthPropertie = Shader.PropertyToID("_emission");
        
        public void Activate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.activatedKeybard);
            keyboardMeshRenderer.material.SetFloat(KeyboardEmissionStrengthPropertie, 1);
            
            panel.SetActive(true);
        }

        public void Desactivate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.desactivatedKeybard);
            keyboardMeshRenderer.material.SetFloat(KeyboardEmissionStrengthPropertie, 0);

            panel.SetActive(false);
        }

        public bool isVisible() {
            return panel.activeInHierarchy;
        }
    }
}
