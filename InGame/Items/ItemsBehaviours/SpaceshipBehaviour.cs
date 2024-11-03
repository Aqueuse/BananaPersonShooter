using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using InGame.Items.ItemsData;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using InGame.Monkeys;
using InGame.Monkeys.Chimpirates;
using InGame.Monkeys.Chimptouristes;
using InGame.Monkeys.Merchimps;
using InGame.SpaceTrafficControl;
using Save.Helpers;
using Save.Templates;
using Tags;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class SpaceshipBehaviour : MonoBehaviour {
        public List<TouristData> touristDatas;
        private MerchantData merchantData;
        public readonly List<MonkeyMenData> monkeyMensData = new();

        [HideInInspector] public SpaceshipSavedData spaceshipSavedData;

        private GameObject chimpmenInstance;
        
        public string spaceshipGuid;
        public string spaceshipName;
        public Color spaceshipUIcolor;
        public int communicationMessagePrefabIndex;
        public CharacterType characterType;

        public int assignatedHangar;
        public Transform spawnPoint;

        private int debrisIndexQuantity;

        private DOTweenPath pathBeetweenSpaceAndHangar;
        private Vector3[] assignatedPathToHangar;
        private const float lookAtRotation = 0.01f;

        public Vector3 arrivalPosition;

        private bool _isPropulsing;
        private const float _propulsionSpeed = 100f;
        private float _step;
        private float _distanceToArrival;

        private Transform _spaceshipTransform;
        private Vector3 _spaceshipPosition;

        private SpaceshipDebrisBehaviour spaceshipDebrisBehaviour;
        private PirateBehaviour pirateBehaviour;
        
        public TravelState travelState;

        private void Start() {
            _spaceshipTransform = transform;
            
            merchantData = spaceshipSavedData.merchantData;

            if (spaceshipSavedData.travelState == TravelState.WAIT_IN_STATION) {
                ObjectsReference.Instance.spaceTrafficControlManager.AssignSpaceshipToHangar(assignatedHangar);
            }
        }
        
        private void Update() {
            if (travelState == TravelState.GO_TO_PATH) {
                _step = _propulsionSpeed * 4 * Time.deltaTime;

                _spaceshipTransform.position = Vector3.MoveTowards(_spaceshipTransform.position, assignatedPathToHangar[0], _step);
                _spaceshipTransform.LookAt(assignatedPathToHangar[0], Vector3.up);

                if (Vector3.Distance(transform.position, assignatedPathToHangar[0]) < 10) {
                    travelState = TravelState.TRAVEL_ON_PATH;
                    MoveToElevator();
                }
            }

            if (travelState == TravelState.FREE_FLIGHT || travelState == TravelState.LEAVES_THE_REGION) {
                _step = _propulsionSpeed * Time.deltaTime;
            
                _spaceshipTransform.position = Vector3.MoveTowards(_spaceshipTransform.position, arrivalPosition, _step);
            
                _spaceshipTransform.LookAt(arrivalPosition, Vector3.up);
                
                if (Vector3.Distance(transform.position, arrivalPosition) < 10) {
                    LeaveRegion();
                }
            }
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.LASER)) return;
            
            GetComponent<CapsuleCollider>().enabled = false;

            foreach (var debrisBehaviour in gameObject.GetComponentsInChildren<SpaceshipDebrisBehaviour>()) {
                debrisBehaviour.Init(other.GetComponent<Laser>());
            }
            
            ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();

            ObjectsReference.Instance.uiSpaceTrafficControlPanel.CloseCommunications(this);
        }
        
        public void GenerateSpaceshipData() {
            GenerateGuid();
            GenerateName();
            GenerateUIColor();
            GenerateMessage();
        }

        public void OpenCommunications() {
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.AddNewCommunication(this);
        }

        private void WaitInStation() {
            travelState = TravelState.WAIT_IN_STATION;

            var chimpmen = Instantiate(
                ObjectsReference.Instance.meshReferenceScriptableObject.GetRandomChimpmen(), 
                spawnPoint.position, Quaternion.identity,
                ObjectsReference.Instance.gameSave.chimpmensContainer
            );

            chimpmen.GetComponent<MonkeyMenBehaviour>().associatedSpaceship = this;

            if (characterType == CharacterType.PIRATE) {
                chimpmen.AddComponent<PirateBehaviour>();
            }

            if (characterType == CharacterType.TOURIST) {
                chimpmenInstance.AddComponent<TouristBehaviour>();
            }

            if (characterType == CharacterType.MERCHIMP) {
                chimpmenInstance.AddComponent<MerchimpBehaviour>();
            }
        }
        
        public void StopWaiting() {
            ObjectsReference.Instance.spaceTrafficControlManager.FreeHangar(assignatedHangar);
            
            travelState = TravelState.TRAVEL_BACK_ON_ELEVATOR;
            TravelBackOnElevator();
        }
        
        public void GoToPath(int hangarNumber) {
            assignatedHangar = hangarNumber;
            assignatedPathToHangar = ObjectsReference.Instance.spaceTrafficControlManager.pathsToHangars[assignatedHangar];

            travelState = TravelState.GO_TO_PATH;
        }
        
        public void MoveToElevator() {
            travelState = TravelState.TRAVEL_ON_PATH;

            transform.DOPath(
                ObjectsReference.Instance.spaceTrafficControlManager.pathsToHangars[assignatedHangar],
                20,
                PathType.CatmullRom).SetLookAt(lookAtRotation).SetEase(Ease.Linear).OnComplete(MoveOnElevatorToHangar);
        }

        public void MoveOnElevatorToHangar() {
            transform.DOPath(
                ObjectsReference.Instance.spaceTrafficControlManager.elevatorsPaths[assignatedHangar],
                20,
                PathType.CatmullRom).SetLookAt(
                ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.SPACESHIP_HANGARS],
                stableZRotation:true).OnComplete(WaitInStation);
        }

        private void TravelBackOnElevator() {
            var reversePathFromHangarToElevator = ObjectsReference.Instance.spaceTrafficControlManager.elevatorsPaths[assignatedHangar].Reverse().ToArray(); 

            transform.DOPath(reversePathFromHangarToElevator,
                20,
                PathType.CatmullRom).OnComplete(TravelBackToSpace);
        }

        private void TravelBackToSpace() {
            travelState = TravelState.LEAVES_THE_REGION;
            arrivalPosition.y = -10f;
        }

        private void LeaveRegion() {
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.CloseCommunications(this);
            ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Remove(spaceshipGuid);

            ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();
            Destroy(gameObject);
        }
        
        private void GenerateGuid() {
            spaceshipSavedData = new SpaceshipSavedData();

            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }
        }

        private void GenerateName() {
            spaceshipName = ObjectsReference.Instance.spaceTrafficControlManager.GetUniqueSpaceshipName();
        }

        private void GenerateUIColor() {
            spaceshipUIcolor = SpaceTrafficControlManager.GetRandomColor();
        }

        private void GenerateMessage() {
            var systemRandom = new System.Random();
            communicationMessagePrefabIndex = systemRandom.Next(ObjectsReference.Instance.uiSpaceTrafficControlPanel.spaceshipMessagesByCharacterType[characterType].Count);
        }

        public void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            spaceshipSavedData = new SpaceshipSavedData {
                spaceshipName = spaceshipName,
                spaceshipGuid = spaceshipGuid,
                communicationMessagePrefabIndex = communicationMessagePrefabIndex,
                uiColor = JsonHelper.FromColorToString(spaceshipUIcolor),
                merchantData = merchantData,
                characterType = characterType,
                spaceshipPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                travelState = travelState,
                touristDatas = touristDatas,
                arrivalPoint = JsonHelper.FromVector3ToString(arrivalPosition),
                hangarNumber = assignatedHangar
            };
        }
    }
}
