using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIcommunication : MonoBehaviour {
        public SpaceshipBehaviour associatedSpaceshipBehaviour;

        [SerializeField] private Image communicationButtonImage;
        [SerializeField] private TextMeshProUGUI communicationButtonQuantity;

        
        [SerializeField] private Transform spaceshipsListContainer;
        
        [SerializeField] private GameObject messagePlaceholder;
        
        [SerializeField] private CanvasGroup answersListCanvasGroup;
        
        [SerializeField] private GameObject spaceshipButtonPrefab;

        [SerializeField] private GenericDictionary<int, Button> hangarButtonsByHangarNumber;

        [SerializeField] private Color hangarAvailableColor;
        [SerializeField] private Color hangarUnavailableColor;
        
        public GenericDictionary<CharacterType, List<GameObject>> spaceshipMessagesByCharacterType;
        
        private GameObject spaceshipMessage;
        
        public void ShowCommunication() {
            ShowCommunicationMessage(associatedSpaceshipBehaviour);
        }
        
                public void AddNewCommunication(SpaceshipBehaviour spaceshipBehaviour) {
            var spaceshipButton = Instantiate(spaceshipButtonPrefab, spaceshipsListContainer, false);
            
            spaceshipButton.GetComponent<UIcommunication>().associatedSpaceshipBehaviour = spaceshipBehaviour;
            spaceshipButton.GetComponentInChildren<TextMeshProUGUI>().text = spaceshipBehaviour.spaceshipData.spaceshipName;
            spaceshipButton.GetComponent<Image>().color = spaceshipBehaviour.spaceshipData.spaceshipUIcolor;
            
            RefreshCommunicationButton();
        }
        
        public void ShowCommunicationMessage(SpaceshipBehaviour spaceshipBehaviour) {
            ObjectsReference.Instance.spaceTrafficControlManager.selectedSpaceship = spaceshipBehaviour;

            answersListCanvasGroup.alpha = 1;
            answersListCanvasGroup.interactable = true;
            answersListCanvasGroup.blocksRaycasts = true;

            // if (spaceshipMessage != null) spaceshipMessage.SetActive(false);
            // spaceshipMessage = spaceshipMessagesByCharacterType
            //     [spaceshipBehaviour.spaceshipData.characterType]
            //     [spaceshipBehaviour.spaceshipData.communicationMessagePrefabIndex]; 
            
            spaceshipMessage.SetActive(true);
            messagePlaceholder.SetActive(false);
        }

        public void CloseCommunicationsFromUI() {
            CloseCommunications(ObjectsReference.Instance.spaceTrafficControlManager.selectedSpaceship);
        }

        public void CloseCommunications(SpaceshipBehaviour spaceshipBehaviour) {
            foreach (var uIcommunication in spaceshipsListContainer.GetComponentsInChildren<UIcommunication>()) {
                if (uIcommunication.associatedSpaceshipBehaviour == spaceshipBehaviour) {
                    Destroy(uIcommunication.gameObject); 
                }
            }

            answersListCanvasGroup.alpha = 0;
            answersListCanvasGroup.interactable = false;
            answersListCanvasGroup.blocksRaycasts = false;
            
            if (spaceshipMessage != null) spaceshipMessage.SetActive(false);
            messagePlaceholder.SetActive(true);
            
            RefreshCommunicationButton();
        }
        
        public void RefreshHangarAvailability() {
            foreach (var hangar in ObjectsReference.Instance.spaceTrafficControlManager.hangarAvailabilityByHangarNumber) {
                if (hangar.Value) { // hangar available
                    hangarButtonsByHangarNumber[hangar.Key].interactable = true;
                    hangarButtonsByHangarNumber[hangar.Key].GetComponent<Image>().color = hangarAvailableColor;
                }

                else {
                    hangarButtonsByHangarNumber[hangar.Key].interactable = false;
                    hangarButtonsByHangarNumber[hangar.Key].GetComponent<Image>().color = hangarUnavailableColor;
                }
            }
        }

        public void RefreshCommunicationButton() {
            var communicationsQuantity = spaceshipsListContainer.GetComponentsInChildren<UIcommunication>().Length;

            communicationButtonImage.color = communicationsQuantity > 0 ? hangarUnavailableColor : hangarAvailableColor;

            communicationButtonQuantity.text = "(" + communicationsQuantity + ")";
        }
    }
}
