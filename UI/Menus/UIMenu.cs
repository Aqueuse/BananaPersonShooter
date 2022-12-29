using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus {
    public class UIMenu : MonoBehaviour {
        [SerializeField] private GameObject[] tabs;
        [SerializeField] private Image[] tabsButtons;

        [SerializeField] Color activatedColor;
        [SerializeField] Color unactivatedColor;
    
        public void Switch_to_Tab(int index) {
            for (var i=0; i<tabs.Length; i++) {
                tabs[i].SetActive(false);
                tabsButtons[i].color = unactivatedColor;
            }
        
            tabs[index].SetActive(true);
            tabsButtons[index].color = activatedColor;
        }
    }
}
