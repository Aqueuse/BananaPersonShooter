using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomPanel : MonoBehaviour {
        [SerializeField] private MeshRenderer keyboardMeshRenderer;
        [SerializeField] private CanvasGroup panelCanvasGroup;
        
        public CommandRoomPanelType commandRoomPanelType;
        
        private static readonly int KeyboardColorPropertie = Shader.PropertyToID("_Color");
        
        public void Activate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.activatedKeybard);
            panelCanvasGroup.alpha = 1f;
        }

        public void Desactivate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.desactivatedKeybard);
            panelCanvasGroup.alpha = 0f;
        }

        public bool isVisible() {
            return panelCanvasGroup.alpha > 0.1f;
        }
    }
}
