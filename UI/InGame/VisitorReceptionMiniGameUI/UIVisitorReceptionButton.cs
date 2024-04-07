using UnityEngine;

namespace UI.InGame.VisitorReceptionMiniGameUI {
    public class UIVisitorReceptionButton : MonoBehaviour {
        // TODO : Feed with visitor Data
        [HideInInspector] public GameObject visitorGameObject;
        [SerializeField] private CanvasGroup optionsCanvasGroup;

        public void ShowOptions() {
            optionsCanvasGroup.alpha = 1;
            optionsCanvasGroup.interactable = true;
            optionsCanvasGroup.blocksRaycasts = true;
        }

        public void HideOptions() {
            optionsCanvasGroup.alpha = 0;
            optionsCanvasGroup.interactable = false;
            optionsCanvasGroup.blocksRaycasts = false;
        }
        
        public void RefuseVisitor() {
            ObjectsReference.Instance.uiTouristReception.RefuseVisitor(visitorGameObject);
        }

        public void AcceptVisitor() {
            ObjectsReference.Instance.uiTouristReception.AcceptVisitor(visitorGameObject);
        }
    }
}
