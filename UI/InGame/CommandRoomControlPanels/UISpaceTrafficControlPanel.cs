using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.CommandRoomControlPanels {
    public enum SpaceTrafficControlTab {
        COMMUNICATIONS,
        SPACE,
        CAMERAS
    }
    
    public class UISpaceTrafficControlPanel : MonoBehaviour {
        private Color activationColor;

        [SerializeField] private GenericDictionary<SpaceTrafficControlTab, Image> tabImagesBySpaceTrafficControlTab;
        [SerializeField] private GenericDictionary<SpaceTrafficControlTab, TextMeshProUGUI> tabTextBySpaceTrafficControlTab;
        [SerializeField] private GenericDictionary<SpaceTrafficControlTab, Transform> panelsBySpaceTrafficControlTab;
        
        private void Start() {
            activationColor = ObjectsReference.Instance.uiManager.activationColor;
        }

        public void SwitchToTab(SpaceTrafficControlTab spaceTrafficControlTab) {
            foreach (var panels in panelsBySpaceTrafficControlTab) {
                tabImagesBySpaceTrafficControlTab[panels.Key].color = activationColor;
                tabTextBySpaceTrafficControlTab[panels.Key].color = Color.black;
                panelsBySpaceTrafficControlTab[panels.Key].gameObject.SetActive(false);
            }
            
            tabImagesBySpaceTrafficControlTab[spaceTrafficControlTab].color = Color.black;
            tabTextBySpaceTrafficControlTab[spaceTrafficControlTab].color = activationColor;
            panelsBySpaceTrafficControlTab[spaceTrafficControlTab].gameObject.SetActive(true);
        }
    }
}
