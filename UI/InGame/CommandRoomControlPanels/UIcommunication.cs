using InGame.Items.ItemsBehaviours;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIcommunication : MonoBehaviour {
        public SpaceshipBehaviour associatedSpaceshipBehaviour;

        public void ShowCommunication() {
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.ShowCommunicationMessage(associatedSpaceshipBehaviour);
        }

        private void OnDestroy() {
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.RefreshCommunicationButton();
        }
    }
}
