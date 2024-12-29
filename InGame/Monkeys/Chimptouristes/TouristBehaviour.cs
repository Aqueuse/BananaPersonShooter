using System.Collections.Generic;
using System.Linq;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Buildables.VisitorsBuildable;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimptouristes {
    public class TouristBehaviour : MonoBehaviour {
        public MonkeyMenBehaviour monkeyMenBehaviour;
        private NavMeshAgent _navMeshAgent;
        private Animator animator;
        private SearchWaitingLine searchWaitingLine;
        private SearchBuildableToUse searchBuildableToUse;

        private static readonly int isSitting = Animator.StringToHash("isSitting");

        //////////// (👉ﾟヮﾟ)👉   IA  👈(ﾟヮﾟ👈) ///////////
        public float distanceToDestination;
        private NavMeshPath path;
        
        public float searchTimer = 100;
        private RaycastHit raycastHit;
        private Vector3 rotatingAxis;
        
        public NeedType actualNeed;
        public List<BuildableBehaviour> visitedBuildables;
        
        public BuildableBehaviour buildableToReach;

        public int notoriety;

        private void Start() {
            animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            monkeyMenBehaviour = GetComponent<MonkeyMenBehaviour>();
            searchWaitingLine = monkeyMenBehaviour.searchWaitingLine;
            searchBuildableToUse = monkeyMenBehaviour.searchBuildableToUse;
        }

        private void Update() {
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.IN_WAITING_LINE) return;
            
            monkeyMenBehaviour.SynchronizeAnimatorAndAgent();
            
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_TO_TELEPORTER) {
                // we must recalcutate distanceToDestination for each case 🎃 🍌
                // to prevent the Slipping Of The State Machine Of Death 👻 😱
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                
                if (distanceToDestination < 1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position);
                    searchWaitingLine.enabled = true;
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.SEARCH_WAITING_LINE;
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.SEARCH_WAITING_LINE) {
                searchTimer -= 1;
                
                if (searchTimer < 0) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
                    
                    monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenBehaviour.monkeyMenData.spaceshipGuid].chimpMensSpawnPoint.position;
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_BACK_TO_SPACESHIP;
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_TO_WAITING_LINE) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                if (distanceToDestination < 1f) {
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

                if (distanceToDestination < 1f) {
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_BACK_TO_TELEPORTER;
                    monkeyMenBehaviour.monkeyMenData.destination = ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position;
                }
            }
            
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_FILL_NEED) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                if (distanceToDestination < 1f) {
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.FILLING_NEED;

                    var buildableData = (VisitorsBuildablePropertiesScriptableObject)buildableToReach.buildablePropertiesScriptableObject;
                    
                    if (buildableData.isAnimationLooping) {
                        Invoke(nameof(FillNeed), 10);
                    }

                    _navMeshAgent.updateRotation = false;
                    animator.SetTrigger(buildableData.animatorParameterToActivate);
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.FILLING_NEED) {
                transform.LookAt(buildableToReach.ChimpTargetLookAtTransform, worldUp:Vector3.up);
            }
            
            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_BACK_TO_TELEPORTER) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);

                if (distanceToDestination < 1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_HANGARS].position);
                    
                    monkeyMenBehaviour.monkeyMenData.destination = monkeyMenBehaviour.associatedSpaceshipBehaviour.chimpMensSpawnPoint.position; 
                    _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);
                    
                    monkeyMenBehaviour.monkeyMenData.touristState = TouristState.GO_BACK_TO_SPACESHIP;
                }
            }

            if (monkeyMenBehaviour.monkeyMenData.touristState == TouristState.GO_BACK_TO_SPACESHIP) {
                distanceToDestination = Vector3.Distance(monkeyMenBehaviour.monkeyMenData.destination, transform.position);
                
                _navMeshAgent.SetDestination(monkeyMenBehaviour.monkeyMenData.destination);

                if (distanceToDestination < 1f) {
                    ObjectsReference.Instance.spaceshipsSpawner.RemoveGuestInCampaignCreator();
                    
                    // TODO : HAD NOTORIETY FEEDBACK
                    
                    monkeyMenBehaviour.associatedSpaceshipBehaviour.travelers.Remove(monkeyMenBehaviour);

                    if (monkeyMenBehaviour.associatedSpaceshipBehaviour.travelers.Count == 0) {
                        monkeyMenBehaviour.associatedSpaceshipBehaviour.StopWaiting();
                    }
                    
                    Destroy(gameObject);
                }
            }
        }

        public void StartVisiting() {
            searchTimer = 100;
            monkeyMenBehaviour.monkeyMenData.touristState = TouristState.SEARCH_NEED;
            SortNeeds();
            searchBuildableToUse.enabled = true;
        }

        public void FillNeed() {
            var buildableData = (VisitorsBuildablePropertiesScriptableObject)buildableToReach.buildablePropertiesScriptableObject;
            
            monkeyMenBehaviour.monkeyMenData.needs[buildableData.needType] -= buildableData.needValue;
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
