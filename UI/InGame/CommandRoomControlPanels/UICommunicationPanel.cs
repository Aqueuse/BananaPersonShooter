using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using TMPro;
using Tweaks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UICommunicationPanel : MonoBehaviour {
        [SerializeField] private CanvasGroup answersListCanvasGroup;
        [SerializeField] private CanvasGroup messagePlaceholderCanvasGroup;

        [SerializeField] private Transform spaceshipsListContainer;

        [SerializeField] private GameObject spaceshipButtonPrefab;

        [SerializeField] private GenericDictionary<int, Button> hangarButtonsByHangarNumber;
        
        public GenericDictionary<CharacterType, List<GameObject>> spaceshipMessagesByCharacterType;
        
        private GameObject spaceshipMessage;
        
        public void AddNewCommunication(SpaceshipBehaviour spaceshipBehaviour) {
            var spaceshipButton = Instantiate(spaceshipButtonPrefab, spaceshipsListContainer, false);
            
            spaceshipButton.GetComponent<UICommunicationButton>().associatedSpaceshipBehaviour = spaceshipBehaviour;
            spaceshipButton.GetComponentInChildren<TextMeshProUGUI>().text = spaceshipBehaviour.spaceshipData.spaceshipName;
            spaceshipButton.GetComponent<Image>().color = spaceshipBehaviour.spaceshipData.spaceshipUIcolor;
            
            ObjectsReference.Instance.communicationsManager.RefreshCommunicationQuantityButton();
        }
        
        public void ShowCommunicationMessage(SpaceshipBehaviour spaceshipBehaviour) {
            ObjectsReference.Instance.spaceTrafficControlManager.selectedSpaceship = spaceshipBehaviour;
            
            UITweaks.SetCanvasGroupActif(answersListCanvasGroup, true);
            
            if (spaceshipMessage != null) spaceshipMessage.SetActive(false);
            
            // TODO : replace with text and regex espacing
            // Pour les caractères Unicode UTF-16 (format \uXXXX)
            // string texteAvecUnicode = rawMaterial.Key.spriteAtlasIndex;
            // var tokiPonaEscaped = Regex.Unescape(texteAvecUnicode);
            
            spaceshipMessage = spaceshipMessagesByCharacterType
                [spaceshipBehaviour.spaceshipData.characterType]
                [spaceshipBehaviour.spaceshipData.communicationMessagePrefabIndex]; 
            
            spaceshipMessage.SetActive(true);
            UITweaks.SetCanvasGroupActif(messagePlaceholderCanvasGroup, false);
        }
        
        public void CloseCommunications(SpaceshipBehaviour spaceshipBehaviour) {
            foreach (var uiCommunicationButton in spaceshipsListContainer.GetComponentsInChildren<UICommunicationButton>()) {
                if (uiCommunicationButton.associatedSpaceshipBehaviour == spaceshipBehaviour) {
                    Destroy(uiCommunicationButton.gameObject);
                }
            }

            UITweaks.SetCanvasGroupActif(answersListCanvasGroup, false);
            
            if (spaceshipMessage != null) spaceshipMessage.SetActive(false);
            
            UITweaks.SetCanvasGroupActif(messagePlaceholderCanvasGroup, true);
            
            ObjectsReference.Instance.communicationsManager.RefreshCommunicationQuantityButton();
        }
        
        public void RefreshHangarAvailability() {
            foreach (var hangar in ObjectsReference.Instance.spaceTrafficControlManager.hangarAvailabilityByHangarNumber) {
                if (hangar.Value) { // hangar available
                    hangarButtonsByHangarNumber[hangar.Key].interactable = true;
                    hangarButtonsByHangarNumber[hangar.Key].GetComponent<Image>().color = ObjectsReference.Instance.communicationsManager.hangarAvailableColor;
                }

                else {
                    hangarButtonsByHangarNumber[hangar.Key].interactable = false;
                    hangarButtonsByHangarNumber[hangar.Key].GetComponent<Image>().color = ObjectsReference.Instance.communicationsManager.hangarUnavailableColor;
                }
            }
        }
    }
}
