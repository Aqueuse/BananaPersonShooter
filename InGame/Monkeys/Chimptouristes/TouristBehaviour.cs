using System.Collections.Generic;
using System.Linq;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Characters.Visitors;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Chimptouristes {
    public class TouristBehaviour : MonoBehaviour {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _transform;
        [SerializeField] private Animator animator;
        [SerializeField] private SearchBuildableToUse searchBuildableToUse;

        public TouristPropertiesScriptableObject touristPropertiesScriptableObject;
        public string visitorPosition;
        public string visitorRotation;

        //////////// (👉ﾟヮﾟ)👉   IA  👈(ﾟヮﾟ👈) ///////////

        public IOrderedEnumerable<KeyValuePair<NeedType, int>> sortedNeeds;
        public NeedType actualNeed;

        public List<BuildableBehaviour> visitedBuildables;
        public TouristState touristState;

        private Vector3 rotatingAxis;
        private RaycastHit raycastHit;
        
        private static readonly int VelocityX = Animator.StringToHash("XVelocity");
        private static readonly int VelocityZ = Animator.StringToHash("ZVelocity");
        private static readonly int ShouldMove = Animator.StringToHash("shouldMove");
        private static readonly int isSitting = Animator.StringToHash("isSitting");

        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

        private NavMeshPath path;
        public float distanceToDestination;
        public float searchTimer;

        public BuildableBehaviour buildableToReach;
        public Vector3 destination;

        public int notoriety;

        private void Start() {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(1, 1, 1);
        }

        private void Update() {
            if (touristState == TouristState.IN_WAITING_LINE) return;
            
            SynchronizeAnimatorAndAgent();

            if (touristState == TouristState.SEARCH_NEED) {
                searchTimer -= 1;
                if (searchTimer < 0) {
                    destination = ObjectsReference.Instance.chimpManager.sasTransform.position;
                    _navMeshAgent.SetDestination(destination);
                    touristState = TouristState.GO_BACK_TO_SAS;
                }

                if (searchBuildableToUse.hasFoundBuildable) {
                    buildableToReach = searchBuildableToUse.buildableFounded;
                    buildableToReach.isVisitorTargeted = true;
                    visitedBuildables.Add(buildableToReach);
                    
                    destination = buildableToReach.ChimpTargetTransform.position;
                    _navMeshAgent.SetDestination(destination);
                    touristState = TouristState.GO_FILL_NEED;

                    searchBuildableToUse.hasFoundBuildable = false;
                    searchBuildableToUse.enabled = false;
                }
            }
            
            if (touristState == TouristState.SEARCH_RANDOM_POINT) {
                var randomPosition = transform.position + Random.insideUnitSphere * 100;

                if (NavMesh.SamplePosition(randomPosition, out var navMeshHit, 2, 1)) {
                    if (Vector3.Distance(destination, transform.position) > 10) {
                        path = new NavMeshPath();
                        if (_navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                            if (path.status == NavMeshPathStatus.PathComplete) {
                                destination = navMeshHit.position;
                                _navMeshAgent.SetDestination(destination);
                                touristState = TouristState.GO_TO_RANDOM_POINT;
                            }
                        }
                    }
                }
            }

            if (touristState == TouristState.GO_TO_RANDOM_POINT) {
                distanceToDestination = Vector3.Distance(destination, transform.position);

                if (distanceToDestination < 0.5f) {
                    touristState = TouristState.GO_BACK_TO_SAS;
                    destination = ObjectsReference.Instance.chimpManager.sasTransform.position;
                }
            }
            
            if (touristState == TouristState.GO_FILL_NEED) {
                distanceToDestination = Vector3.Distance(destination, transform.position);

                if (distanceToDestination < 0.5f) {
                    touristState = TouristState.FILLING_NEED;
                    if (buildableToReach.visitorsBuildablePropertiesScriptableObject.isAnimationLooping) {
                        Invoke(nameof(FillNeed), 10);
                    }

                    _navMeshAgent.updateRotation = false;
                    animator.SetTrigger(buildableToReach.visitorsBuildablePropertiesScriptableObject.animatorParameterToActivate);
                }
            }

            if (touristState == TouristState.FILLING_NEED) {
                transform.LookAt(buildableToReach.ChimpTargetLookAtTransform, worldUp:Vector3.up);
            }
            
            if (touristState == TouristState.GO_BACK_TO_SAS) {
                distanceToDestination = Vector3.Distance(destination, transform.position);

                if (distanceToDestination < 6f) {
                    ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();

                    // TODO : HAD NOTORIETY FEEDBACK

                    Destroy(gameObject);
                }
            }
        }

        public void StartVisiting() {
            searchTimer = 1000;
            touristState = TouristState.SEARCH_NEED;
            SortNeeds();
            searchBuildableToUse.enabled = true;
        }

        public void FillNeed() {
            touristPropertiesScriptableObject.visitorNeeds[buildableToReach.visitorsBuildablePropertiesScriptableObject.needType] -= buildableToReach.visitorsBuildablePropertiesScriptableObject.needValue;
            visitedBuildables.Add(buildableToReach);
            buildableToReach.isVisitorTargeted = false;
            
            if (actualNeed == NeedType.REST) animator.SetBool(isSitting, false);
            
            SortNeeds();
            
            searchBuildableToUse.enabled = true;
            searchTimer = 1000;

            _navMeshAgent.updateRotation = true;
            touristState = TouristState.SEARCH_NEED;
        }

        private void SortNeeds() {
            sortedNeeds = touristPropertiesScriptableObject.visitorNeeds.OrderByDescending(pair => pair.Value);
        }
        
        private void SynchronizeAnimatorAndAgent() {
            _worldDeltaPosition = _navMeshAgent.nextPosition - _transform.position;

            // Map 'worldDeltaPosition' to local space
            _worldDeltaPositionX = Vector3.Dot(_transform.right, _worldDeltaPosition);
            _worldDeltaPositionY = Vector3.Dot(_transform.forward, _worldDeltaPosition);
            _deltaPosition = new Vector2(_worldDeltaPositionX, _worldDeltaPositionY);

            // Low-pass filter the deltaMove
            _smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, _deltaPosition, _smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f) _velocity = _smoothDeltaPosition / Time.deltaTime;

            _shouldMove = _velocity.magnitude > 0.5f && _navMeshAgent.remainingDistance > _navMeshAgent.radius;
            
            // Update animation parameters
            animator.SetBool(ShouldMove, _shouldMove);
            animator.SetFloat(VelocityX, _velocity.x);
            animator.SetFloat(VelocityZ, _velocity.y);
        }
        
        private void OnAnimatorMove() {
            if (_navMeshAgent != null) {
                transform.position = _navMeshAgent.nextPosition;
            }
        }
    }
}
