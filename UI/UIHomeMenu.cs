using UnityEngine;

namespace UI {
    public class UIHomeMenu : MonoBehaviour {
        public void Play() {
            GameManager.Instance.Play();
        }

        public void Quit() {
            GameManager.Instance.Quit();
        }
    }
}
