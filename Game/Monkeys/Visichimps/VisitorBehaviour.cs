using Game.CommandRoomPanelControls;
using Gestion.BuildablesBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Monkeys.Visichimps {
    public class VisitorBehaviour : MonoBehaviour {
        public GenericDictionary<NeedType, int> visitorNeeds;

        public BuildableBehaviour[] visitedBuildables;

        public string visitorGuid;
        public string visitorPosition;
        public string visitorRotation;
        public VisitorType visitorType;
        public VisitorState visitorState;
        public int notoriety;

        // APPARENCE
        public float textureRotation;
        public int prefabIndex;
        public string visitorName;
        public string visitorDescription;

        // UI
        public Sprite visitorSnapshot;

        // synchronize navmeshagent with animator
        private NavMeshAgent _navMeshAgent;
        private Animator animator;
        private Transform _transform;

        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int ShouldMove = Animator.StringToHash("shouldMove");

        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

        // IA
        private NavMeshPath path;
        public float distanceToDestination;

        public BuildableBehaviour buildableToReach;
        public Vector3 destination;

        private void Start() {
            animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _transform = transform;
            
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(1, 1, 1);
        }

        private void Update() {
            if (visitorState == VisitorState.IN_WAITING_LINE) return;
            
            SynchronizeAnimatorAndAgent();

            if (visitorState == VisitorState.SEARCH_RANDOM_POINT) {
                var randomPosition = transform.position + Random.insideUnitSphere * 100;

                if (NavMesh.SamplePosition(randomPosition, out var navMeshHit, 2, 1)) {
                    if (Vector3.Distance(destination, transform.position) > 10) {
                        path = new NavMeshPath();
                        if (_navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                            if (path.status == NavMeshPathStatus.PathComplete) {
                                destination = navMeshHit.position;
                                visitorState = VisitorState.GO_RANDOM_POINT;
                            }
                        }
                    }
                }
            }
            
            distanceToDestination = Vector3.Distance(destination, transform.position);
            
            if (visitorState == VisitorState.GO_RANDOM_POINT) {
                _navMeshAgent.SetDestination(destination);

                if (distanceToDestination < 0.5f) {
                    visitorState = VisitorState.GO_BACK_TO_SAS;
                }
            }
            
            if (visitorState == VisitorState.GO_FILL_NEED) {
                _navMeshAgent.SetDestination(destination);

                if (distanceToDestination < 0.5f) {
                    visitorState = VisitorState.FILLING_NEED;
                    animator.SetTrigger(buildableToReach.visitorsBuildablePropertiesScriptableObject.animatorTriggerToSet);
                }
            }
            
            // if need is filled, remove points on visitor needs dictionnary
            // search back for buildables to visit
            ChimpManager.Instance.SetNextVisitorDestination(this);
            
            if (visitorState == VisitorState.GO_BACK_TO_SAS) {
                _navMeshAgent.SetDestination(destination);
                
                if (distanceToDestination < 6f) {
                    CommandRoomControlPanelsManager.Instance.marketingCampaignManager.RemoveGuest();
                    
                    // TODO : HAD NOTORIETY FEEDBACK
                    
                    Destroy(gameObject);
                }
            }
        }

        public void FillNeed() {
            visitorNeeds[buildableToReach.visitorsBuildablePropertiesScriptableObject.needType] -= buildableToReach.visitorsBuildablePropertiesScriptableObject.needValue;
            
            ChimpManager.Instance.SetNextVisitorDestination(this);
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
