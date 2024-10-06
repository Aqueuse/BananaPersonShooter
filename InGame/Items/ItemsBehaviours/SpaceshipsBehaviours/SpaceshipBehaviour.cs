using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using InGame.Items.ItemsData;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using InGame.SpaceTrafficControl;
using Save.Templates;
using Tags;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class SpaceshipBehaviour : MonoBehaviour {
        [HideInInspector] public SpaceshipSavedData spaceshipSavedData;
        public List<MonkeyMenData> monkeyMensData = new();

        public string spaceshipGuid;
        public string spaceshipName;
        public Color spaceshipUIcolor;
        public int communicationMessagePrefabIndex;
        public CharacterType characterType;

        public int assignatedHangar;
        public Transform spawnPoint;
        
        private DOTweenPath pathBeetweenSpaceAndHangar;
        private Vector3[] assignatedPathToHangar;
        public float lookAtRotation = 0.01f;
        
        public SpaceshipSavedData savedData;
        
        [SerializeField] private Transform spaceshipDebris;
        
        public Vector3 arrivalPosition;

        private bool _isPropulsing;
        private const float _propulsionSpeed = 100f;
        private float _step;
        private float _distanceToArrival;

        private Transform _spaceshipTransform;
        private Vector3 _spaceshipPosition;
        
        public TravelState travelState;
        
        private void Start() {
            _spaceshipTransform = transform;
        }
        
        public virtual void Init() {}
        
        private void Update() {
            if (travelState == TravelState.GO_TO_PATH) {
                _step = _propulsionSpeed*4 * Time.deltaTime;
            
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

            if (characterType == CharacterType.MERCHIMP) {
                GetComponent<MerchantSpaceshipBehaviour>().StartWaitingTimer();
            }
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
                ObjectsReference.Instance.spaceTrafficControlManager.teleportUpTransform,
                stableZRotation:true).OnComplete(WaitInStation);
        }

        protected void TravelBackOnElevator() {
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
        
        private void OnTriggerEnter(Collider other) {
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.CANNON_MINI_GAME_PROJECTILE)) return;

            other.GetComponent<Projectile>().Destroy();
            
            GetComponent<BoxCollider>().enabled = false;
            
            foreach (var debris in spaceshipDebris.GetComponentsInChildren<SpaceshipDebrisBehaviour>()) {
                var debrisRigidbody = debris.gameObject.AddComponent<Rigidbody>();
                debrisRigidbody.useGravity = false;
                
                debrisRigidbody.AddExplosionForce(10f, transform.position, 10f);
                debrisRigidbody.AddTorque(debris.transform.position, ForceMode.Impulse);
                debris.GetComponent<SpaceshipDebrisBehaviour>().isInSpace = true;
                debris.GetComponent<SpaceshipDebrisBehaviour>().DestroyIfUnreachable();

                debris.transform.parent = ObjectsReference.Instance.gameSave.spaceshipDebrisSave.spaceshipDebrisContainer.transform;
            }

            ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();

            ObjectsReference.Instance.uiSpaceTrafficControlPanel.CloseCommunications(this);

            Destroy(gameObject);
        }

        public virtual void GenerateSaveData() { }
    }
}
