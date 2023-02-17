using UnityEngine;

namespace Input.UIActions {
    public class UISchemaSwitch : MonoBehaviour {
        public void EnableSchema() {
            gameObject.SetActive(true);
        }

        public void DisableSchema() {
            gameObject.SetActive(false);
        }
    }
}
