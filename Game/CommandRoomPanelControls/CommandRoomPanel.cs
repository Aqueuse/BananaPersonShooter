using UnityEngine;
using UnityEngine.UI;

namespace Game.CommandRoomPanelControls {
    public class CommandRoomPanel : MonoBehaviour {
        [SerializeField] private MeshRenderer keyboardMeshRenderer;
        [SerializeField] private CanvasGroup panelCanvasGroup;
        [SerializeField] private Image tabBackground;
        
        public CommandRoomPanelType commandRoomPanelType;

        public bool isVisible;
        
        private static readonly int KeyboardColorPropertie = Shader.PropertyToID("_Color");

        public void Activate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.activatedKeybard);
            panelCanvasGroup.alpha = 1;
            
            tabBackground.enabled = true;
            
            isVisible = true;
        }

        public void Desactivate() {
            keyboardMeshRenderer.material.SetColor(KeyboardColorPropertie, CommandRoomControlPanelsManager.Instance.desactivatedKeybard);
            panelCanvasGroup.alpha = 0;
            
            tabBackground.enabled = false;

            isVisible = false;
        }
    }
}
