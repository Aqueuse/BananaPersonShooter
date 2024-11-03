using InGame.MiniGames.MonkeyMensReceptionMiniGame;
using InGame.Monkeys;
using UnityEngine;

namespace UI.InGame.VisitorReceptionMiniGameUI {
    public class UIMonkeyMensReception : MonoBehaviour {
        [SerializeField] private MonkeyMensReception monkeyMensReception;
        
        [SerializeField] private GameObject monkeyMensReceptionButton;
        [SerializeField] private Transform ScrollListContent;

        [SerializeField] private GameObject noMonkeyMensInWaitingLineGameObject;
        
        public void RefreshUIWaintingList() {
            var visitorsButtonList = ScrollListContent.GetComponentsInChildren<UIVisitorReceptionButton>();

            noMonkeyMensInWaitingLineGameObject.SetActive(monkeyMensReception.monkeyMensInWaitingLine.Count == 0);

            foreach (var uiVisitorReceptionButton in visitorsButtonList) {
                Destroy(uiVisitorReceptionButton.gameObject);
            }

            foreach (var visitorInWaitingList in monkeyMensReception.monkeyMensInWaitingLine) {
                var visitorButton = Instantiate(monkeyMensReceptionButton, ScrollListContent);
                visitorButton.GetComponent<UIVisitorReceptionButton>().monkeyMenBehaviour = visitorInWaitingList;
            }
        }
        
        public void AcceptVisitor(MonkeyMenBehaviour monkeyMenBehaviour) {
            monkeyMensReception.AcceptChimpmenInStation(monkeyMenBehaviour);
            RefreshUIWaintingList();
        }

        public void RefuseVisitor(MonkeyMenBehaviour monkeyMenBehaviour) {
            monkeyMensReception.RemoveVisitor(monkeyMenBehaviour);
            RefreshUIWaintingList();
        }
    }
}
