using TMPro;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UICannons : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI noBananaText;
        [SerializeField] private GenericDictionary<BananaEffect, TextMeshProUGUI> slidersByBananaGoopType;
        
        public void RefreshBananaGoopsQuantity() {
            foreach (var bananaGoop in ObjectsReference.Instance.cannonsManager.bananaGoopInventory.bananaGoopInventory) {
                slidersByBananaGoopType[bananaGoop.Key].text = bananaGoop.Value.ToString();
            }
        }
    }
}
