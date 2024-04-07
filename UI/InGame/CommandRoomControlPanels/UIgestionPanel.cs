using UnityEngine;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIgestionPanel : MonoBehaviour {
        [SerializeField] private GameObject mapPanel;
        [SerializeField] private GameObject monkeysPanel;
        [SerializeField] private GameObject chimpsPanel;

        public void ShowMapPanel() {
            mapPanel.SetActive(true);
            monkeysPanel.SetActive(false);
            chimpsPanel.SetActive(false);
        }

        public void ShowMonkeysPanel() {
            mapPanel.SetActive(false);
            monkeysPanel.SetActive(true);
            chimpsPanel.SetActive(false);
        }

        public void ShowChimpsPanel() {
            mapPanel.SetActive(false);
            monkeysPanel.SetActive(false);
            chimpsPanel.SetActive(true);
        }
    }
}
