using UnityEngine;

namespace Interactions.InteractionsActions {
    public class BananaGunInteraction : MonoBehaviour {
        public static void Activate(GameObject _interactedObject) {
            _interactedObject.SetActive(false);
            
            ObjectsReference.Instance.tutorial.FinishTutorial();
        }
    }
}
