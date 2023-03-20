using Data.Craftables;
using Game;
using TMPro;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building.Plateforms {
    public class GrabbableItem : MonoBehaviour {
        public CraftableDataScriptableObject craftableDataScriptableObject;
        [SerializeField] private TextMeshProUGUI quantityText;

        public int quantity;
        
        void Start() {
            quantity = 1;
            quantityText.text = "("+quantity+")";
        }

        public void AddQuantity(int addedQuantity) {
            quantity += addedQuantity;
            quantityText.text = "("+quantity+")";
        }

        public void GrabPrintedItem() {
            Inventory.Instance.AddQuantity(craftableDataScriptableObject.itemThrowableType, quantity);

            if (quantity == 1) {
                var message = "+ "+quantity+" " +LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("platform");
                UIQueuedMessages.Instance.AddMessage(message);
            }

            else {
                var message = "+ "+quantity+" " +LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("platforms");
                UIQueuedMessages.Instance.AddMessage(message);
            }
            
            BuildStation.Instance.RemovePlatform();
        }
    }
}
