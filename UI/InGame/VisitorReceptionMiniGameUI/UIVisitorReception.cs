using InGame.MiniGames.TouristsReceptionMiniGame;
using UnityEngine;

namespace UI.InGame.VisitorReceptionMiniGameUI {
    public class UIVisitorReception : MonoBehaviour {
        [SerializeField] private GameObject visitorReceptionButton;
        [SerializeField] private Transform ScrollListContent;

        [SerializeField] private GameObject noVisitorInWaitingLineGameObject;
        
        public void RefreshUIWaintingList() {
            var visitorsButtonList = ScrollListContent.GetComponentsInChildren<UIVisitorReceptionButton>();

            noVisitorInWaitingLineGameObject.SetActive(TouristsReception.Instance.touristsInWaitingLine.Count == 0);
            
            foreach (var uiVisitorReceptionButton in visitorsButtonList) {
                Destroy(uiVisitorReceptionButton.gameObject);
            }

            foreach (var visitorInWaitingList in TouristsReception.Instance.touristsInWaitingLine) {
                var visitorButton = Instantiate(visitorReceptionButton, ScrollListContent);
                visitorButton.GetComponent<UIVisitorReceptionButton>().visitorGameObject = visitorInWaitingList;
            }
        }
        
        public void AcceptVisitor(GameObject visitorGameObject) {
            TouristsReception.Instance.AcceptTouristInStation(visitorGameObject);
            RefreshUIWaintingList();
        }

        public void RefuseVisitor(GameObject visitorGameObject) {
            TouristsReception.Instance.RemoveVisitor(visitorGameObject);
            RefreshUIWaintingList();
        }
    }
}
