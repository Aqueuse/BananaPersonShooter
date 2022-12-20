using UnityEngine.UI;

namespace UI {
    public class UIBananapedia : MonoSingleton<UIBananapedia> {
        public void SelectFirstBananapediaEntry() {
            GetComponentsInChildren<UIBananapediaEntry>()[0].GetComponent<Button>().onClick.Invoke();
        }
    }
}
