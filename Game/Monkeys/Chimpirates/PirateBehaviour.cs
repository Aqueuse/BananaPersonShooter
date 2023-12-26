using Game.CommandRoomPanelControls;
using Gestion.BuildablesBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Monkeys.Chimpirates {
    public class PirateBehaviour : MonoBehaviour {
        [SerializeField] private float explositionForce;
        
        public BuildableBehaviour buildableToBreak;
        public PirateState pirateState;
        public Vector3 destination;
        
        private static readonly int startBreakingAnimatorProperty = Animator.StringToHash("BREAK");

        // synchronize navmeshagent with animator
        private NavMeshAgent _navMeshAgent;
        private Animator animator;
        
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int ShouldMove = Animator.StringToHash("shouldMove");
        
        private Transform _transform;
        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

        private Vector3 randomDirection;
        private Vector3 finalPosition;

        // IA
        public float distanceToDestination;
        private NavMeshPath path;

        public GenericDictionary<RawMaterialType, int> pirateInventory = new();
        private GameObject itemToDrop;
        
        private void Start() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            _transform = transform;
            
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(1, 1, 1);

            ChimpManager.Instance.SetPirateDestination(_navMeshAgent);
        }

        private void Update() {
            if (ObjectsReference.Instance.gameManager.isGamePlaying) {
                SynchronizeAnimatorAndAgent();
            }

            if (pirateState == PirateState.SEARCH_RANDOM_POINT) {
                var randomPosition = transform.position + Random.insideUnitSphere * 100;

                if (NavMesh.SamplePosition(randomPosition, out var navMeshHit, 2, 1)) {
                    if (Vector3.Distance(destination, transform.position) > 10) {
                        path = new NavMeshPath();
                        if (_navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                            if (path.status == NavMeshPathStatus.PathComplete) {
                                destination = navMeshHit.position;
                                pirateState = PirateState.GO_TO_RANDOM_POINT;
                            }
                        }
                    }
                }
            }

            distanceToDestination = Vector3.Distance(destination, transform.position);

            if (pirateState == PirateState.GO_BREAK_THING) {
                _navMeshAgent.SetDestination(destination);

                if (distanceToDestination < 0.5f) {
                    pirateState = PirateState.BREAK_THING;

                    var rawMaterialsWithQuantity = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableToBreak.buildableType].rawMaterialsWithQuantity; 

                    // can't copy directly from itemScriptableObject - Sadge
                    foreach (var rawMaterial in rawMaterialsWithQuantity) {
                        pirateInventory.Add(rawMaterial.Key, rawMaterial.Value);
                    }

                    buildableToBreak.BreakBuildable();

                    animator.SetTrigger(startBreakingAnimatorProperty);
                    Invoke(nameof(GoBackToSas), 5);
                }
            }

            if (pirateState == PirateState.GO_TO_RANDOM_POINT) {
                _navMeshAgent.SetDestination(destination);
                
                if (distanceToDestination < 2f) {
                    Invoke(nameof(GoBackToSas), 5);
                }
            }

            if (pirateState == PirateState.GO_BACK_TO_SAS || pirateState == PirateState.FLEE) {
                _navMeshAgent.SetDestination(destination);
                
                if (distanceToDestination < 6f) {
                    CommandRoomControlPanelsManager.Instance.marketingCampaignManager.RemoveGuest();
                    
                    Destroy(gameObject);
                }
            }
        }

        // Banana ! Σ(っ °Д °;)っ
        public void Flee() {
            DropPartOfInventory();

            if (pirateState == PirateState.FLEE) return;
            
            pirateState = PirateState.FLEE;
            destination = ChimpManager.Instance.sasTransform.position;
            
            animator.SetLayerWeight(0, 0);
            animator.SetLayerWeight(1, 1);
        }

        private void DropPartOfInventory() {
            var distanceToDropFromCenter = Random.Range(0.1f, 0.5f);
            var explositionCenterPosition = transform.position;
            
            // lâche quelques trucs dans l'inventaire s'il y en a encore
            foreach (var itemInInventory in pirateInventory) {
                for (int i = 0; i < itemInInventory.Value; i++) {
                    distanceToDropFromCenter += Random.Range(0.1f, 0.5f);
                    var spawnPosition = explositionCenterPosition + Random.insideUnitSphere * distanceToDropFromCenter;
                    if (spawnPosition.y < 1) spawnPosition.y = 1;

                    itemToDrop =
                        Instantiate(
                            ObjectsReference.Instance.meshReferenceScriptableObject.prefabByRawMaterialType[itemInInventory.Key],
                            spawnPosition,
                            Quaternion.identity);
                    itemToDrop.GetComponent<Rigidbody>().AddExplosionForce(explositionForce, explositionCenterPosition, 10);
                }
            }
            
            pirateInventory.Clear();
        }

        private void GoBackToSas() {
            pirateState = PirateState.GO_BACK_TO_SAS;
            destination = ChimpManager.Instance.sasTransform.position;
        }
        
        private void OnAnimatorMove() {
            if (_navMeshAgent != null) {
                transform.position = _navMeshAgent.nextPosition;
            }
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
    }
}