using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using InGame.Items.ItemsData;
using InGame.Items.ItemsProperties.Characters;
using InGame.MiniGames.SpaceTrafficControl.projectiles;
using InGame.Monkeys.Chimpvisitors;
using InGame.Monkeys.Merchimps;
using InGame.SpaceTrafficControl;
using Save.Helpers;
using Save.Templates;
using Tags;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class SpaceshipBehaviour : MonoBehaviour {
        [SerializeField] private Transform _spaceshipTransform;
        public SpaceshipData spaceshipData;
        
        public Transform visitorsSpawnPoint;
        public Transform merchimpSpawnPoint;
        
        [HideInInspector] public SpaceshipSavedData spaceshipSavedData;

        private DOTweenPath pathBeetweenSpaceAndHangar;
        private Vector3[] assignatedPathToHangar;

        private float _step;

        private GroupBehaviour associatedGroupBehaviour;
        
        // Generate spaceship Data for new spaceship
        public void Init(Vector3 arrivalPoint, CharacterType characterType, SpaceshipType spaceshipType) {
            spaceshipData.spaceshipGuid = Guid.NewGuid().ToString();
            spaceshipData.spaceshipName = ObjectsReference.Instance.spaceTrafficControlManager.GetUniqueSpaceshipName();
            spaceshipData.arrivalPosition = arrivalPoint;
            spaceshipData.spaceshipType = spaceshipType;
            spaceshipData.characterType = characterType;
            spaceshipData.spaceshipUIcolor = SpaceTrafficControlManager.GetRandomColor();
        }
        
        private void Update() {
            if (spaceshipData.travelState == TravelState.GO_TO_PATH) {
                _step = SpaceshipData._propulsionSpeed * 4 * Time.deltaTime;

                _spaceshipTransform.position = Vector3.MoveTowards(_spaceshipTransform.position, assignatedPathToHangar[0], _step);
                _spaceshipTransform.LookAt(assignatedPathToHangar[0], Vector3.up);

                if (Vector3.Distance(transform.position, assignatedPathToHangar[0]) < 10) {
                    spaceshipData.travelState = TravelState.TRAVEL_ON_PATH;
                    MoveToElevator();
                }
            }

            if (spaceshipData.travelState == TravelState.FREE_FLIGHT | spaceshipData.travelState == TravelState.LEAVES_THE_REGION) {
                _step = SpaceshipData._propulsionSpeed * Time.deltaTime;
            
                _spaceshipTransform.position = Vector3.MoveTowards(_spaceshipTransform.position, spaceshipData.arrivalPosition, _step);
            
                _spaceshipTransform.LookAt(spaceshipData.arrivalPosition, Vector3.up);
                
                if (Vector3.Distance(transform.position, spaceshipData.arrivalPosition) < 10) {
                    LeaveRegion();
                }
            }
        }
        
        private void SpawnVisitors(GroupScriptableObject groupScriptableObject, string associatedSpaceshipGuid) {
            if (groupScriptableObject.members.Count == 1 && groupScriptableObject.members[0].characterType == CharacterType.MERCHIMP) {
                var merchimp = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.merchimpPrefab,
                    merchimpSpawnPoint.position,
                    merchimpSpawnPoint.rotation,
                    null
                ).GetComponent<MerchimpBehaviour>();
            
                merchimp.Init(
                    groupScriptableObject.members[0],
                    this
                );
            }

            else {
                var group = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.groupPrefab,
                    visitorsSpawnPoint.position,
                    Quaternion.identity,
                    null
                ).GetComponent<GroupBehaviour>();
                
                group.SpawnMembers(groupScriptableObject.members, associatedSpaceshipGuid);

                associatedGroupBehaviour = group;
                group.spaceshipVisitorsSpawnPoint = visitorsSpawnPoint.position;
            }

        }

        private void RespawnVisitors(List<MonkeyMenSavedData> groupMembers) {
            if (groupMembers.Count == 1 && groupMembers[0].characterType == CharacterType.MERCHIMP) {
                var merchimp = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.merchimpPrefab,
                    merchimpSpawnPoint.position,
                    merchimpSpawnPoint.rotation,
                    null
                ).GetComponent<MerchimpBehaviour>();
            
                merchimp.LoadSavedData(groupMembers[0]);
            }

            else {
                var group = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.groupPrefab,
                    ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position,
                    Quaternion.identity,
                    null
                ).GetComponent<GroupBehaviour>();
                
                foreach (var member in groupMembers) {
                    var visitorInstance = Instantiate(
                        original: ObjectsReference.Instance.meshReferenceScriptableObject.monkeyMenPrefabs[member.prefabIndex],
                        position: JsonHelper.FromStringToVector3(member.position),
                        rotation: JsonHelper.FromStringToQuaternion(member.rotation),
                        parent: ObjectsReference.Instance.gameSave.savablesItemsContainer
                    );

                    visitorInstance.GetComponent<VisitorBehaviour>().LoadSavedData(member);
                }

                associatedGroupBehaviour = group;
                group.spaceshipVisitorsSpawnPoint = visitorsSpawnPoint.position;
            }
        }
        
        public void OpenCommunications(CharacterType characterType) {
            var systemRandom = new System.Random();

            spaceshipData.communicationMessagePrefabIndex = 
                systemRandom.Next(ObjectsReference.Instance.uiCommunicationPanel
                    .spaceshipMessagesByCharacterType[characterType].Count);

            ObjectsReference.Instance.uiCommunicationPanel.AddNewCommunication(this);
        }

        public void WaitInStation() {
            spaceshipData.travelState = TravelState.WAIT_IN_STATION;

            ConvertToSolidSpaceship();
            
            foreach (var spaceshipDebrisBehaviour in GetComponentsInChildren<SpaceshipDebrisBehaviour>()) {
                spaceshipDebrisBehaviour.enabled = false;
            }

            SpawnVisitors(
                spaceshipData.characterType == CharacterType.MERCHIMP
                    ? ObjectsReference.Instance.meshReferenceScriptableObject.GetNextMerchimpGroup()
                    : ObjectsReference.Instance.meshReferenceScriptableObject.GetNextVisitorGroup(),
                spaceshipData.spaceshipGuid
            );
        }
        
        public void StopWaiting() {
            ObjectsReference.Instance.spaceTrafficControlManager.FreeHangar(spaceshipData.assignatedHangar);
            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
            
            spaceshipData.travelState = TravelState.TRAVEL_BACK_ON_ELEVATOR;
            TravelBackOnElevator();
        }
        
        public void GoToPath(int hangarNumber) {
            spaceshipData.assignatedHangar = hangarNumber;
            assignatedPathToHangar = ObjectsReference.Instance.spaceTrafficControlManager.pathsToHangars[spaceshipData.assignatedHangar];

            spaceshipData.travelState = TravelState.GO_TO_PATH;
        }
        
        private void MoveToElevator() {
            spaceshipData.travelState = TravelState.TRAVEL_ON_PATH;
            
            transform.DOPath(
                ObjectsReference.Instance.spaceTrafficControlManager.pathsToHangars[spaceshipData.assignatedHangar],
                20,
                PathType.CatmullRom).SetLookAt(SpaceshipData.lookAtRotation).SetEase(Ease.Linear).OnComplete(MoveOnElevatorToHangar);
        }

        private void MoveOnElevatorToHangar() {
            ConvertToSolidSpaceship();

            transform.DOPath(
                ObjectsReference.Instance.spaceTrafficControlManager.elevatorsPaths[spaceshipData.assignatedHangar],
                20,
                PathType.CatmullRom).SetLookAt(
                ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.SPACESHIP_HANGARS],
                stableZRotation:true).OnComplete(WaitInStation);
        }

        private void TravelBackOnElevator() {
            var reversePathFromHangarToElevator = ObjectsReference.Instance.spaceTrafficControlManager.elevatorsPaths[spaceshipData.assignatedHangar].Reverse().ToArray(); 

            transform.DOPath(reversePathFromHangarToElevator,
                20,
                PathType.CatmullRom).OnComplete(TravelBackToSpace);
        }

        private void TravelBackToSpace() {
            ConvertToExplodableSpaceship();
            
            spaceshipData.travelState = TravelState.LEAVES_THE_REGION;
            spaceshipData.arrivalPosition.y = -10f;
        }

        private void LeaveRegion() {
            ObjectsReference.Instance.uiCommunicationPanel.CloseCommunications(this);
            ObjectsReference.Instance.spaceTrafficControlManager.FreeHangar(spaceshipData.assignatedHangar);
            
            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Remove(spaceshipData.spaceshipGuid);

            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationButton();
            ObjectsReference.Instance.spaceshipsSpawner.RemoveGuestInCampaignCreator();
            Destroy(gameObject);
        }

        private void ConvertToSolidSpaceship() {
            GetComponent<CapsuleCollider>().enabled = false;
            
            foreach (var spaceshipDebrisBehaviour in GetComponentsInChildren<SpaceshipDebrisBehaviour>()) {
                spaceshipDebrisBehaviour.enabled = false;
                spaceshipDebrisBehaviour.GetComponent<MeshCollider>().isTrigger = false;
            }
        }

        private void ConvertToExplodableSpaceship() {
            GetComponent<CapsuleCollider>().enabled = true;
            
            foreach (var spaceshipDebrisBehaviour in GetComponentsInChildren<SpaceshipDebrisBehaviour>()) {
                spaceshipDebrisBehaviour.enabled = true;
                spaceshipDebrisBehaviour.GetComponent<MeshCollider>().isTrigger = true;
            }
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.LASER)) return;
            
            GetComponent<CapsuleCollider>().enabled = false;

            foreach (var debrisBehaviour in gameObject.GetComponentsInChildren<SpaceshipDebrisBehaviour>()) {
                debrisBehaviour.enabled = true;
                debrisBehaviour.Init(other.GetComponent<Laser>());
            }
            
            ObjectsReference.Instance.spaceshipsSpawner.RemoveGuestInCampaignCreator();
            ObjectsReference.Instance.uiCommunicationPanel.CloseCommunications(this);
        }
        
        public void LoadSavedData(SpaceshipSavedData spaceshipSavedData) {
            spaceshipData.spaceshipGuid = spaceshipSavedData.spaceshipGuid;
            spaceshipData.spaceshipName = spaceshipSavedData.spaceshipName;
            spaceshipData.arrivalPosition = JsonHelper.FromStringToVector3(spaceshipSavedData.arrivalPoint);
            spaceshipData.characterType = spaceshipSavedData.characterType;
            spaceshipData.spaceshipType = spaceshipSavedData.spaceshipType;
            spaceshipData.communicationMessagePrefabIndex = spaceshipSavedData.communicationMessagePrefabIndex;
            spaceshipData.spaceshipUIcolor = JsonHelper.FromStringToColor(spaceshipSavedData.uiColor);
            spaceshipData.travelState = spaceshipSavedData.travelState;
            spaceshipData.assignatedHangar = spaceshipSavedData.hangarNumber;
            
            spaceshipData.groupTravelState = spaceshipSavedData.groupTravelState;
            spaceshipData.guichetsMapsToVisit = spaceshipSavedData.guichetsMapsToVisit;
            spaceshipData.mapPointInterests = spaceshipSavedData.mapPointInterests;

            this.spaceshipSavedData = spaceshipSavedData;

            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Add(
                spaceshipData.spaceshipGuid, 
                this);

            if (spaceshipData.travelState == TravelState.WAIT_IN_STATION) {
                ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(spaceshipData.assignatedHangar);

                RespawnVisitors(spaceshipSavedData.MonkeyMenSavedDatas);
            }

            if (spaceshipData.travelState == TravelState.GO_TO_PATH) {
                GoToPath(spaceshipData.assignatedHangar);
            }
        
            if (spaceshipData.travelState == TravelState.TRAVEL_ON_PATH) {
                MoveToElevator();
            }
        
            if (spaceshipData.travelState == TravelState.TRAVEL_ON_ELEVATOR) {
                MoveOnElevatorToHangar();
            }
        
            if (spaceshipData.travelState == TravelState.TRAVEL_BACK_ON_ELEVATOR) {
                spaceshipData.travelState = TravelState.LEAVES_THE_REGION;
                transform.position = new Vector3(transform.position.x, -10f, transform.position.z);
                spaceshipData.arrivalPosition.y = -10f;
            }

            if (spaceshipData.travelState == TravelState.LEAVES_THE_REGION) {
                spaceshipData.arrivalPosition.y = -10f;
            }

            if (spaceshipData.travelState == TravelState.FREE_FLIGHT | spaceshipData.travelState == TravelState.LEAVES_THE_REGION) {
                ObjectsReference.Instance.uiCommunicationPanel.AddNewCommunication(this);
            }
        }

        public void GenerateSaveData() {
            if (string.IsNullOrEmpty(spaceshipData.spaceshipGuid)) {
                spaceshipData.spaceshipGuid = Guid.NewGuid().ToString();
            }

            spaceshipSavedData = new SpaceshipSavedData {
                spaceshipName = spaceshipData.spaceshipName,
                spaceshipGuid = spaceshipData.spaceshipGuid,
                communicationMessagePrefabIndex = spaceshipData.communicationMessagePrefabIndex,
                uiColor = JsonHelper.FromColorToString(spaceshipData.spaceshipUIcolor),
                characterType = spaceshipData.characterType,
                spaceshipType = spaceshipData.spaceshipType,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = spaceshipData.travelState,
                arrivalPoint = JsonHelper.FromVector3ToString(spaceshipData.arrivalPosition),
                hangarNumber = spaceshipData.assignatedHangar,
            };

            if (associatedGroupBehaviour) {
                spaceshipSavedData.guichetsMapsToVisit = associatedGroupBehaviour.mapsGuichetToVisit.ToArray();
                spaceshipSavedData.groupTravelState = associatedGroupBehaviour.groupTravelState;
                spaceshipSavedData.mapPointInterests = associatedGroupBehaviour.mapPointsOfInterests.ToArray();

                List<MonkeyMenSavedData> monkeyMenSavedDatas = new List<MonkeyMenSavedData>();
                
                foreach (var visitorBehaviour in associatedGroupBehaviour.members) {
                    monkeyMenSavedDatas.Add(visitorBehaviour.GenerateSavedData());
                }

                spaceshipSavedData.MonkeyMenSavedDatas = monkeyMenSavedDatas;
            }
        }
    }
}
