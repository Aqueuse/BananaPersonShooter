using UnityEngine;

namespace Game {
    public class ManageAccess : MonoBehaviour {
        [SerializeField] private GameObject interaction;
        [SerializeField] private GameObject accessDenied;
        [SerializeField] private CanvasGroup authorizedPanelCanvasGroup;

        private void Start() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) AuthorizeUsage();
            else {
                ForbidUsage();
            }
        }

        public void ForbidUsage() {
            authorizedPanelCanvasGroup.alpha = 0.3f;
            interaction.SetActive(false);
            accessDenied.SetActive(true);
        }

        public void AuthorizeUsage() {
            authorizedPanelCanvasGroup.alpha = 1;
            interaction.SetActive(true);
            accessDenied.SetActive(false);
        }
    }
}
