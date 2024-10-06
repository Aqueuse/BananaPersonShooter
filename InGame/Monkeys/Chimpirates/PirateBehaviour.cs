using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Monkeys.PhysicToNavMeshCoordinations;
using Save.Helpers;
using Save.Templates;
using Tags;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Monkeys.Chimpirates {
    public class PirateBehaviour : MonkeyMenBehaviour {
        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator animator;
        [SerializeField] private SearchBuildableToBreak searchBuildableToBreak;
        [SerializeField] private float explosionForce;
        
        private Vector3 spaceshipPosition;
        
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
        
        private void Update() {
            if (monkeyMenData.pirateState == PirateState.PLATEFORM_INTERACTION) return;

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME && monkeyMenData.pirateState != PirateState.BREAK_THING) {
                SynchronizeAnimatorAndAgent();
            }

            if (monkeyMenData.pirateState == PirateState.GO_TO_SAS) {
                _navMeshAgent.SetDestination(monkeyMenData.destination);

                if (distanceToDestination < 0.1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.spaceTrafficControlManager.teleportDownTransform.position);
                    GoFindThingToBreak();
                }
            }

            if (monkeyMenData.pirateState == PirateState.SEARCH_THING_TO_BREAK) {
                searchTimer -= 1;
                
                if (searchTimer < 0) {
                    GoBackToSas();
                }

                if (searchBuildableToBreak.hasFoundBuildable) {
                    buildableToBreak = searchBuildableToBreak.buildableFounded;
                    buildableToBreak.isPirateTargeted = true;
                    monkeyMenData.destination = buildableToBreak.ChimpTargetTransform.position;
                    monkeyMenData.pirateState = PirateState.GO_BREAK_THING;

                    searchBuildableToBreak.hasFoundBuildable = false;
                    searchBuildableToBreak.enabled = false;
                }
            }

            if (monkeyMenData.pirateState == PirateState.SEARCH_RANDOM_POINT) {
                var randomPosition = transform.position + Random.insideUnitSphere * 100;

                if (NavMesh.SamplePosition(randomPosition, out var navMeshHit, 2, 1)) {
                    if (Vector3.Distance(monkeyMenData.destination, transform.position) > 10) {
                        path = new NavMeshPath();
                        if (_navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                            if (path.status == NavMeshPathStatus.PathComplete) {
                                monkeyMenData.destination = navMeshHit.position;
                                monkeyMenData.pirateState = PirateState.GO_TO_RANDOM_POINT;
                            }
                        }
                    }
                }
            }

            distanceToDestination = Vector3.Distance(monkeyMenData.destination, transform.position);

            if (monkeyMenData.pirateState == PirateState.GO_BREAK_THING) {
                _navMeshAgent.SetDestination(monkeyMenData.destination);

                if (distanceToDestination < 1f) {
                    animator.SetTrigger(startBreakingAnimatorProperty);
                    buildableToBreak.BreakBuildable();

                    monkeyMenData.pirateState = PirateState.BREAK_THING;

                    var rawMaterialsWithQuantity = ObjectsReference.Instance.meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableToBreak.buildableType].rawMaterialsWithQuantity; 

                    // can't copy directly from itemScriptableObject - Sadge ╯︿╰
                    foreach (var rawMaterial in rawMaterialsWithQuantity) {
                        monkeyMenData.droppedInventory[rawMaterial.Key] = rawMaterial.Value;
                    }
                }
            }

            if (monkeyMenData.pirateState == PirateState.GO_TO_RANDOM_POINT) {
                _navMeshAgent.SetDestination(monkeyMenData.destination);
                
                if (distanceToDestination < 2f) {
                    monkeyMenData.pirateState = PirateState.GO_TO_SAS; 
                    Invoke(nameof(GoBackToSas), 5);
                }
            }

            if (monkeyMenData.pirateState == PirateState.GO_BACK_TO_SAS || monkeyMenData.pirateState == PirateState.FLEE) {
                _navMeshAgent.SetDestination(monkeyMenData.destination);
                
                if (distanceToDestination < 1f) {
                    _navMeshAgent.Warp(ObjectsReference.Instance.spaceTrafficControlManager.teleportUpTransform.position);
                    monkeyMenData.destination = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenData.spaceshipGuid].spawnPoint.position;
                    monkeyMenData.pirateState = PirateState.GO_BACK_TO_SPACESHIP;
                }
            }

            if (monkeyMenData.pirateState == PirateState.GO_BACK_TO_SPACESHIP) {
                if (distanceToDestination < 1f) {
                    ObjectsReference.Instance.spaceshipsSpawner.RemoveGuest();
                    Destroy(gameObject);
                }
            }
        }

        private void GoFindThingToBreak() {
            searchTimer = 1000;
            monkeyMenData.pirateState = PirateState.SEARCH_THING_TO_BREAK;
            searchBuildableToBreak.enabled = true;
        }
        
        // Banana ! Σ(っ °Д °;)っ
        public void Flee() {
            if (monkeyMenData.pirateState == PirateState.FLEE) return;

            animator.SetBool(isInAirAnimatorProperty, false);
            
            DropPartOfInventory();

            animator.SetLayerWeight(1, 1);

            monkeyMenData.destination = ObjectsReference.Instance.chimpManager.sasTransform.position;
            monkeyMenData.pirateState = PirateState.FLEE;
        }

        private void DropPartOfInventory() {
            var distanceToDropFromCenter = Random.Range(0.1f, 0.5f);
            var explositionCenterPosition = transform.position;
            
            // lâche quelques trucs dans l'inventaire s'il y en a encore
            foreach (var itemInInventory in monkeyMenData.droppedInventory) {
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

            monkeyMenData.droppedInventory.Clear();
        }

        public void GoBackToSas() {
            monkeyMenData.pirateState = PirateState.GO_BACK_TO_SAS;
            monkeyMenData.destination = ObjectsReference.Instance.spaceTrafficControlManager.teleportDownTransform.position;
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

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != 7) return;

            if (other.GetComponent<Tag>().itemScriptableObject.buildableType == BuildableType.BUMPER) {
                other.GetComponent<PlateformBehaviour>().isPirateTargeted = false;
                other.GetComponent<PlateformBehaviour>().Activate(GetComponent<Rigidbody>(), 7000);
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 11) return;

            if (monkeyMenData.pirateState == PirateState.PLATEFORM_INTERACTION) {
                GetComponent<PiratePhysicNavMeshCoordination>().SwitchToNavMeshAgent();
                GoBackToSas();
            }
        }
        
        public override void LoadFromSavedData() {
            if (!monkeyMenData.isInSpaceship) {
                SetColors();
                
                transform.position = monkeyMenData.position;
                transform.rotation = monkeyMenData.rotation;
                
                _navMeshAgent.updatePosition = false;
                _navMeshAgent.updateRotation = true;

                _navMeshAgent.velocity = new Vector3(1, 1, 1);
            }
        }
        
        public override void GenerateSavedData() {
            monkeyMenSavedData = new MonkeyMenSavedData {
                uid = monkeyMenData.uid,
                name = monkeyMenData.monkeyMenName,
                characterType = monkeyMenData.characterType,
                prefabNumber = monkeyMenData.prefabNumber,
                clothColorsPreset = monkeyMenData.clothColorsPreset,
                isInSpaceship = monkeyMenData.isInSpaceship,
                pirateState = monkeyMenData.pirateState,
                destination = JsonHelper.FromVector3ToString(monkeyMenData.destination),
                droppedInventory = monkeyMenData.droppedInventory,
                bitKongQuantity = monkeyMenData.bitKongQuantity,
                spaceshipGuid = monkeyMenData.spaceshipGuid,
                position = JsonHelper.FromVector3ToString(transform.position),
                rotation = JsonHelper.FromQuaternionToString(transform.rotation),
            };
        }
    }
}