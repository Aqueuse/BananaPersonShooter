using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public class UIgestionPanel : MonoBehaviour {
        [SerializeField] private GameObject stationPanel;
        [SerializeField] private GameObject monkeysPanel;
        [SerializeField] private GameObject chimpsPanel;

        [SerializeField] private Slider lightSlider;
        [SerializeField] private Image lightSliderBackgroundImage;
        [SerializeField] private Color lightOnSliderColor;
        [SerializeField] private Color lightOffSliderColor;

        public void ShowMapPanel() {
            stationPanel.SetActive(true);
            monkeysPanel.SetActive(false);
            chimpsPanel.SetActive(false);
        }

        public void ShowMonkeysPanel() {
            stationPanel.SetActive(false);
            monkeysPanel.SetActive(true);
            chimpsPanel.SetActive(false);
        }

        public void ShowChimpsPanel() {
            stationPanel.SetActive(false);
            monkeysPanel.SetActive(false);
            chimpsPanel.SetActive(true);
        }

        public void SwitchLight(float lightValue) {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_HOME) return;
            
            if (lightValue < 1) {
                lightSlider.value = 0;
                lightSliderBackgroundImage.color = lightOffSliderColor;
                ObjectsReference.Instance.gameManager.SwitchToNightLightSetting();
                ObjectsReference.Instance.worldData.stationLightSetting = 0;
            }

            else {
                lightSlider.value = 1;
                lightSliderBackgroundImage.color = lightOnSliderColor;
                RenderSettings.ambientLight = Color.white;
                ObjectsReference.Instance.worldData.stationLightSetting = 1;
            }
        }
    }
}
