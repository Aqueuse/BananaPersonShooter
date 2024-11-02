using System.Collections.Generic;
using System.Linq;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimptouristes {
    public class TouristBehaviour : MonoBehaviour {
        public MonkeyMenBehaviour monkeyMenBehaviour;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator animator;
        
        //////////// (👉ﾟヮﾟ)👉   IA  👈(ﾟヮﾟ👈) ///////////
        [SerializeField] private SearchBuildableToUse searchBuildableToUse;
        
        public NeedType actualNeed;

        public List<BuildableBehaviour> visitedBuildables;

        private Vector3 rotatingAxis;
        private RaycastHit raycastHit;
        
        private static readonly int isSitting = Animator.StringToHash("isSitting");
        
        private NavMeshPath path;
        public float distanceToDestination;
        public float searchTimer;

        public BuildableBehaviour buildableToReach;

        public int notoriety;

        private void Start() {
            animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(1, 1, 1);
            
            monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.chimpManager.waitingSpot.position;
            _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
        }

        private void Update() {
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.IN_WAITING_LINE) return;
            
            monkeyMenBehaviour.SynchronizeAnimatorAndAgent();

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_TO_WAITING_LINE) {
                if (distanceToDestination < 0.5f) {
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.IN_WAITING_LINE;
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.SEARCH_NEED) {
                searchTimer -= 1;
                if (searchTimer < 0) {
                    monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position;
                    _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_BACK_TO_TELEPORTER;
                }

                if (searchBuildableToUse.hasFoundBuildable) {
                    buildableToReach = searchBuildableToUse.buildableFounded;
                    buildableToReach.isVisitorTargeted = true;
                    visitedBuildables.Add(buildableToReach);
                    
                    monkeyMenBehaviour.monkeyMenData.destination = buildableToReach.ChimpTargetTransform.position;
                    _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_FILL_NEED;

                    searchBuildableToUse.hasFoundBuildable = false;
                    searchBuildableToUse.enabled = false;
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.SEARCH_RANDOM_POINT) {
                var randomPosition = transform.position + Random.insideUnitSphere * 100;

                if (NavMesh.SamplePosition(randomPosition, out var navMeshHit, 2, 1)) {
                    if (Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position) > 10) {
                        path = new NavMeshPath();
                        if (_navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                            if (path.status == NavMeshPathStatus.PathComplete) {
                                monkeyMenBehaviour.monkeyMenData.destination = navMeshHit.position;
                                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                                monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_TO_RANDOM_POINT;
                            }
                        }
                    }
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_TO_RANDOM_POINT) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                if (distanceToDestination < 0.5f) {
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_BACK_TO_TELEPORTER;
                    monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position;
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_FILL_NEED) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                if (distanceToDestination < 0.5f) {
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.FILLING_NEED;
                    if (buildableToReach.visitorsBuildablePropertiesScriptableObject.isAnimationLooping) {
                        Invoke(nameof(FillNeed), 10);
                    }

                    _navMeshAgent.updateRotation = false;
                    animator.SetTrigger(buildableToReach.visitorsBuildablePropertiesScriptableObject.animatorParameterToActivate);
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.FILLING_NEED) {
                transform.LookAt(buildableToReach.ChimpTargetLookAtTransform, worldUp:Vector3.up);
            }
            
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_BACK_TO_TELEPORTER) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                if (distanceToDestination < 6f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
                    _navMeshAgent.SetDestination(monkeyMenBehaviour.associatedSpaceship.spawnPoint.position);
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_BACK_TO_SPACESHIP;
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_BACK_TO_SPACESHIP) {
                if (distanceToDestination < 6f) {
                    ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();
                    // TODO : HAD NOTORIETY FEEDBACK
                    monkeyMenBehaviour.associatedSpaceship.StopWaiting();
                    Destroy(gameObject);
                }
            }
        }

        public void StartVisiting() {
            searchTimer = 1000;
            monkeyMenBehaviour.monkeyMenData.touristState = TouristState.SEARCH_NEED;
            SortNeeds();
            searchBuildableToUse.enabled = true;
        }

        public void FillNeed() {
            monkeyMenBehaviour.monkeyMenData.needs[buildableToReach.visitorsBuildablePropertiesScriptableObject.needType] -= buildableToReach.visitorsBuildablePropertiesScriptableObject.needValue;
            visitedBuildables.Add(buildableToReach);
            buildableToReach.isVisitorTargeted = false;
            
            if (actualNeed == NeedType.REST) animator.SetBool(isSitting, false);
            
            SortNeeds();
            
            searchBuildableToUse.enabled = true;
            searchTimer = 1000;

            _navMeshAgent.updateRotation = true;
            monkeyMenBehaviour.monkeyMenData.touristState = TouristState.SEARCH_NEED;
        }

        private void SortNeeds() {
            monkeyMenBehaviour.sortedNeeds = monkeyMenBehaviour.monkeyMenData.needs.OrderByDescending(pair => pair.Value);
        }
    }
}
