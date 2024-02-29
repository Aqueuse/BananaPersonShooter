using InGame.MiniGames.VisitorReceptionMiniGame;
using UnityEngine;

namespace UI.InGame.VisitorReceptionMiniGameUI {
    public class UIVisitorReception : MonoBehaviour {
        [SerializeField] private GameObject visitorReceptionButton;
        [SerializeField] private Transform ScrollListContent;

        [SerializeField] private GameObject noVisitorInWaitingLineGameObject;
        
        public void RefreshUIWaintingList() {
            var visitorsButtonList = ScrollListContent.GetComponentsInChildren<UIVisitorReceptionButton>();

            noVisitorInWaitingLineGameObject.SetActive(VisitorReception.Instance.visitorsInWaitingLine.Count == 0);
            
            foreach (var uiVisitorReceptionButton in visitorsButtonList) {
                Destroy(uiVisitorReceptionButton.gameObject);
            }

            foreach (var visitorInWaitingList in VisitorReception.Instance.visitorsInWaitingLine) {
                var visitorButton = Instantiate(visitorReceptionButton, ScrollListContent);
                visitorButton.GetComponent<UIVisitorReceptionButton>().visitorGameObject = visitorInWaitingList;
            }
        }
        
        public void AcceptVisitor(GameObject visitorGameObject) {
            VisitorReception.Instance.AcceptVisitorInStation(visitorGameObject);
            RefreshUIWaintingList();
        }

        public void RefuseVisitor(GameObject visitorGameObject) {
            VisitorReception.Instance.RemoveVisitor(visitorGameObject);
            RefreshUIWaintingList();
        }
    }
}
