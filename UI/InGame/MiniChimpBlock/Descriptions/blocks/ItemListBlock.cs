using TMPro;
using UnityEngine;

namespace UI.InGame.MiniChimpBlock.Descriptions.blocks {
    public class ItemListBlock : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemList;

        public void SetBlock(string itemName, int[] listItemsQuantities, string[] listItems) {
            this.itemName.text = itemName;
            itemList.text = "";

            for (var i = 0; i < listItemsQuantities.Length; i++) {
                itemList.text +=
                    "x " +
                    listItemsQuantities[i] +
                    " " +
                    listItems[i] +
                    "\n";
            }
        }
    
    
    }
}
