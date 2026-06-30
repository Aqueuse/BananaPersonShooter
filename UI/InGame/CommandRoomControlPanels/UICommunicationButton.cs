using InGame.Items.ItemsBehaviours;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UICommunicationButton : MonoBehaviour {
        public SpaceshipBehaviour associatedSpaceshipBehaviour;

        public void ShowCommunication() {
            ObjectsReference.Instance.uiCommunicationPanel.ShowCommunicationMessage(associatedSpaceshipBehaviour);
        }
    }
}