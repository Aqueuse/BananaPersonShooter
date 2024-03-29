using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class UIMessage : MonoBehaviour {
        public void SetMessage(string message) {
            GetComponentInChildren<TextMeshProUGUI>().text = message;
        }

        private void Start() {
            Invoke(nameof(DestroyMe), 5);
        }


        private void DestroyMe() {
            Destroy(transform.gameObject);
        }
    }
}
