using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsData.Characters;
using InGame.Items.ItemsProperties.Characters;
using InGame.Monkeys.PhysicToNavMeshCoordinations;
using Save.Helpers;
using Save.Templates;
using Tags;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Monkeys.Chimpvisitors {
    public class VisitorBehaviour : MonoBehaviour {
        public MonkeyMenData monkeyMenData;

        [SerializeField] private Transform _transform;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        
        [SerializeField] private OrganicRaycast organicRaycast;
        [SerializeField] private SpriteRenderer headSprite;
        
        [SerializeField] private float explosionForce;

        private static readonly int VelocityX = Animator.StringToHash("XVelocity");
        private static readonly int VelocityZ = Animator.StringToHash("ZVelocity");
        private static readonly int ShouldMove = Animator.StringToHash("shouldMove");

        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        
        [HideInInspector] public SpaceshipBehaviour associatedSpaceshipBehaviour;

        private static readonly int color00 = Shader.PropertyToID("_Color00");
        private static readonly int color01 = Shader.PropertyToID("_Color01");
        private static readonly int color02 = Shader.PropertyToID("_Color02");
        private static readonly int color10 = Shader.PropertyToID("_Color10");
        private static readonly int color11 = Shader.PropertyToID("_Color11");
        private static readonly int color12 = Shader.PropertyToID("_Color12");
        private static readonly int color20 = Shader.PropertyToID("_Color20");
        private static readonly int color21 = Shader.PropertyToID("_Color21");
        private static readonly int color22 = Shader.PropertyToID("_Color22");

        private Color[] colorPreset;
        
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
        
        private VisitorBuildableBehaviour _visitorBuildableBehaviour;
        public bool isFollowingGroup;
        
        private GroupBehaviour _groupBehaviour;
        
        public void Init(GroupBehaviour groupBehaviour, MonkeyMenPropertiesScriptableObject monkeyMenProperties, string associatedSpaceshipGuid, Vector3 spawnPoint) {
            _groupBehaviour = groupBehaviour;
            monkeyMenData.spaceshipGuid = associatedSpaceshipGuid;

            associatedSpaceshipBehaviour =
                ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenData.spaceshipGuid];
            
            SetColors(monkeyMenProperties.colorSets);

            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = true;

            navMeshAgent.velocity = new Vector3(1, 1, 1);

            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(stateInfo.fullPathHash, 0, Random.Range(0,1f));
            
            navMeshAgent.Warp(spawnPoint);
        }

        private void Update() {
            SynchronizeAnimatorAndAgent();
        }

        public void OnNeedDetected(GameObject gameObject) {
            if (!isFollowingGroup) return;
            if (monkeyMenData.isSatisfied) return;
            
            var needLocation = gameObject.transform.GetComponent<VisitorBuildableBehaviour>();

            if (needLocation.need != monkeyMenData.need) return;
            if (needLocation.isOccupied) return;
            
            organicRaycast.enabled = false;
            _groupBehaviour.members.Remove(this);

            isFollowingGroup = false;

            needLocation.enabled = true;
            needLocation.PrepareOccupation(navMeshAgent, monkeyMenData.need == NeedType.PILLAGE);
            SetDestination(gameObject.transform.position);
        }
        
        public void FinishSatisfyNeed() {
            monkeyMenData.isSatisfied = true;
            isFollowingGroup = true;
            
            if (!_groupBehaviour.members.Contains(this)) 
                _groupBehaviour.members.Add(this);
            SetDestination(_groupBehaviour.transform.position);
        }
        
        private void SetDestination(Vector3 destination) {
            navMeshAgent.SetDestination(destination);
        }
        
        public bool HasArrivedToDestination() {
            if (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 2f)
                return false;

            // S'il croit être arrivé mais continue de glisser
            if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude > 0.1f)
                return false;

            return true;
        }

        public bool CheckIsArrivedToSpaceship() {
            if (navMeshAgent.pathPending || navMeshAgent.remainingDistance < 2f) {
                return true;
            }
            
            return false;
        }
        
        // 🍌 Banana ! Σ(っ °Д °;)っ
        public void Flee() {
            animator.SetBool(isInAirAnimatorProperty, false);
            animator.SetLayerWeight(1, 1);
            
            if (monkeyMenData.need == NeedType.PILLAGE) DropPartOfInventory();
            
            monkeyMenData.destination = ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[monkeyMenData.spaceshipGuid].visitorsSpawnPoint.position;
        }

        private void DropPartOfInventory() {
            var distanceToDropFromCenter = Random.Range(0.1f, 0.5f);
            var explositionCenterPosition = transform.position;
            
            // lâche quelques trucs dans l'inventaire s'il y en a encore
            foreach (var itemInInventory in monkeyMenData.rawMaterialsInventory) {
                for (var i = 0; i < itemInInventory.Value; i++) {
                    distanceToDropFromCenter += Random.Range(0.1f, 0.5f);
                    var spawnPosition = explositionCenterPosition + Random.insideUnitSphere * distanceToDropFromCenter;
                    if (spawnPosition.y < 1) spawnPosition.y = 1;

                    itemToDrop =
                        Instantiate(
                            ObjectsReference.Instance.meshReferenceScriptableObject.rawMaterialPrefabByRawMaterialType[itemInInventory.Key],
                            spawnPosition,
                            Quaternion.identity);
                    itemToDrop.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explositionCenterPosition, 10);
                }
            }

            monkeyMenData.rawMaterialsInventory.Clear();
        }

        private void GoBackToSpaceship() {
            monkeyMenData.destination = associatedSpaceshipBehaviour.visitorsSpawnPoint.position;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != 7) return;
            
            if (!other.GetComponent<Tag>()) return;
            if (!other.GetComponent<Tag>().itemScriptableObject) return;

            if (other.GetComponent<Tag>().itemScriptableObject.buildableType == BuildableType.BUMPER) {
                other.GetComponent<BumperBehaviour>().isPirateTargeted = false;
                other.GetComponent<BumperBehaviour>().Activate(GetComponent<Rigidbody>(), 7000);
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 11) return;

            GetComponent<ChimpMenPhysicNavMeshCoordination>().SwitchToNavMeshAgent();
            GoBackToSpaceship();
        }
        
        private void OnAnimatorMove() {
            if (navMeshAgent != null) {
                transform.position = navMeshAgent.nextPosition;
            }
        }

        private void SynchronizeAnimatorAndAgent() {
            _worldDeltaPosition = navMeshAgent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            _worldDeltaPositionX = Vector3.Dot(transform.right, _worldDeltaPosition);
            _worldDeltaPositionY = Vector3.Dot(transform.forward, _worldDeltaPosition);
            _deltaPosition = new Vector2(_worldDeltaPositionX, _worldDeltaPositionY);

            // Low-pass filter the deltaMove
            _smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, _deltaPosition, _smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f) _velocity = _smoothDeltaPosition / Time.deltaTime;

            _shouldMove = _velocity.magnitude > 0.5f && navMeshAgent.remainingDistance > navMeshAgent.radius;

            // Update animation parameters
            animator.SetBool(ShouldMove, _shouldMove);
            animator.SetFloat(VelocityX, _velocity.x);
            animator.SetFloat(VelocityZ, _velocity.y);
        }
        
        private void SetColors(Color[] colorsSet) {
            var monkeyMenMaterial = meshRenderer.material;
            
            monkeyMenMaterial.SetColor(color00, colorsSet[0]);
            monkeyMenMaterial.SetColor(color01, colorsSet[1]);
            monkeyMenMaterial.SetColor(color02, colorsSet[2]);
            monkeyMenMaterial.SetColor(color10, colorsSet[3]);
            monkeyMenMaterial.SetColor(color11, colorsSet[4]);
            monkeyMenMaterial.SetColor(color12, colorsSet[5]);
            monkeyMenMaterial.SetColor(color20, colorsSet[6]);
            monkeyMenMaterial.SetColor(color21, colorsSet[7]);
            monkeyMenMaterial.SetColor(color22, colorsSet[8]);

            meshRenderer.material = monkeyMenMaterial;
        }

        public void LoadSavedData(MonkeyMenSavedData monkeyMenSavedData) {
            monkeyMenData.colorsSet = monkeyMenSavedData.colorsSet;
            monkeyMenData.characterType = monkeyMenSavedData.characterType;
            monkeyMenData.bitKongQuantity = monkeyMenSavedData.bitKongQuantity;
            monkeyMenData.destination = JsonHelper.FromStringToVector3(monkeyMenSavedData.destination);
            monkeyMenData.isSatisfied = monkeyMenSavedData.isSatisfied;

            monkeyMenData.ingredientsInventory = monkeyMenSavedData.ingredientsInventory;
            monkeyMenData.manufacturedItemsInventory = monkeyMenSavedData.manufacturedItemsInventory;
            monkeyMenData.rawMaterialsInventory = monkeyMenSavedData.rawMaterialsInventory;

            SetColors(monkeyMenSavedData.colorsSet);

            associatedSpaceshipBehaviour =
                ObjectsReference.Instance.spaceTrafficControlManager.spaceshipBehavioursByGuid[
                    monkeyMenData.spaceshipGuid];
        }
        
        public MonkeyMenSavedData GenerateSavedData() {
            return new MonkeyMenSavedData {
                characterType = monkeyMenData.characterType,
                ingredientsInventory = monkeyMenData.ingredientsInventory,
                manufacturedItemsInventory = monkeyMenData.manufacturedItemsInventory,
                rawMaterialsInventory = monkeyMenData.rawMaterialsInventory,
                bitKongQuantity = monkeyMenData.bitKongQuantity,
                uid = monkeyMenData.uid,
                name = monkeyMenData.monkeyMenName,
                prefabIndex = monkeyMenData.prefabIndex,
                colorsSet = monkeyMenData.colorsSet,
                destination = JsonHelper.FromVector3ToString(monkeyMenData.destination),
                spaceshipGuid = monkeyMenData.spaceshipGuid,
                position = JsonHelper.FromVector3ToString(transform.position),
                rotation = JsonHelper.FromQuaternionToString(transform.rotation),
                need = monkeyMenData.need,
                isSatisfied = monkeyMenData.isSatisfied
            };
        }
    }
}
