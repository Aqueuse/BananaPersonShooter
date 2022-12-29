using UnityEngine;

namespace UI.Menus {
    public class UiGameMenu : MonoBehaviour {
        public void Quit() {
            GameManager.Instance.ReturnHome();
        }
    }
}
