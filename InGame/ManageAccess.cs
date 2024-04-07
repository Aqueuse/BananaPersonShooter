using UnityEngine;

namespace InGame {
    public class ManageAccess : MonoBehaviour {
        [SerializeField] private GameObject playInteraction;
        [SerializeField] private GameObject accessManagedVisual;

        private void Start() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) AuthorizeUsage();
            else {
                ForbidUsage();
            }
        }

        public void ForbidUsage() {
            playInteraction.SetActive(false);
            if (accessManagedVisual != null)
                accessManagedVisual.SetActive(true);
        }

        public void AuthorizeUsage() {
            playInteraction.SetActive(true);
            if (accessManagedVisual != null)
                accessManagedVisual.SetActive(false);
        }
    }
}
