using System;
using System.Collections.Generic;
using Game.Monkeys;
using Game.Monkeys.Visichimps;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.VisitorReceptionMiniGame {
    public class VisitorReception : MonoSingleton<VisitorReception> {
        public LinkedList<GameObject> visitorsInWaitingLine = new();
        
        [SerializeField] private Transform waitingListStart;
        
        private int visitorsTypeQuantity;
    
        private void Start() {
            visitorsTypeQuantity = Enum.GetNames(typeof(VisitorType)).Length;
        }

        public void AddVisitorInWaitingLine(VisitorType visitorType) {
            var newVisitorPosition = waitingListStart.position;
            newVisitorPosition.z += 1f * visitorsInWaitingLine.Count;
            
            // decide witch type of visitor to spawn
            var visitorPrefabIndex = Random.Range(0, visitorsTypeQuantity);
            //var visitorType = (VisitorType)visitorPrefabIndex;
//            var visitorType = VisitorType.VISITOR_ADULT_MEDIUM;

            var newVisitor = Instantiate(
                original: ObjectsReference.Instance.meshReferenceScriptableObject.visitorPrefabByPrefabIndex[visitorType],
                position: newVisitorPosition,
                rotation:Quaternion.identity);

            var newVisitorBehaviour = newVisitor.GetComponent<VisitorBehaviour>();

            newVisitorBehaviour.visitorGuid = Guid.NewGuid().ToString();
            newVisitorBehaviour.visitorType = visitorType;
            
            newVisitorBehaviour.textureRotation = 0;
            newVisitorBehaviour.prefabIndex = visitorPrefabIndex;
            newVisitorBehaviour.visitorName = "Roger";
            newVisitorBehaviour.visitorDescription = "un visiteur, venu d'ailleurs !";
            
            newVisitorBehaviour.visitorNeeds[NeedType.FUN] = 1;
            newVisitorBehaviour.visitorNeeds[NeedType.REST] = 1;
            newVisitorBehaviour.visitorNeeds[NeedType.HUNGER] = 1;
            newVisitorBehaviour.visitorNeeds[NeedType.SOUVENIR] = 1;
            newVisitorBehaviour.visitorNeeds[NeedType.KNOWLEDGE] = 1;

            ChimpManager.Instance.SetNextVisitorDestination(newVisitorBehaviour);

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

            visitorToAccept.GetComponent<NavMeshAgent>().Warp(ChimpManager.Instance.GetRandomSasSpawnPoint().position);
//            visitorToAccept.transform.position = ;
            
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
