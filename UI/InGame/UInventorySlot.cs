using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.InGame {
    public class UInventorySlot : MonoBehaviour, ISelectHandler {
        [SerializeField] private BananasDataScriptableObject bananasDataScriptableObject;
        [SerializeField] private GameObject quantityText;

        public void OnSelect(BaseEventData eventData) {
            SetDescription();
            BananaMan.Instance.activeBanana = bananasDataScriptableObject;
            BananaMan.Instance.activeBananaType = bananasDataScriptableObject.bananaType;
            
            UInventory.Instance.lastselectedInventoryItem = gameObject;
            
            UISlotsManager.Instance.AssignToSelectedSlot(bananasDataScriptableObject.bananaType);
        }
        
        public void SetQuantity(int quantity) {
            quantityText.GetComponent<TextMeshProUGUI>().text = quantity.ToString();
        }

        public void SetDescription() {
            UInventory.Instance.itemDescription.text = bananasDataScriptableObject.bananaDescription;
            UInventory.Instance.itemHealth.text = "health +"+bananasDataScriptableObject.healthBonus;
            UInventory.Instance.itemDamage.text = "damage +"+bananasDataScriptableObject.damage;
            UInventory.Instance.itemResistance.text = "resistance +"+bananasDataScriptableObject.resistanceBonus;
        }
    }
}
