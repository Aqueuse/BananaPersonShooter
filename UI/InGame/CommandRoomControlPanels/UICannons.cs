using TMPro;
using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UICannons : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI noBananaText;
        [SerializeField] private TextMeshProUGUI bananaQuantityText;
        
        public void RefreshBananaGoopsQuantity() {
            bananaQuantityText.text = ObjectsReference.Instance.cannonsManager.bananaGoopQuantity.ToString();
        }
    }
}
