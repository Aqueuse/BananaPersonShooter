using System.Collections.Generic;
using InGame.Monkeys.Visichimps;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.MiniGames.VisitorReceptionMiniGame {
    public class VisitorReception : MonoSingleton<VisitorReception> {
        public LinkedList<GameObject> visitorsInWaitingLine = new();
        
        [SerializeField] private Transform waitingListStart;
        
        public void AddVisitorInWaitingLine() {
            var newVisitorPosition = waitingListStart.position;
            newVisitorPosition.z += 1f * visitorsInWaitingLine.Count;
            
            var newVisitor = Instantiate(
                original: ObjectsReference.Instance.meshReferenceScriptableObject.visitorsPrefab[0],
                position: newVisitorPosition,
                rotation:Quaternion.identity);
            
            // crée une miniature qui pourra être affiché dans la liste UI
                // TP snapshotCam to visitor transform position
                // recul cam from 1 en Z
                // move up cam from YFacePosition in visiorType dictionnary
                
            visitorsInWaitingLine.AddLast(newVisitor);
            
            ObjectsReference.Instance.uiVisitorReception.RefreshUIWaintingList();

            ShrinkVisitorWaitingLine();
        }

        public void RemoveVisitor(GameObject visitorToRemove) {
            visitorsInWaitingLine.Remove(visitorToRemove);
            
            Destroy(visitorToRemove);

            ShrinkVisitorWaitingLine();
        }

        public void AcceptVisitorInStation(GameObject visitorToAccept) {
            visitorsInWaitingLine.Remove(visitorToAccept);

            visitorToAccept.GetComponent<NavMeshAgent>().Warp(ObjectsReference.Instance.chimpManager.GetRandomSasSpawnPoint().position);
            
            visitorToAccept.GetComponent<VisitorBehaviour>().enabled = true;
            visitorToAccept.GetComponent<VisitorBehaviour>().StartVisiting();
            
            ShrinkVisitorWaitingLine();
        }

        private void ShrinkVisitorWaitingLine() {
            for (int i = 0; i < visitorsInWaitingLine.Count; i++) {
                var newVisitorPosition = waitingListStart.position;
                newVisitorPosition.z += 1f * i;
            }
        }
    }
}
