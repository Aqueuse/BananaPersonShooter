using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIgestionPanel : MonoBehaviour {
        [SerializeField] private GenericDictionary<SceneType, Image> mapCalquesByMapType;

        [SerializeField] private TextMeshProUGUI piratesQuantityText;
        [SerializeField] private TextMeshProUGUI visitorsQuantityText;
        [SerializeField] private TextMeshProUGUI chimployeesQuantityText;
        [SerializeField] private TextMeshProUGUI debrisQuantityText;
        
        public void RefreshMapDataUI(int piratesQuantity, int visitorsQuantity, int chimployeesQuantity, int debrisQuantity) {
            piratesQuantityText.text = piratesQuantity.ToString();
            visitorsQuantityText.text = visitorsQuantity.ToString();
            chimployeesQuantityText.text = chimployeesQuantity.ToString();
            debrisQuantityText.text = debrisQuantity.ToString();
        }
    }
}
