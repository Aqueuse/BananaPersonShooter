using System.Collections.Generic;
using DG.Tweening;
using InGame.Items.ItemsProperties.Characters;
using Save.Helpers;
using Save.Templates;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Monkeys.Chimpvisitors {
    public class GroupBehaviour : MonoBehaviour {
        private GroupScriptableObject _groupScriptableObject;
        
        public List<VisitorBehaviour> members = new ();
        public Vector3 groupNextDestination;
        
        [SerializeField] private DOTweenPath doTweenPath;
        private int dotweenPathIndex;
        private int currentSegmentIndex;
        
        public void SpawnMembers(GroupScriptableObject groupScriptableObject) {
            _groupScriptableObject = groupScriptableObject;

            dotweenPathIndex = Random.Range(0, ObjectsReference.Instance.gameManager.mapsDotweenPaths.Count);
            doTweenPath = ObjectsReference.Instance.gameManager.mapsDotweenPaths[dotweenPathIndex];
            
            foreach (var member in groupScriptableObject.members) {
                if (NavMesh.SamplePosition(doTweenPath.wps[0], out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) {
                
                    var visitor = Instantiate(ObjectsReference.Instance.meshReferenceScriptableObject.monkeyMenPrefabs[member.prefabIndex], hit.position, Quaternion.identity).GetComponent<VisitorBehaviour>();
                    visitor.Init(this, member);
                    members.Add(visitor);
                }
                else {
                    Debug.LogWarning($"Impossible de placer un membre du groupe {groupScriptableObject.name} sur le NavMesh !");
                }
            }
            
            transform.position = doTweenPath.wps[0];
            InvokeRepeating(nameof(CheckGroupArrival), 0.1f, 0.2f);
        }

        private void RespawnMembers(GroupSavedData groupSavedData) {
            dotweenPathIndex = groupSavedData.dotweenPathIndex;
            currentSegmentIndex = groupSavedData.currentSegmentIndex;
            
            foreach (var visitorSavedData in groupSavedData.visitorsSavedDatas) {
                var visitorInstance = Instantiate(
                    original: ObjectsReference.Instance.meshReferenceScriptableObject.monkeyMenPrefabs[visitorSavedData.prefabIndex],
                    position: JsonHelper.FromStringToVector3(visitorSavedData.position),
                    rotation: JsonHelper.FromStringToQuaternion(visitorSavedData.rotation),
                    parent: ObjectsReference.Instance.gameSave.savablesItemsContainer
                );

                visitorInstance.GetComponent<VisitorBehaviour>().LoadSavedData(visitorSavedData);
            }
        }
        
        private Vector3 ThrowExplorationPalet() {
            currentSegmentIndex++;

            // the loop is completed / back to the gate
            if (currentSegmentIndex >= doTweenPath.wps.Count) {
                CancelInvoke(nameof(CheckGroupArrival));
                InvokeRepeating(nameof(checkArrivalToGate), 0.1f, 0.2f);
                
                return doTweenPath.wps[0];
            }
            
            Vector3 start = doTweenPath.wps[currentSegmentIndex-1];
            Vector3 end = doTweenPath.wps[currentSegmentIndex];
            
            // Interpolation aléatoire sur le segment
            return Vector3.Lerp(start, end, Random.Range(0.01f, 0.99f));
        }
        
        public void CheckGroupArrival() {
            foreach (var memberBehaviour in members) {
                if (!memberBehaviour.HasArrivedToDestination())
                    return;
            }
            
            groupNextDestination = ThrowExplorationPalet();
            transform.position = groupNextDestination;
            
            foreach (var memberBehaviour in members) {
                memberBehaviour.SetDestination(groupNextDestination);
            }
        }

        private void checkArrivalToGate() {
            // we miss someone, probably gone feeding a need, will wait
            if (members.Count != _groupScriptableObject.members.Count) return;
            
            foreach (var memberBehaviour in members) {
                if (!memberBehaviour.CheckIsArrivedToGate()) {
                    return;
                }
            }

            // everyone is here ? Okay, bye bye station !
            foreach (var memberBehaviour in members) {
                Destroy(memberBehaviour.gameObject);
            }
            
            CancelInvoke(nameof(checkArrivalToGate));
            Destroy(gameObject);
        }

        public GroupSavedData GenerateSaveData() {
            var groupMonkeyMenSavedData = new List<MonkeyMenSavedData>();
            
            foreach (var visitorBehaviour in members) {
                groupMonkeyMenSavedData.Add(visitorBehaviour.GenerateSavedData());
            }

            return new GroupSavedData {
                dotweenPathIndex = dotweenPathIndex,
                currentSegmentIndex = currentSegmentIndex,
                visitorsSavedDatas = groupMonkeyMenSavedData
            };
        }

        public void LoadSaveData(GroupSavedData groupSavedData) {
            RespawnMembers(groupSavedData);
            
            // refresh after eventually removing unpopulated spaceships
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationButton();
            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
        }
    }
}
