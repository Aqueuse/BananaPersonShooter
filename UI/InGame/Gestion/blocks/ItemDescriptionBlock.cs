using TMPro;
using UnityEngine;

namespace UI.InGame.Gestion.blocks {
    public class ItemDescriptionBlock : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;

        public void SetBlock(string itemName, string itemDescription) {
            this.itemName.text = itemName;
            this.itemDescription.text = itemDescription;
        }
    }
}
