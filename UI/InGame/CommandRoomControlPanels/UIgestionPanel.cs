using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIgestionPanel : MonoBehaviour {
        [SerializeField] private GenericDictionary<RegionType, Image> mapCalquesByMapType;

        [SerializeField] private TextMeshProUGUI piratesQuantityText;
        [SerializeField] private TextMeshProUGUI visitorsQuantityText;
        [SerializeField] private TextMeshProUGUI chimployeesQuantityText;
        [SerializeField] private TextMeshProUGUI debrisQuantityText;

        public void RefreshWorldDataUI() {
            var world = ObjectsReference.Instance.gameData.worldData;

            piratesQuantityText.text = world.piratesQuantity.ToString();
            visitorsQuantityText.text = world.visitorsQuantity.ToString();
            chimployeesQuantityText.text = world.chimployeesQuantity.ToString();
            debrisQuantityText.text = (world.piratesDebris+world.visitorsDebris).ToString();
        }

        public void ShowMapCalque(RegionType regionType) {
            foreach (var mapCalque in mapCalquesByMapType) {
                mapCalque.Value.enabled = regionType == mapCalque.Key;
            }
        }
    }
}
