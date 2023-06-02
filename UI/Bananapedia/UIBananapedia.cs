using UnityEngine;
using UnityEngine.UI;

namespace UI.Bananapedia {
    public class UIBananapedia : MonoBehaviour {
        public void SelectFirstBananapediaEntry() {
            GetComponentsInChildren<UIBananapediaEntry>()[0].GetComponent<Button>().onClick.Invoke();
        }
    }
}
