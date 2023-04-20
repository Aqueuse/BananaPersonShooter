using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIBananapedia : MonoBehaviour {
        public void SelectFirstBananapediaEntry() {
            GetComponentsInChildren<UIBananapediaEntry>()[0].GetComponent<Button>().onClick.Invoke();
        }
    }
}
