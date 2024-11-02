using TMPro;
using UnityEngine;

namespace InGame.Dialogues {
    public class MiniChimpDialoguesManager : MonoBehaviour {
        [SerializeField] private GameObject sentenceText;
        [SerializeField] private GameObject nextSentenceButton;
        [SerializeField] private RectTransform responsesChoicesContainer;

        [SerializeField] private GameObject responseChoicePrefab;

        [SerializeField] private DialogueSetScriptableObject initialDialogueSetScriptableObject;
        
        private DialogueSetScriptableObject currentDialogueSetScriptableObject;
        public int sentenceIndex;
        
        public void ChooseResponse(DialogueSetScriptableObject dialogueSetScriptableObject) {
            currentDialogueSetScriptableObject = dialogueSetScriptableObject;

            sentenceIndex = 0;

            var unescapedString = System.Text.RegularExpressions.Regex.Unescape(currentDialogueSetScriptableObject
                .sentencesSequences[sentenceIndex].stringList[ObjectsReference.Instance.gameSettings.languageIndexSelected]);
            sentenceText.GetComponent<TextMeshProUGUI>().text = unescapedString;
                
            nextSentenceButton.SetActive(currentDialogueSetScriptableObject.sentencesSequences.Count > 1);
                
            foreach (var responseChoiceButton in responsesChoicesContainer.GetComponentsInChildren<ResponseChoiceButton>()) {
                Destroy(responseChoiceButton.gameObject);
            }
                
            // set responsesChoices
            foreach (var dialogueSet in currentDialogueSetScriptableObject.dialogueSetScriptableObjectsByResponsesChoices) {
                var responseChoice = Instantiate(responseChoicePrefab, responsesChoicesContainer);
                responseChoice.GetComponent<ResponseChoiceButton>().associatedDialogueSet = dialogueSet.Value;
                
                responseChoice.GetComponent<ResponseChoiceButton>().responseText.text =
                    dialogueSet.Key.stringList[ObjectsReference.Instance.gameSettings.languageIndexSelected];
            }
        }
        
        public void NextSentence() {
            sentenceIndex += 1;
            
            if (sentenceIndex <= currentDialogueSetScriptableObject.sentencesSequences.Count-1) {
                var unescapedString = System.Text.RegularExpressions.Regex.Unescape(currentDialogueSetScriptableObject
                    .sentencesSequences[sentenceIndex].stringList[ObjectsReference.Instance.gameSettings.languageIndexSelected]);
                sentenceText.GetComponent<TextMeshProUGUI>().text = unescapedString;
            }

            if (sentenceIndex == currentDialogueSetScriptableObject.sentencesSequences.Count - 1) {
                nextSentenceButton.SetActive(false);
            }
        }

        public void ResetDialogue() {
            ChooseResponse(initialDialogueSetScriptableObject);
        }
    }
}
