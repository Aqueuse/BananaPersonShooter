using System;
using DG.Tweening;
using InGame.MiniGames.SpaceTrafficControlMiniGame.projectiles;
using Save.Templates;
using Tags;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class SpaceshipBehaviour : MonoBehaviour {
        public string spaceshipGuid;
        public string spaceshipName;
        public Color spaceshipUIcolor;
        public int communicationMessagePrefabIndex;
        public CharacterType characterType;

        public int assignatedHangar;
        private DOTweenPath pathBeetweenSpaceAndHangar;
        private Vector3[] assignatedPathToHangar;
        public float lookAtRotation = 0.01f;
        
        public string savedData;
        
        protected SpaceshipSavedData spaceshipSavedData;
        
        [SerializeField] private Transform spaceshipDebris;
        
        public Vector3 _arrivalPosition;

        private bool _isPropulsing;
        private float _propulsionSpeed;
        private float _step;
        private float _distanceToArrival;

        private Transform _spaceshipTransform;
        private Vector3 _spaceshipPosition;
        
        public TravelState travelState;
        public float timeToNextState;
        
        private void Start() {
            _spaceshipTransform = transform;
        }
        
        public virtual void Init() {}
        
        private void Update() {
            if (travelState == TravelState.FREE_FLIGHT) {
                _spaceshipPosition = _spaceshipTransform.position;

                _step = _propulsionSpeed * Time.deltaTime;
            
                _spaceshipPosition = Vector3.MoveTowards(_spaceshipPosition, _arrivalPosition, _step);
                _spaceshipTransform.position = _spaceshipPosition;
            
                _spaceshipTransform.LookAt(_arrivalPosition, Vector3.up);
                
                if (Vector3.Distance(transform.position, _arrivalPosition) < 10) {
                    ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid.Remove(spaceshipGuid);
                    Destroy(gameObject);
                }
            }

            // TODO : change spaceship Path (HangarDestination)
            // use Bezier class to calculate docking path
            if (travelState == TravelState.GO_TO_STATION) {
                if (Vector3.Distance(transform.position, assignatedPathToHangar[^1]) < 20) {
                    lookAtRotation = 1;
                }
            }

            if (travelState == TravelState.LEAVES_THE_REGION) {
                
            }
        }
        
        public void InitiatePropulsion(Vector3 destination) {
            _arrivalPosition = destination;
            _propulsionSpeed = ObjectsReference.Instance.spaceshipsSpawner.spaceshipPropulsionSpeed;
        }

        public void OpenCommunications() {
            GenerateGuid();
            GenerateName();
            GenerateUIColor();
            GenerateMessage();

            ObjectsReference.Instance.uiSpaceTrafficControlPanel.AddNewCommunication(this);
        }
        
        public void MoveToHangar(int hangarNumber) {
            assignatedHangar = hangarNumber;

            assignatedPathToHangar = ObjectsReference.Instance.spaceTrafficControlManager.pathsToHangars[hangarNumber];
            
            assignatedPathToHangar[0] = transform.position;
            transform.DOPath(
                ObjectsReference.Instance.spaceTrafficControlManager.pathsToHangars[hangarNumber],
                60,
                PathType.CatmullRom).SetLookAt(lookAtRotation);

            travelState = TravelState.GO_TO_STATION;
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
            spaceshipUIcolor = ObjectsReference.Instance.spaceTrafficControlManager.GetRandomColor();
        }

        private void GenerateMessage() {
            var systemRandom = new System.Random();
            communicationMessagePrefabIndex = systemRandom.Next(ObjectsReference.Instance.uiSpaceTrafficControlPanel.spaceshipMessagesByCharacterType[characterType].Count);
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.CANNON_MINI_GAME_PROJECTILE)) return;

            other.GetComponent<Projectile>().Destroy();
            
            GetComponent<BoxCollider>().enabled = false;
            
            foreach (var debris in spaceshipDebris.GetComponentsInChildren<DebrisBehaviour>()) {
                var debrisRigidbody = debris.gameObject.AddComponent<Rigidbody>();
                debrisRigidbody.useGravity = false;
                
                debrisRigidbody.AddExplosionForce(10f, transform.position, 10f);
                debrisRigidbody.AddTorque(debris.transform.position, ForceMode.Impulse);
                debris.GetComponent<DebrisBehaviour>().isInSpace = true;
                debris.GetComponent<DebrisBehaviour>().DestroyIfUnreachable();

                debris.transform.parent = ObjectsReference.Instance.gameSave.debrisSave.debrisContainer.transform;
            }
            
            ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();
            
            ObjectsReference.Instance.uiSpaceTrafficControlPanel.CloseCommunications();
            
            Destroy(gameObject);
        }
        
        public virtual void GenerateSaveData() { }
    }
}
