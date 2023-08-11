using UnityEngine;

namespace Items.ItemsActions {
    public class BananaGunItemAction : MonoBehaviour {
        public static void Activate(GameObject _interactedObject) {
            _interactedObject.SetActive(false);
            
            ObjectsReference.Instance.tutorial.FinishTutorial();
        }
    }
}
