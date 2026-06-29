using System;
using System.Collections.Generic;
using InGame.Items.ItemsData;
using InGame.Items.ItemsData.Characters;
using InGame.MiniGames.SpaceTrafficControl.projectiles;
using InGame.Monkeys.Chimpvisitors;
using InGame.SpaceTrafficControl;
using Save.Helpers;
using Save.Templates;
using Tags;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace InGame.Items.ItemsBehaviours {
    public class SpaceshipBehaviour : MonoBehaviour {
        [SerializeField] private SplineAnimate _splineAnimate;
        public SpaceshipData spaceshipData;
        
        public Transform visitorsSpawnPoint;
        public Transform merchimpSpawnPoint;

        [SerializeField] private GameObject distantDot;
        
        [HideInInspector] public SpaceshipSavedData spaceshipSavedData;
        
        private Spline currentSplineToFollow;

        private float _step;
        
        private List<MonkeyMenData> visitorsMonkeyMenDatas = new List<MonkeyMenData>();
        private List<VisitorBehaviour> visitorsBehaviours = new List<VisitorBehaviour>();

        private GameObject visitor;

        private void OnEnable() {
            _splineAnimate.Completed += ChangeTravelState;
        }
        
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
            switch (spaceshipData.travelState) {
                case TravelState.FREE_FLIGHT or TravelState.LEAVES_THE_REGION:
                    _step = SpaceshipData._propulsionSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, spaceshipData.arrivalPosition, _step);
                    transform.LookAt(spaceshipData.arrivalPosition, Vector3.up);
                    
                    if (Vector3.Distance(transform.position, spaceshipData.arrivalPosition) < 10) LeaveRegion();

                    break;
            }
        }
        
        private void ChangeTravelState() {
            switch (spaceshipData.travelState) {
                case TravelState.TRAVEL_ON_SPACE_PATH:
                    TravelOnElevatorPath();
                    spaceshipData.travelState = TravelState.TRAVEL_ON_ELEVATOR_PATH;
                    break;
                case TravelState.TRAVEL_ON_ELEVATOR_PATH:
                    WaitInStation();
                    spaceshipData.travelState = TravelState.WAIT_IN_STATION;
                    break;
                case TravelState.TRAVEL_BACK_ON_ELEVATOR_PATH:
                    TravelBackToSpace();
                    spaceshipData.travelState = TravelState.LEAVES_THE_REGION;
                    break;
            }
        }

        public void OpenCommunications(CharacterType characterType) {
            var systemRandom = new System.Random();

            spaceshipData.communicationMessagePrefabIndex = 
                systemRandom.Next(ObjectsReference.Instance.uiCommunicationPanel
                    .spaceshipMessagesByCharacterType[characterType].Count);

            ObjectsReference.Instance.uiCommunicationPanel.AddNewCommunication(this);
        }
        
        public void TravelOnSpacePath(int hangarNumber) {
            spaceshipData.travelState = TravelState.TRAVEL_ON_SPACE_PATH;
            spaceshipData.assignatedHangar = hangarNumber;

            // move space_path spline first knot position to the spaceship to get a fuild travel
            var BezierKnot = new BezierKnot { Position = transform.position };
            ObjectsReference.Instance.spaceTrafficControlManager.spacePaths[spaceshipData.assignatedHangar].Spline[0] = BezierKnot;
            
            _splineAnimate.Container =
                ObjectsReference.Instance.spaceTrafficControlManager.spacePaths[spaceshipData.assignatedHangar]; 
            
            _splineAnimate.Play();
        }
        
        private void TravelOnElevatorPath() {
            _splineAnimate.Container = ObjectsReference.Instance.spaceTrafficControlManager.elevatorPaths[spaceshipData.assignatedHangar];
            _splineAnimate.Alignment = SplineAnimate.AlignmentMode.SplineObject;

            _splineAnimate.Duration = 5;
            
            _splineAnimate.Restart(true);

            distantDot.GetComponent<SpriteRenderer>().enabled = false; // TODO : remove when the overlay on the cannon camera is done 
        }
        
        private void WaitInStation() {
            spaceshipData.travelState = TravelState.WAIT_IN_STATION;

            ConvertToSolidSpaceship();

            switch (spaceshipData.characterType) {
                case CharacterType.MERCHIMP:
                    SpawnMerchimp();
                    break;
                case CharacterType.VISITOR:
                    SpawnVisitors();
                    break;
            }
        }

        private void SpawnVisitors() {
            // TODO : create randomized list of monkeyMens 
            var visitorsQuantity = Random.Range(2, 5);

            for (int i = 0; i < visitorsQuantity; i++) {
                var monkeyMenData = new MonkeyMenData {
                    uid = Guid.NewGuid().ToString(),
                    monkeyMenName = ObjectsReference.Instance.meshReferenceScriptableObject.GetRandomChimpmenName(),
                    characterType = CharacterType.VISITOR,
                    prefabIndex = 0,
                    colorsSet = ObjectsReference.Instance.meshReferenceScriptableObject.GetRandomChimpenColorsPreset(),
                    need = NeedType.FUN,
                    isSatisfied = false,
                    destination = default,
                    rawMaterialsInventory = new Dictionary<RawMaterialType, int>(),
                    ingredientsInventory = new Dictionary<IngredientsType, int>(),
                    manufacturedItemsInventory = new Dictionary<ManufacturedItemsType, int>(),
                    bitKongQuantity = 0,
                    spaceshipGuid = spaceshipData.spaceshipGuid
                };

                visitor = Instantiate(
                    ObjectsReference.Instance.meshReferenceScriptableObject.monkeyMenPrefabs[monkeyMenData.prefabIndex],
                    visitorsSpawnPoint.position,
                    Quaternion.identity,
                    null
                );

                visitorsBehaviours.Add(visitor.GetComponent<VisitorBehaviour>());
                visitor.GetComponent<VisitorBehaviour>().Init(monkeyMenData, visitorsSpawnPoint.position);
            }
        }
        
        private void SpawnMerchimp() {}
        
        private void ConvertToSolidSpaceship() {
            GetComponent<CapsuleCollider>().enabled = false;
            
            foreach (var spaceshipDebrisBehaviour in GetComponentsInChildren<SpaceshipDebrisBehaviour>()) {
                spaceshipDebrisBehaviour.enabled = false;
                spaceshipDebrisBehaviour.GetComponent<MeshCollider>().isTrigger = false;
            }
        }
        
        private void RespawnVisitors(List<MonkeyMenSavedData> groupMembers) {
            foreach (var member in groupMembers) {
                var visitorInstance = Instantiate(
                    original: ObjectsReference.Instance.meshReferenceScriptableObject.monkeyMenPrefabs[member.prefabIndex],
                    position: JsonHelper.FromStringToVector3(member.position),
                    rotation: JsonHelper.FromStringToQuaternion(member.rotation),
                    parent: ObjectsReference.Instance.gameSave.savablesItemsContainer
                );
                visitorInstance.GetComponent<VisitorBehaviour>().LoadSavedData(member);
            }
        }

        public void StopWaiting() {
            ObjectsReference.Instance.spaceTrafficControlManager.FreeHangar(spaceshipData.assignatedHangar);
            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
            
            spaceshipData.travelState = TravelState.TRAVEL_BACK_ON_ELEVATOR_PATH;
            TravelBackOnElevatorPath();
        }
        
        private void TravelBackOnElevatorPath() {
            ObjectsReference.Instance.spaceTrafficControlManager.FreeHangar(spaceshipData.assignatedHangar);
            ObjectsReference.Instance.uiCommunicationPanel.RefreshHangarAvailability();
            ObjectsReference.Instance.spaceshipsSpawner.RemoveGuestInCampaignCreator();
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationQuantityButton();

            ObjectsReference.Instance.spaceTrafficControlManager.elevatorPaths[spaceshipData.assignatedHangar].ReverseFlow(0);

            _splineAnimate.Container =
                ObjectsReference.Instance.spaceTrafficControlManager.elevatorPaths[spaceshipData.assignatedHangar];
            
            _splineAnimate.Play();
        }

        private void TravelBackToSpace() {
            ConvertToExplodableSpaceship();
            
            spaceshipData.travelState = TravelState.LEAVES_THE_REGION;
            spaceshipData.arrivalPosition.y = -10f;
            
            distantDot.SetActive(true);  // TODO : remove when the overlay on the cannon camera is done
        }

        private void LeaveRegion() {
            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Remove(spaceshipData.spaceshipGuid);
            Destroy(gameObject);
            
            ObjectsReference.Instance.uiCommunicationPanel.CloseCommunications(this);
            ObjectsReference.Instance.uiCommunicationPanel.RefreshCommunicationQuantityButton();
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
        
        public void LoadSavedData(SpaceshipSavedData spaceshipSavedDataToLoad) {
            spaceshipData.spaceshipGuid = spaceshipSavedDataToLoad.spaceshipGuid;
            spaceshipData.spaceshipName = spaceshipSavedDataToLoad.spaceshipName;
            spaceshipData.arrivalPosition = JsonHelper.FromStringToVector3(spaceshipSavedDataToLoad.arrivalPoint);
            spaceshipData.characterType = spaceshipSavedDataToLoad.characterType;
            spaceshipData.spaceshipType = spaceshipSavedDataToLoad.spaceshipType;
            spaceshipData.communicationMessagePrefabIndex = spaceshipSavedDataToLoad.communicationMessagePrefabIndex;
            spaceshipData.spaceshipUIcolor = JsonHelper.FromStringToColor(spaceshipSavedDataToLoad.uiColor);
            spaceshipData.travelState = spaceshipSavedDataToLoad.travelState;
            spaceshipData.assignatedHangar = spaceshipSavedDataToLoad.hangarNumber;
            
            spaceshipData.groupTravelState = spaceshipSavedDataToLoad.groupTravelState;
            spaceshipData.guichetsMapsToVisit = spaceshipSavedDataToLoad.guichetsMapsToVisit;
            spaceshipData.mapPointInterests = spaceshipSavedDataToLoad.mapPointInterests;

            this.spaceshipSavedData = spaceshipSavedDataToLoad;

            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Add(spaceshipData.spaceshipGuid, this);
            
            switch (spaceshipData.travelState) {
                case TravelState.FREE_FLIGHT:
                    ObjectsReference.Instance.uiCommunicationPanel.AddNewCommunication(this);
                    break;
                case TravelState.TRAVEL_ON_SPACE_PATH:
                    TravelOnSpacePath(spaceshipData.assignatedHangar);
                    break;
                case TravelState.TRAVEL_ON_ELEVATOR_PATH:
                    TravelOnElevatorPath();
                    break;
                case TravelState.WAIT_IN_STATION:
                    ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(spaceshipData.assignatedHangar);
                    RespawnVisitors(spaceshipSavedDataToLoad.MonkeyMenSavedDatas);
                    break;
                case TravelState.TRAVEL_BACK_ON_ELEVATOR_PATH:
                    TravelBackOnElevatorPath();
                    break;
                case TravelState.LEAVES_THE_REGION:
                    spaceshipData.arrivalPosition.y = -10f;
                    break;
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

            if (spaceshipData.characterType == CharacterType.VISITOR) {
                List<MonkeyMenSavedData> monkeyMenSavedDatas = new List<MonkeyMenSavedData>();
                
                foreach (var visitorBehaviour in visitorsBehaviours) {
                    monkeyMenSavedDatas.Add(visitorBehaviour.GenerateSavedData());
                }

                spaceshipSavedData.MonkeyMenSavedDatas = monkeyMenSavedDatas;
            }
        }
    }
}
