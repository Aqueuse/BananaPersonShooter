using System.Collections.Generic;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace InGame.MiniGames.MonkeyMensReceptionMiniGame {
    public class MonkeyMensReception : MonoSingleton<MonkeyMensReception> {
        public LinkedList<VisitorBehaviour> VisitorsInWaitingLine = new();
        
        [SerializeField] private Transform waitingListStart;
        
        public void AddMonkeyMensInWaitingLine(VisitorBehaviour monkeyMenBehaviour) {
            var newChimpmenPosition = waitingListStart.position;
            newChimpmenPosition.z += 1f * VisitorsInWaitingLine.Count;
            
            VisitorsInWaitingLine.AddLast(monkeyMenBehaviour);
            
            ObjectsReference.Instance.uiTouristReception.RefreshUIWaintingList();

            ShrinkMonkeyMensWaitingLine();
        }

        public void RemoveVisitor(VisitorBehaviour monkeyMenToRemove) {
            VisitorsInWaitingLine.Remove(monkeyMenToRemove);
            
            Destroy(monkeyMenToRemove);

            ShrinkMonkeyMensWaitingLine();
        }

        public void AcceptVisitorInStation(VisitorBehaviour visitorToAccept) {
            VisitorsInWaitingLine.Remove(visitorToAccept);
            
            // TODO : merge pirateBehaviour with visitorBehaviour : pirates have only a need PILLAGE (they just want
            // to break buildables, no other need 
            
            ShrinkMonkeyMensWaitingLine();
        }

        private void ShrinkMonkeyMensWaitingLine() {
            for (var i = 0; i < VisitorsInWaitingLine.Count; i++) {
                var newMonkeyMenPosition = waitingListStart.position;
                newMonkeyMenPosition.z += 1f * i;
            }
        }
    }
}
