using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UIMonkey : MonoBehaviour {
        private Vector3 bananaManXY;
        
        private void Update() { 
            transform.LookAt(new Vector3(BananaMan.Instance.transform.position.x, transform.position.y, BananaMan.Instance.transform.position.z));
        }

        public void Add_Satiety(float satietyValue) {
            GetComponentInChildren<Slider>().value = satietyValue;
        }

        public void Show_Monkey_Life_Slider() {
            GetComponentInChildren<CanvasGroup>().alpha = 1f;
        }

        public void Hide_Monkey_Life_Slider() {
            GetComponentInChildren<CanvasGroup>().alpha = 0f;
        }

        public void SetMaxSatiety(float maxSatiety) {
            GetComponentInChildren<Slider>().maxValue = maxSatiety;
        }
    }
}
