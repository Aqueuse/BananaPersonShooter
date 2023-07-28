using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomPanel : MonoBehaviour {
        [SerializeField] private MeshRenderer keyboardMeshRenderer;
        [SerializeField] private GameObject panel; 
        
        public CommandRoomPanelType commandRoomPanelType;
        
        private static readonly int KeyboardColorPropertie = Shader.PropertyToID("_Color");
        
        public void Activate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.activatedKeybard);
            panel.SetActive(true);
        }

        public void Desactivate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.desactivatedKeybard);
            panel.SetActive(false);
        }

        public bool isVisible() {
            return panel.activeInHierarchy;
        }
    }
}
