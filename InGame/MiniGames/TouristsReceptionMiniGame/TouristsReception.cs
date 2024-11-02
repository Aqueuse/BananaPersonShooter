using System.Collections.Generic;
using InGame.Monkeys.Chimptouristes;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.MiniGames.TouristsReceptionMiniGame {
    public class TouristsReception : MonoSingleton<TouristsReception> {
        public LinkedList<GameObject> touristsInWaitingLine = new();
        
        [SerializeField] private Transform waitingListStart;
        
        public void AddTouristInWaitingLine() {
            var newTouristPosition = waitingListStart.position;
            newTouristPosition.z += 1f * touristsInWaitingLine.Count;
            
            var newVisitor = Instantiate(
                original: ObjectsReference.Instance.meshReferenceScriptableObject.chimpmensPrefab[0],
                position: newTouristPosition,
                rotation:Quaternion.identity);
            
            // TODO : crée une miniature qui pourra être affiché dans la liste UI
                // TP snapshotCam to visitor transform position
                // recul cam from 1 en Z
                // move up cam from YFacePosition in touristType dictionnary
                
            touristsInWaitingLine.AddLast(newVisitor);
            
            ObjectsReference.Instance.uiTouristReception.RefreshUIWaintingList();

            ShrinkTouristsWaitingLine();
        }

        public void RemoveVisitor(GameObject touristToRemove) {
            touristsInWaitingLine.Remove(touristToRemove);
            
            Destroy(touristToRemove);

            ShrinkTouristsWaitingLine();
        }

        public void AcceptTouristInStation(GameObject touristToAccept) {
            touristsInWaitingLine.Remove(touristToAccept);

            touristToAccept.GetComponent<NavMeshAgent>().Warp(ObjectsReference.Instance.chimpManager.GetRandomSasSpawnPoint().position);
            
            touristToAccept.GetComponent<TouristBehaviour>().enabled = true;
            touristToAccept.GetComponent<TouristBehaviour>().StartVisiting();
            
            ShrinkTouristsWaitingLine();
        }

        private void ShrinkTouristsWaitingLine() {
            for (int i = 0; i < touristsInWaitingLine.Count; i++) {
                var newTouristPosition = waitingListStart.position;
                newTouristPosition.z += 1f * i;
            }
        }
    }
}
