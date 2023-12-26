using ItemsProperties;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.InGame {
    public class UIQueuedMessages : MonoBehaviour {
        [SerializeField] private GameObject queuedMessagePrefab;
        [SerializeField] private LocalizedString nothingToRetrieve;

        private GenericDictionary<ItemCategory, LocalizedStringTable> translationTableByItemCategory;
        
        public void AddToInventory(ItemScriptableObject itemScriptableObject, int quantity) {
            ObjectsReference.Instance.uiQueuedMessages.AddMessage("+ "+ quantity+" "+ itemScriptableObject.GetName());
        }

        public void RemoveFromInventory(ItemScriptableObject itemScriptableObject, int quantity) {
            ObjectsReference.Instance.uiQueuedMessages.AddMessage("- "+ quantity+" "+ itemScriptableObject.GetName());
        }

        public void AddMessage(string message) {
            var queuedMessage = Instantiate(queuedMessagePrefab, transform);
        
            queuedMessage.GetComponent<UIMessage>().SetMessage(message);
        }

        public void AddNothingToRetrieveMessage() {
            AddMessage(nothingToRetrieve.GetLocalizedString());
        }
    }
}
