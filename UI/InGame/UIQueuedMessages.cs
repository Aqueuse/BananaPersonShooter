using Data;
using Enums;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UI.InGame {
    public class UIQueuedMessages : MonoBehaviour {
        [SerializeField] private GameObject queuedMessagePrefab;
        [SerializeField] private LocalizedString nothingToRetrieve;

        private GenericDictionary<ItemCategory, LocalizedStringTable> translationTableByItemCategory;

        public void UnlockBlueprint(BuildableType buildableType) {
            ObjectsReference.Instance.uiQueuedMessages.AddMessage(
                LocalizationSettings.StringDatabase.GetTable("UI").GetEntry("unlocked").GetLocalizedString()+
                " "+
                LocalizationSettings.StringDatabase.GetTable("items").GetEntry(buildableType.ToString().ToLower()).GetLocalizedString()
            );
        }

        public void AddToInventory(ItemScriptableObject itemScriptableObject, int quantity) {
            ObjectsReference.Instance.uiQueuedMessages.AddMessage("+ "+ quantity+" "+ itemScriptableObject.GetName());
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
