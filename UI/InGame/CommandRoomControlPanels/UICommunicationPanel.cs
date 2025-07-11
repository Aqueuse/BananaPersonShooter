using System.Collections.Generic;
using InGame.Items.ItemsBehaviours;
using InGame.MiniGames.MarketingCampaign;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UICommunicationPanel : MonoBehaviour {
        private const float campaignWorldsBoxShrinkedWidth = 104.04f;
        private const float campaignWorldsBoxExpandedWidth = 146.6995f;

        [SerializeField] private RectTransform currentCampaignRectTransform;
        
        [SerializeField] private GameObject launchCampaignDownButton;
        [SerializeField] private GameObject worldsBoxScrollList;
        
        [SerializeField] private Transform currentCampaignContainer;
        [SerializeField] private Transform adWorldsListContainer;

        [SerializeField] private GameObject communicationsPanel;

        private readonly Vector3 currentCampaignContainerBoxSize = new (0.576944f, 0.576944f, 0.576944f);
        private readonly Vector3 AdWorldsListContainerBoxSize = Vector3.one;
        
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
        
        public void MoveAdWordBoxToCurrentCampaign(AdWordBox adWordBox) {
            adWordBox.transform.SetParent(currentCampaignContainer);
            adWordBox.GetComponent<RectTransform>().localScale = currentCampaignContainerBoxSize;
        }

        public void MoveAdWordBoxToList(AdWordBox adWordBox) {
            adWordBox.transform.SetParent(adWorldsListContainer);
            adWordBox.GetComponent<RectTransform>().localScale = AdWorldsListContainerBoxSize;
        }
        
        public void ShowCampaignCreatorTools() {
            worldsBoxScrollList.SetActive(true);
            launchCampaignDownButton.SetActive(true);
            
            currentCampaignRectTransform.localPosition = new Vector3(-0.4469f, 0.238f);
            currentCampaignRectTransform.sizeDelta =
                new Vector2(campaignWorldsBoxShrinkedWidth, currentCampaignRectTransform.sizeDelta.y);
        }
        
        public void HideCampaignCreationTools() {
            worldsBoxScrollList.SetActive(false);
            launchCampaignDownButton.SetActive(false);
            
            currentCampaignRectTransform.localPosition = new Vector3(-0f, 0.238f);
            currentCampaignRectTransform.sizeDelta =
                new Vector2(campaignWorldsBoxExpandedWidth, currentCampaignRectTransform.sizeDelta.y);
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
            
            if (spaceshipMessage != null) spaceshipMessage.SetActive(false);
            
            // TODO : replace with text and regex espacing
            // cf solution found by perplexity
            // Pour les caractères Unicode UTF-16 (format \uXXXX)
            // string texteAvecUnicode = rawMaterial.Key.spriteAtlasIndex;
            // var tokiPonaEscaped = Regex.Unescape(texteAvecUnicode);
            
            spaceshipMessage = spaceshipMessagesByCharacterType
                [spaceshipBehaviour.spaceshipData.characterType]
                [spaceshipBehaviour.spaceshipData.communicationMessagePrefabIndex]; 
            
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
        
        public void SwitchToAdCampaignTab() {
            communicationsPanel.SetActive(false);
        }
            
        public void SwitchToCommunicationsTab() {
            communicationsPanel.SetActive(true);
        }
    }
}
