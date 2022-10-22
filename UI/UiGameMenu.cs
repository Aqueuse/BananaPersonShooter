using UnityEngine;

namespace UI {
    public class UiGameMenu : MonoBehaviour {
        public void Quit() {
            GameManager.Instance.ReturnHome();
        }
    }
}
