using UnityEngine;

namespace UI.InGame.MainBlock {
    public class CommandRoomPanelButton : MonoBehaviour {
        [SerializeField] private CommandRoomPanelType commandRoomPanelType;

        private bool isSelected;

        public void FocusUnfocus() {
            if (isSelected) {
                isSelected = false;
                ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel(true);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }

            else {
                isSelected = true;
                ObjectsReference.Instance.commandRoomControlPanelsManager.FocusPanel((int)commandRoomPanelType);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
    }
}
