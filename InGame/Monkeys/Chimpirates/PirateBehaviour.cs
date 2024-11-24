using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Monkeys.PhysicToNavMeshCoordinations;
using Tags;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Monkeys.Chimpirates {
    public class PirateBehaviour : MonoBehaviour {
        public MonkeyMenBehaviour monkeyMenBehaviour;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private SearchWaitingLine searchWaitingLine;
        private SearchBuildableToBreak searchBuildableToBreak;

        [SerializeField] private float explosionForce;

        // synchronize navmeshagent with animator
        private static readonly int startBreakingAnimatorProperty = Animator.StringToHash("break");
        private static readonly int isInAirAnimatorProperty = Animator.StringToHash("isInAir");

        private Vector3 randomDirection;
        private Vector3 finalPosition;

        private BuildableBehaviour buildableToBreak;

        //////////// (👉ﾟヮﾟ)👉   IA  👈(ﾟヮﾟ👈) ///////////
        public float distanceToDestination;
        private NavMeshPath path;

        public float searchTimer = 100;
        private RaycastHit raycastHit;
        private Vector3 rotatingAxis;

        private GameObject itemToDrop;

        private void Start() {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            monkeyMenBehaviour = GetComponent<MonkeyMenBehaviour>();
            searchWaitingLine = monkeyMenBehaviour.searchWaitingLine;
            searchBuildableToBreak = monkeyMenBehaviour.searchBuildableToBreak;
        }

        private void Update() {
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.PLATEFORM_INTERACTION) return;

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME & monkeyMenBehaviour.monkeyMenData.pirateState != PirateState.BREAK_THING) {
                monkeyMenBehaviour.SynchronizeAnimatorAndAgent();
            }
            
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.GO_TO_TELEPORTER) {
                // we must recalcutate distanceToDestination for each case 🎃 🍌
                // to prevent the Slipping Of The State Machine Of Death 👻 😱
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                
                if (distanceToDestination < 1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position);
                    searchWaitingLine.enabled = true;
                    monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.SEARCH_WAITING_LINE;
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.SEARCH_WAITING_LINE) {
                searchTimer -= 1;
                
                if (searchTimer < 0) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
                    
                    monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenBehaviour.monkeyMenData.spaceshipGuid].chimpMensSpawnPoint.position;
                    monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_BACK_TO_SPACESHIP;
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.SEARCH_RANDOM_POINT) {
                var randomPosition = transform.position + Random.insideUnitSphere * 100;

                if (NavMesh.SamplePosition(randomPosition, out var navMeshHit, 2, 1)) {
                    if (Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position) > 10) {
                        path = new NavMeshPath();
                        if (_navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                            if (path.status == NavMeshPathStatus.PathComplete) {
                                monkeyMenBehaviour.monkeyMenData.destination = navMeshHit.position;
                                monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_TO_RANDOM_POINT;
                            }
                        }
                    }
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.GO_TO_RANDOM_POINT) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);
                
                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                
                if (distanceToDestination < 2f) {
                    searchTimer = 100;
                    monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.SEARCH_THING_TO_BREAK; 
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.SEARCH_THING_TO_BREAK) {
                searchTimer -= 1;
                
                if (searchBuildableToBreak.hasFoundBuildable) {
                    buildableToBreak = searchBuildableToBreak.buildableFounded;
                    buildableToBreak.isPirateTargeted = true;
                    monkeyMenBehaviour.monkeyMenData.destination = buildableToBreak.ChimpTargetTransform.position;
                    monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_BREAK_THING;

                    searchBuildableToBreak.hasFoundBuildable = false;
                    searchBuildableToBreak.enabled = false;
                }

                else if (searchTimer < 0) {
                    monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position;
                    monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_BACK_TO_TELEPORTER;
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.GO_BREAK_THING) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);

                if (distanceToDestination < 1f) {
                    _animator.SetTrigger(startBreakingAnimatorProperty);
                    buildableToBreak.BreakBuildable();

                    monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.BREAK_THING;

                    var rawMaterialsWithQuantity = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableToBreak.buildableType].rawMaterialsWithQuantity; 

                    // can't copy directly from itemScriptableObject - Sadge ╯︿╰
                    foreach (var rawMaterial in rawMaterialsWithQuantity) {
                        monkeyMenBehaviour.monkeyMenData.rawMaterialsInventory[rawMaterial.Key] = rawMaterial.Value;
                    }
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.GO_BACK_TO_TELEPORTER | monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.FLEE) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                
                if (distanceToDestination < 1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
                    
                    monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenBehaviour.monkeyMenData.spaceshipGuid].chimpMensSpawnPoint.position;
                    monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_BACK_TO_SPACESHIP;
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.GO_BACK_TO_SPACESHIP) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);

                if (distanceToDestination < 1f) {
                    ObjectsReference.Instance.spaceshipsSpawner.RemoveGuestInCampaignCreator();

                    monkeyMenBehaviour.associatedSpaceshipBehaviour.travelers.Remove(monkeyMenBehaviour);

                    if (monkeyMenBehaviour.associatedSpaceshipBehaviour.travelers.Count == 0) {
                        monkeyMenBehaviour.associatedSpaceshipBehaviour.StopWaiting();
                    }
                    
                    Destroy(gameObject);
                }
            }
        }

        public void StartPiracy() {
            searchTimer = 1000;
            monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.SEARCH_THING_TO_BREAK;
            searchBuildableToBreak.enabled = true;
        }
        
        // Banana ! Σ(っ °Д °;)っ
        public void Flee() {
            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.FLEE) return;

            _animator.SetBool(isInAirAnimatorProperty, false);
            
            DropPartOfInventory();

            _animator.SetLayerWeight(1, 1);

            monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position;
            monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.FLEE;
        }

        private void DropPartOfInventory() {
            var distanceToDropFromCenter = Random.Range(0.1f, 0.5f);
            var explositionCenterPosition = transform.position;
            
            // lâche quelques trucs dans l'inventaire s'il y en a encore
            foreach (var itemInInventory in monkeyMenBehaviour.monkeyMenData.rawMaterialsInventory) {
                for (var i = 0; i < itemInInventory.Value; i++) {
                    distanceToDropFromCenter += Random.Range(0.1f, 0.5f);
                    var spawnPosition = explositionCenterPosition + Random.insideUnitSphere * distanceToDropFromCenter;
                    if (spawnPosition.y < 1) spawnPosition.y = 1;

                    itemToDrop =
                        Instantiate(
                            ObjectsReference.Instance.meshReferenceScriptableObject.prefabByRawMaterialType[itemInInventory.Key],
                            spawnPosition,
                            Quaternion.identity);
                    itemToDrop.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explositionCenterPosition, 10);
                }
            }

            monkeyMenBehaviour.monkeyMenData.rawMaterialsInventory.Clear();
        }

        public void GoBackToTeleporter() {
            monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position;
            monkeyMenBehaviour.monkeyMenData.pirateState = PirateState.GO_BACK_TO_TELEPORTER;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != 7) return;
            
            if (other.GetComponent<Tag>() == null) return;
            if (other.GetComponent<Tag>().itemScriptableObject == null) return;

            if (other.GetComponent<Tag>().itemScriptableObject.buildableType == BuildableType.BUMPER) {
                other.GetComponent<PlateformBehaviour>().isPirateTargeted = false;
                other.GetComponent<PlateformBehaviour>().Activate(GetComponent<Rigidbody>(), 7000);
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 11) return;

            if (monkeyMenBehaviour.monkeyMenData.pirateState == PirateState.PLATEFORM_INTERACTION) {
                GetComponent<ChimpMenPhysicNavMeshCoordination>().SwitchToNavMeshAgent();
                GoBackToTeleporter();
            }
        }
        
    }
}