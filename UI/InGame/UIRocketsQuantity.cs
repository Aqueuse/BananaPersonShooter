using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UIRocketsQuantity : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI rocketsQuantityText;
    
        public void Set_Rockets_Quantity(int quantity) {
            rocketsQuantityText.text = quantity > 999 ? "999+" : quantity.ToString();
        }
    }
}
