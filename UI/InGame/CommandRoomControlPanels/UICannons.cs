using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UICannons : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI noBananaText;
        [SerializeField] private TextMeshProUGUI bananaQuantity;
        [SerializeField] private Image bananaImage;

        public void SetBananaType(Sprite bananaImage, Color projectileColor) {
            this.bananaImage.sprite = bananaImage;
            bananaQuantity.color = projectileColor;
        }
        
        public void SetBananaQuantity(int bananaQuantity, BananaType bananaType) {
            if (bananaType == BananaType.EMPTY || ObjectsReference.Instance.bananasInventory.bananasInventory[bananaType] == 0) {
                ObjectsReference.Instance.uiCannons.noBananaText.alpha = 100;
                this.bananaQuantity.text = "0";
                return;
            }

            ObjectsReference.Instance.uiCannons.noBananaText.alpha = 0;
            this.bananaQuantity.text = bananaQuantity.ToString();
        }
    }
}
