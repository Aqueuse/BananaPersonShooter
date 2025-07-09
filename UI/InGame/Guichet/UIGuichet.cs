using InGame.MiniGames.MonkeyMensReceptionMiniGame;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace UI.InGame.Guichet {
    public class UIGuichet : MonoBehaviour {
        [SerializeField] private MonkeyMensReception monkeyMensReception;
        
        [SerializeField] private GameObject monkeyMensReceptionButton;
        [SerializeField] private Transform ScrollListContent;

        [SerializeField] private GameObject noMonkeyMensInWaitingLineGameObject;
        
        public void RefreshUIWaintingList() {
            var visitorsButtonList = ScrollListContent.GetComponentsInChildren<UIVisitorReceptionButton>();

            noMonkeyMensInWaitingLineGameObject.SetActive(monkeyMensReception.VisitorsInWaitingLine.Count == 0);

            foreach (var uiVisitorReceptionButton in visitorsButtonList) {
                Destroy(uiVisitorReceptionButton.gameObject);
            }

            foreach (var visitorInWaitingList in monkeyMensReception.VisitorsInWaitingLine) {
                var visitorButton = Instantiate(monkeyMensReceptionButton, ScrollListContent);
                visitorButton.GetComponent<UIVisitorReceptionButton>().VisitorBehaviour = visitorInWaitingList;
            }
        }
        
        public void AcceptVisitor(VisitorBehaviour visitorBehaviour) {
            monkeyMensReception.AcceptVisitorInStation(visitorBehaviour);
            RefreshUIWaintingList();
        }

        public void RefuseVisitor(VisitorBehaviour visitorBehaviour) {
            monkeyMensReception.RemoveVisitor(visitorBehaviour);
            RefreshUIWaintingList();
        }
    }
}
