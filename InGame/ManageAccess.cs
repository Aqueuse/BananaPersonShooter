using UnityEngine;

namespace InGame {
    public class ManageAccess : MonoBehaviour {
        [SerializeField] private GameObject playInteraction;
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
            playInteraction.SetActive(false);
            accessDenied.SetActive(true);
        }

        public void AuthorizeUsage() {
            authorizedPanelCanvasGroup.alpha = 1;
            playInteraction.SetActive(true);
            accessDenied.SetActive(false);
        }
    }
}
