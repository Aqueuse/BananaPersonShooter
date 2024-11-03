using System.Collections.Generic;
using InGame.Monkeys;
using InGame.Monkeys.Chimpirates;
using InGame.Monkeys.Chimptouristes;
using UnityEngine;

namespace InGame.MiniGames.MonkeyMensReceptionMiniGame {
    public class MonkeyMensReception : MonoSingleton<MonkeyMensReception> {
        public LinkedList<MonkeyMenBehaviour> monkeyMensInWaitingLine = new();
        
        [SerializeField] private Transform waitingListStart;
        
        public void AddMonkeyMensInWaitingLine(MonkeyMenBehaviour monkeyMenBehaviour) {
            var newChimpmenPosition = waitingListStart.position;
            newChimpmenPosition.z += 1f * monkeyMensInWaitingLine.Count;
            
            monkeyMensInWaitingLine.AddLast(monkeyMenBehaviour);
            
            ObjectsReference.Instance.uiTouristReception.RefreshUIWaintingList();

            ShrinkMonkeyMensWaitingLine();
        }

        public void RemoveVisitor(MonkeyMenBehaviour monkeyMenToRemove) {
            monkeyMensInWaitingLine.Remove(monkeyMenToRemove);
            
            Destroy(monkeyMenToRemove);

            ShrinkMonkeyMensWaitingLine();
        }

        public void AcceptChimpmenInStation(MonkeyMenBehaviour monkeyMenToAccept) {
            monkeyMensInWaitingLine.Remove(monkeyMenToAccept);

            if (monkeyMenToAccept.monkeyMenData.characterType == CharacterType.TOURIST) {
                monkeyMenToAccept.GetComponent<TouristBehaviour>().StartVisiting();
            }

            if (monkeyMenToAccept.monkeyMenData.characterType == CharacterType.PIRATE) {
                monkeyMenToAccept.GetComponent<PirateBehaviour>().StartPiracy();
            }

            ShrinkMonkeyMensWaitingLine();
        }

        private void ShrinkMonkeyMensWaitingLine() {
            for (var i = 0; i < monkeyMensInWaitingLine.Count; i++) {
                var newMonkeyMenPosition = waitingListStart.position;
                newMonkeyMenPosition.z += 1f * i;
            }
        }
    }
}
