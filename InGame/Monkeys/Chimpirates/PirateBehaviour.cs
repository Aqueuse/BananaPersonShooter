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
        
        [SerializeField] private float explosionForce;
        
        // synchronize navmeshagent with animator
        private static readonly int startBreakingAnimatorProperty = Animator.StringToHash("break");
        private static readonly int isInAirAnimatorProperty = Animator.StringToHash("isInAir");
        
        private Vector3 randomDirection;
        private Vector3 finalPosition;
        
        private BuildableBehaviour buildableToBreak;
        
        //////////// (👉ﾟヮﾟ)👉   IA  👈(ﾟヮﾟ👈) ///////////
        public PirateState pirateState = PirateState.GO_TO_TELEPORTER;
        public float distanceToDestination;
        private NavMeshPath path;

        public float searchTimer;
        private RaycastHit raycastHit;
        private Vector3 rotatingAxis;

        private GameObject itemToDrop;

        private void Start() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            monkeyMenBehaviour = GetComponent<MonkeyMenBehaviour>();
        }

        private void Update() {
            if (pirateState == PirateState.PLATEFORM_INTERACTION) return;

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME && pirateState != PirateState.BREAK_THING) {
                monkeyMenBehaviour.SynchronizeAnimatorAndAgent();
            }

            if (pirateState == PirateState.GO_TO_TELEPORTER) {
                _navMeshAgent.SetDestination(monkeyMenBehaviour.destination);

                if (distanceToDestination < 0.1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
                    searchTimer = 1000;
                    pirateState = PirateState.SEARCH_THING_TO_BREAK;
                    monkeyMenBehaviour.searchBuildableToBreak.enabled = true;
                }
            }

            if (pirateState == PirateState.SEARCH_THING_TO_BREAK) {
                searchTimer -= 1;
                
                if (searchTimer < 0) {
                    GoBackToTeleporter();
                }

                if (monkeyMenBehaviour.searchBuildableToBreak.hasFoundBuildable) {
                    buildableToBreak = monkeyMenBehaviour.searchBuildableToBreak.buildableFounded;
                    buildableToBreak.isPirateTargeted = true;
                    monkeyMenBehaviour.destination = buildableToBreak.ChimpTargetTransform.position;
                    pirateState = PirateState.GO_BREAK_THING;

                    monkeyMenBehaviour.searchBuildableToBreak.hasFoundBuildable = false;
                    monkeyMenBehaviour.searchBuildableToBreak.enabled = false;
                }
            }

            if (pirateState == PirateState.SEARCH_RANDOM_POINT) {
                var randomPosition = transform.position + Random.insideUnitSphere * 100;

                if (NavMesh.SamplePosition(randomPosition, out var navMeshHit, 2, 1)) {
                    if (Vector3.Distance(monkeyMenBehaviour.destination, transform.position) > 10) {
                        path = new NavMeshPath();
                        if (_navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                            if (path.status == NavMeshPathStatus.PathComplete) {
                                monkeyMenBehaviour.destination = navMeshHit.position;
                                pirateState = PirateState.GO_TO_RANDOM_POINT;
                            }
                        }
                    }
                }
            }

            distanceToDestination = Vector3.Distance(monkeyMenBehaviour.destination, transform.position);

            if (pirateState == PirateState.GO_BREAK_THING) {
                _navMeshAgent.SetDestination(monkeyMenBehaviour.destination);

                if (distanceToDestination < 1f) {
                    _animator.SetTrigger(startBreakingAnimatorProperty);
                    buildableToBreak.BreakBuildable();

                    pirateState = PirateState.BREAK_THING;

                    var rawMaterialsWithQuantity = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableToBreak.buildableType].rawMaterialsWithQuantity; 

                    // can't copy directly from itemScriptableObject - Sadge ╯︿╰
                    foreach (var rawMaterial in rawMaterialsWithQuantity) {
                        monkeyMenBehaviour.monkeyMenData.droppedInventory[rawMaterial.Key] = rawMaterial.Value;
                    }
                }
            }

            if (pirateState == PirateState.GO_TO_RANDOM_POINT) {
                _navMeshAgent.SetDestination(monkeyMenBehaviour.destination);
                
                if (distanceToDestination < 2f) {
                    pirateState = PirateState.GO_TO_TELEPORTER; 
                    Invoke(nameof(GoBackToTeleporter), 5);
                }
            }

            if (pirateState == PirateState.GO_BACK_TO_TELEPORTER || pirateState == PirateState.FLEE) {
                _navMeshAgent.SetDestination(monkeyMenBehaviour.destination);
                
                if (distanceToDestination < 1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
                    monkeyMenBehaviour.destination = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenBehaviour.monkeyMenData.spaceshipGuid].spawnPoint.position;
                    pirateState = PirateState.GO_BACK_TO_SPACESHIP;
                }
            }

            if (pirateState == PirateState.GO_BACK_TO_SPACESHIP) {
                if (distanceToDestination < 1f) {
                    ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();
                    monkeyMenBehaviour.associatedSpaceship.StopWaiting();
                    Destroy(gameObject);
                }
            }
        }
        
        // Banana ! Σ(っ °Д °;)っ
        public void Flee() {
            if (pirateState == PirateState.FLEE) return;

            _animator.SetBool(isInAirAnimatorProperty, false);
            
            DropPartOfInventory();

            _animator.SetLayerWeight(1, 1);

            monkeyMenBehaviour.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position;
            pirateState = PirateState.FLEE;
        }

        private void DropPartOfInventory() {
            var distanceToDropFromCenter = Random.Range(0.1f, 0.5f);
            var explositionCenterPosition = transform.position;
            
            // lâche quelques trucs dans l'inventaire s'il y en a encore
            foreach (var itemInInventory in monkeyMenBehaviour.monkeyMenData.droppedInventory) {
                for (int i = 0; i < itemInInventory.Value; i++) {
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

            monkeyMenBehaviour.monkeyMenData.droppedInventory.Clear();
        }

        public void GoBackToTeleporter() {
            pirateState = PirateState.GO_BACK_TO_TELEPORTER;
            monkeyMenBehaviour.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != 7) return;

            if (other.GetComponent<Tag>().itemScriptableObject.buildableType == BuildableType.BUMPER) {
                other.GetComponent<PlateformBehaviour>().isPirateTargeted = false;
                other.GetComponent<PlateformBehaviour>().Activate(GetComponent<Rigidbody>(), 7000);
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 11) return;

            if (pirateState == PirateState.PLATEFORM_INTERACTION) {
                GetComponent<ChimpMenPhysicNavMeshCoordination>().SwitchToNavMeshAgent();
                GoBackToTeleporter();
            }
        }
        
    }
}