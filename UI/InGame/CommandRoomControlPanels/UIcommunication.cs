using InGame.Items.ItemsBehaviours.SpaceshipsBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIcommunication : MonoBehaviour {
        public SpaceshipBehaviour associatedSpaceshipBehaviour;

        public void ShowCommunication() {
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.ShowCommunicationMessage(associatedSpaceshipBehaviour, this.GetComponent<Button>());
        }
    }
}
