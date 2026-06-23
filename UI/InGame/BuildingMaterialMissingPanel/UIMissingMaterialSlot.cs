using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.BuildingMaterialMissingPanel {
    public class UIMissingMaterialSlot : MonoBehaviour {
        [SerializeField] private Image materialImage;
        [SerializeField] private TextMeshProUGUI materialQuantityText;

        public void SetMaterialSlot(Sprite materialSprite, string materialQuantity) {
            materialImage.sprite = materialSprite;
            materialQuantityText.text = materialQuantity;
        }
    }
}
