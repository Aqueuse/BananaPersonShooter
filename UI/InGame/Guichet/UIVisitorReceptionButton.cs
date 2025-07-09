using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace UI.InGame.Guichet {
    public class UIVisitorReceptionButton : MonoBehaviour {
        // TODO : Feed with visitor Data
        [HideInInspector] public VisitorBehaviour VisitorBehaviour;
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
        
        public void RefuseMonkeyMen() {
            ObjectsReference.Instance.uiTouristReception.RefuseVisitor(VisitorBehaviour);
        }

        public void AcceptMonkeyMen() {
            ObjectsReference.Instance.uiTouristReception.AcceptVisitor(VisitorBehaviour);
        }

        public void TryToConvertMonkeyMen() {
            // try to convince spotted pirate to become chimployee or cultivator if not enough work
            
            // will fail if visitor was in fact tourist
        }
    }
}
