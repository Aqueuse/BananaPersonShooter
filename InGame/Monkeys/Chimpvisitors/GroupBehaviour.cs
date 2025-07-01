using System.Collections.Generic;
using System.Linq;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData.Characters;
using InGame.Items.ItemsProperties.Characters;
using Save.Helpers;
using Save.Templates;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Monkeys.Chimpvisitors {
    public class GroupBehaviour : MonoBehaviour {
        private GroupScriptableObject _groupScriptableObject;
        private string associatedSpaceshipGuid;
        
        public List<VisitorBehaviour> members = new ();
        public Vector3 spaceshipVisitorsSpawnPoint;
        
        public GroupTravelState groupTravelState = GroupTravelState.GO_TO_COROLLE_CENTER;
        
        public Stack<SpawnPoint> mapsGuichetToVisit = new ();
        public Stack<Vector3> mapPointsOfInterests;
        private GuichetBehaviour nextGuichet;
        
        private Vector3 corolleCenter;
        private Vector3 hangarsCenter;
        
        private void Start() {
            // search if at least a map is open
            // if only one map is open, pick it
            // if at least two maps are open, pick them randomly
            
            GoToCorolleCenter();
            
        }

        private void Update() {
            if (groupTravelState == GroupTravelState.START_VISIT) {
                if (HasGroupReachedMe()) {
                    mapsGuichetToVisit = ChooseMapsToExplore();

                    if (mapsGuichetToVisit.Count == 0) {
                        groupTravelState = GroupTravelState.GO_BACK_TO_SPACESHIP;
                        transform.position = spaceshipVisitorsSpawnPoint;
                    }
                    
                    else {
                        ShuffleMapsGuichetToVisit();
                        groupTravelState = GroupTravelState.GO_EXPLORE_MAP;
                    }
                }
            }

            if (groupTravelState == GroupTravelState.GO_TO_COROLLE_CENTER) {
                if (HasGroupReachedMe()) {
                    if (mapsGuichetToVisit.Count == 0) {
                        groupTravelState = GroupTravelState.GO_BACK_TO_SPACESHIP;
                        transform.position = spaceshipVisitorsSpawnPoint;
                    }

                    else {
                        groupTravelState = GroupTravelState.GO_EXPLORE_MAP;
                        nextGuichet = ObjectsReference.Instance.guichetsToMap[mapsGuichetToVisit.Pop()];
                        
                        foreach (var member in members) {
                            nextGuichet.visitorsToWatch.Add(member);
                        }

                        mapPointsOfInterests = nextGuichet.GiveToRandomPointsOfInterest();
                    }
                }
            }

            if (groupTravelState == GroupTravelState.GO_EXPLORE_MAP && HasGroupReachedMe()) {
                if (mapPointsOfInterests.Count == 0) {
                    groupTravelState = GroupTravelState.GO_TO_COROLLE_CENTER;
                    GoToCorolleCenter();
                }

                else {
                    transform.position = mapPointsOfInterests.Pop();
                }
            }

            if (groupTravelState == GroupTravelState.GO_BACK_TO_SPACESHIP) {
                checkArrivalToSpaceship();
            }
        }

        private Stack<SpawnPoint> ChooseMapsToExplore() {
            var guichetsToVisit = new List<SpawnPoint>();
            
            foreach (var guichetBehaviour in ObjectsReference.Instance.guichetsToMap) {
                if (guichetBehaviour.Value.isOpen) guichetsToVisit.Add(guichetBehaviour.Key);
            }
            
            return new Stack<SpawnPoint>(guichetsToVisit);
        }

        private void GoToCorolleCenter() {
            transform.position = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position;
        }
        
        private bool HasGroupReachedMe() {
            return members.All(
                memberBehaviour => !(Vector3.Distance(transform.position, memberBehaviour.transform.position) > 1f)
            );
        }

        private void checkArrivalToSpaceship() {
            // we miss someone, probably gone feeding a need, will wait
            if (members.Count != _groupScriptableObject.members.Count) return;
            
            foreach (var memberBehaviour in members) {
                if (!memberBehaviour.CheckIsArrivedToSpaceship()) {
                    return;
                }
            }

            // everyone is here ?
            foreach (var memberBehaviour in members) {
                Destroy(memberBehaviour.gameObject);
            }

            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[associatedSpaceshipGuid]
                .spaceshipData.travelState = TravelState.TRAVEL_BACK_ON_ELEVATOR;

            Destroy(gameObject); // Bye bye !
        }
        
        public void SpawnMembers(List<MonkeyMenData> members, string associatedSpaceshipGuid) {
            this.associatedSpaceshipGuid = associatedSpaceshipGuid;
            
            var spawnPoint = ObjectsReference.Instance.spaceTrafficControlManager
                .spaceshipBehavioursByGuid[associatedSpaceshipGuid].visitorsSpawnPoint.position;
            
            foreach (var member in members) {
                if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) {
                
                    var visitor = Instantiate(
                        ObjectsReference.Instance.meshReferenceScriptableObject.monkeyMenPrefabs[member.prefabIndex], 
                        hit.position,
                        Quaternion.identity).GetComponent<VisitorBehaviour>();
                    
                    visitor.Init(this, member, associatedSpaceshipGuid, hit.position);
                    this.members.Add(visitor);
                }
                else {
                    Debug.LogWarning("Impossible de placer un membre du groupe sur le NavMesh !");
                }
            }

            groupTravelState = GroupTravelState.START_VISIT;
            transform.position = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE]
                .position;
        }

        private void RespawnMembers(GroupSavedData groupSavedData) {
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
        
        private void ShuffleMapsGuichetToVisit() {
            var mapsCount = mapsGuichetToVisit.Count;

            var mapsToVisitList = mapsGuichetToVisit.ToList();
            
            while (mapsCount > 1) {
                mapsCount--;
                int randomRange = Random.Range(0, mapsCount + 1);
                (mapsToVisitList[randomRange], mapsToVisitList[mapsCount]) = (mapsToVisitList[mapsCount], mapsToVisitList[randomRange]);
            }

            mapsGuichetToVisit = new Stack<SpawnPoint>(mapsToVisitList);
        }
    }
}
