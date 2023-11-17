using UnityEngine;

namespace Game {
    public class ManageAccess : MonoBehaviour {
        [SerializeField] private GameObject interaction;
        [SerializeField] private GameObject accessDenied;

        private void Start() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) AuthorizeUsage();
            else {
                ForbidUsage();
            }
        }

        public void ForbidUsage() {
            interaction.SetActive(false);
            accessDenied.SetActive(true);
        }

        public void AuthorizeUsage() {
            interaction.SetActive(true);
            accessDenied.SetActive(false);
        }
    }
}
