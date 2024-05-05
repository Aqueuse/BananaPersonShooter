using InGame.Items.ItemsBehaviours.SpaceshipsBehaviours;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIcommunication : MonoBehaviour {
        public SpaceshipBehaviour associatedSpaceshipBehaviour;

        public void ShowCommunication() {
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.ShowCommunicationMessage(associatedSpaceshipBehaviour);
        }
    }
}
