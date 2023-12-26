using TMPro;
using UnityEngine;

namespace UI.InGame.Gestion.blocks {
    public class ItemDescriptionBlock : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private GameObject aditionnalDataBlock;
        [SerializeField] private TextMeshProUGUI aditionnalDataBlockText;

        public void SetBlock(string itemName, string itemDescription, bool hasAdditionnalData, string additionnalDataText = "") {
            this.itemName.text = itemName;
            this.itemDescription.text = itemDescription;
            if (hasAdditionnalData) {
                aditionnalDataBlock.SetActive(true);
                aditionnalDataBlockText.text = additionnalDataText;
            }
            else {
                aditionnalDataBlock.SetActive(false);
            }
        }
    }
}
