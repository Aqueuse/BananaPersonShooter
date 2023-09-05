using Data;
using TMPro;
using UnityEngine;

namespace UI.InGame.Gestion {
    public class InventoryGestionPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;

        public void SetDescription(ItemScriptableObject itemScriptableObject) {
            itemName.text = itemScriptableObject.GetName();
            itemDescription.text = itemScriptableObject.GetDescription();
        }
    }
}