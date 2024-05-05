using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsData;
using Save.Helpers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Monkeys.Chimpirates {
    public class PirateBehaviour : MonoBehaviour {
        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator animator;
        [SerializeField] private SearchBuildableToBreak searchBuildableToBreak;
        [SerializeField] private float explositionForce;

        public PirateData pirateSavedData;

        public PirateState pirateState = PirateState.GO_TO_SAS;
        public Vector3 destination;
        
        public Dictionary<RawMaterialType, int> pirateInventory = new() {
            {RawMaterialType.ELECTRONIC, 0},
            {RawMaterialType.BANANA_PEEL, 0},
            {RawMaterialType.METAL, 0},
            {RawMaterialType.FABRIC, 0},
            {RawMaterialType.BATTERY, 0}
        };

        // synchronize navmeshagent with animator
        private static readonly int VelocityX = Animator.StringToHash("XVelocity");
        private static readonly int VelocityZ = Animator.StringToHash("ZVelocity");
        private static readonly int ShouldMove = Animator.StringToHash("shouldMove");
        private static readonly int startBreakingAnimatorProperty = Animator.StringToHash("break");
        private static readonly int isInAirAnimatorProperty = Animator.StringToHash("isInAir");

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
        
        private BuildableBehaviour buildableToBreak;
        
        // IA
        public float distanceToDestination;
        private NavMeshPath path;

        public float searchTimer;
        private RaycastHit raycastHit;
        private Vector3 rotatingAxis;

        private GameObject itemToDrop;

        private void Start() {
            destination = ObjectsReference.Instance.spaceTrafficControlManager.teleportUpTransform.position;
            
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(1, 1, 1);
        }

        private void Update() {
            if (ObjectsReference.Instance.gameManager.isGamePlaying && pirateState != PirateState.BREAK_THING && pirateState != PirateState.PLATEFORM_INTERACTION) {
                SynchronizeAnimatorAndAgent();
            }

            if (pirateState == PirateState.GO_TO_SAS) {
                _navMeshAgent.SetDestination(destination);

                if (distanceToDestination < 0.1f) {
                    Debug.Log("tp up");
                    _navMeshAgent.Warp(ObjectsReference.Instance.spaceTrafficControlManager.teleportDownTransform.position);
                    GoFindThingToBreak();
                }
            }

            if (pirateState == PirateState.SEARCH_THING_TO_BREAK) {
                searchTimer -= 1;
                
                if (searchTimer < 0) {
                    GoBackToSas();
                }

                if (searchBuildableToBreak.hasFoundBuildable) {
                    buildableToBreak = searchBuildableToBreak.buildableFounded;
                    buildableToBreak.isPirateTargeted = true;
                    destination = buildableToBreak.ChimpTargetTransform.position;
                    pirateState = PirateState.GO_BREAK_THING;

                    searchBuildableToBreak.hasFoundBuildable = false;
                    searchBuildableToBreak.enabled = false;
                }
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

                if (distanceToDestination < 1f) {
                    animator.SetTrigger(startBreakingAnimatorProperty);
                    buildableToBreak.BreakBuildable();

                    pirateState = PirateState.BREAK_THING;

                    var rawMaterialsWithQuantity = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableToBreak.buildableType].rawMaterialsWithQuantity; 

                    // can't copy directly from itemScriptableObject - Sadge ╯︿╰
                    foreach (var rawMaterial in rawMaterialsWithQuantity) {
                        pirateInventory.Add(rawMaterial.Key, rawMaterial.Value);
                    }
                }
            }

            if (pirateState == PirateState.GO_TO_RANDOM_POINT) {
                _navMeshAgent.SetDestination(destination);
                
                if (distanceToDestination < 2f) {
                    pirateState = PirateState.GO_TO_SAS; 
                    Invoke(nameof(GoBackToSas), 5);
                }
            }

            if (pirateState == PirateState.GO_BACK_TO_SAS || pirateState == PirateState.FLEE) {
                _navMeshAgent.SetDestination(destination);
                
                if (distanceToDestination < 6f) {
                    ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();
                    
                    Destroy(gameObject);
                }
            }
        }

        private void GoFindThingToBreak() {
            searchTimer = 1000;
            pirateState = PirateState.SEARCH_THING_TO_BREAK;
            searchBuildableToBreak.enabled = true;
        }

        public void Fly() {
            animator.SetFloat(VelocityX, 0);
            animator.SetFloat(VelocityZ, 0);
            animator.SetBool(isInAirAnimatorProperty, true);
        }

        // Banana ! Σ(っ °Д °;)っ
        public void Flee() {
            if (pirateState == PirateState.FLEE) return;

            animator.SetBool(isInAirAnimatorProperty, false);
            
            DropPartOfInventory();

            animator.SetLayerWeight(1, 1);

            destination = ObjectsReference.Instance.chimpManager.sasTransform.position;
            pirateState = PirateState.FLEE;
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

        public void GoToSas() {
            destination = ObjectsReference.Instance.spaceTrafficControlManager.teleportUpTransform.position;
        }

        public void GoBackToSas() {
            pirateState = PirateState.GO_BACK_TO_SAS;
            destination = ObjectsReference.Instance.spaceTrafficControlManager.teleportDownTransform.position;
        }

        public void GenerateSavedData() {
            pirateSavedData = new PirateData {
                piratePosition = JsonHelper.FromVector3ToString(transform.position),
                pirateRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                pirateState = pirateState,
                destination = JsonHelper.FromVector3ToString(destination),
                piratesInventory = pirateInventory
            };
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