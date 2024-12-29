using InGame.Items.ItemsBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIcommunication : MonoBehaviour {
        public SpaceshipBehaviour associatedSpaceshipBehaviour;

        [SerializeField] private Image communicationButtonImage;
        [SerializeField] private TextMeshProUGUI communicationButtonQuantity;

        private GameObject spaceshipMessage;
        
        public void ShowCommunication() {
            ObjectsReference.Instance.uiCommunicationPanel.ShowCommunicationMessage(associatedSpaceshipBehaviour);
        }
    }
}
