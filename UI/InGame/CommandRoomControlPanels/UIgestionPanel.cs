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
        
        public void RefreshMapDataUI(SceneType sceneType) {
            var map = ObjectsReference.Instance.gameData.mapBySceneName[sceneType];

            piratesQuantityText.text = map.piratesQuantity.ToString();
            visitorsQuantityText.text = map.visitorsQuantity.ToString();
            chimployeesQuantityText.text = map.chimployeesQuantity.ToString();
            debrisQuantityText.text = (map.piratesDebris+map.visitorsDebris).ToString();
        }

        public void ShowMapCalque(SceneType sceneType) {
            foreach (var mapCalque in mapCalquesByMapType) {
                mapCalque.Value.enabled = sceneType == mapCalque.Key;
            }
        }
    }
}
