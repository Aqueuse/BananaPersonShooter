using System.Collections.Generic;
using System.Linq;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsData;
using InGame.Monkeys.Chimpirates;
using Save.Helpers;
using Save.Templates;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys {
    public class MonkeyMenBehaviour : MonoBehaviour {
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        public SearchBuildableToBreak searchBuildableToBreak;

        public MonkeyMenSavedData monkeyMenSavedData;
        public MonkeyMenData monkeyMenData;
        public IOrderedEnumerable<KeyValuePair<NeedType, int>> sortedNeeds;

        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator animator;

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

        public Vector3 destination;

        public SpaceshipBehaviour associatedSpaceship;
        private Vector3 spaceshipPosition;

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

        private void OnAnimatorMove() {
            if (_navMeshAgent != null) {
                transform.position = _navMeshAgent.nextPosition;
            }
        }

        public void SynchronizeAnimatorAndAgent() {
            _worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            _worldDeltaPositionX = Vector3.Dot(transform.right, _worldDeltaPosition);
            _worldDeltaPositionY = Vector3.Dot(transform.forward, _worldDeltaPosition);
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
        
        private void SetColors() {
            var monkeyMenMaterial = meshRenderer.material;

            colorPreset = ObjectsReference.Instance.meshReferenceScriptableObject.chimpMensAppearanceScriptableObjects[monkeyMenData.appearanceScriptableObjectIndex].colorSets;
            
            monkeyMenMaterial.SetColor(color00, colorPreset[0]);
            monkeyMenMaterial.SetColor(color01, colorPreset[1]);
            monkeyMenMaterial.SetColor(color02, colorPreset[2]);
            monkeyMenMaterial.SetColor(color10, colorPreset[3]);
            monkeyMenMaterial.SetColor(color11, colorPreset[4]);
            monkeyMenMaterial.SetColor(color12, colorPreset[5]);
            monkeyMenMaterial.SetColor(color20, colorPreset[6]);
            monkeyMenMaterial.SetColor(color21, colorPreset[7]);
            monkeyMenMaterial.SetColor(color22, colorPreset[8]);

            meshRenderer.material = monkeyMenMaterial;
        }
        
        public void GenerateSavedData() {
            var sortedNeedsArray = monkeyMenData.needs.AsEnumerable().ToArray();

            var sortedNeedsDictionnary = new Dictionary<NeedType, int>(); 

            foreach (var need in sortedNeedsArray) {
                sortedNeedsDictionnary.Add(need.Key, need.Value);
            }

            monkeyMenSavedData = new MonkeyMenSavedData {
                characterType = CharacterType.MERCHIMP,
                pirateState = monkeyMenData.pirateState,
                ingredientsInventory = monkeyMenData.ingredientsInventory,
                bananasInventory = monkeyMenData.bananasInventory,
                manufacturedItemsInventory = monkeyMenData.manufacturedItemsInventory,
                droppedInventory = monkeyMenData.droppedInventory,
                bitKongQuantity = monkeyMenData.bitKongQuantity,
                uid = monkeyMenData.uid,
                name = monkeyMenData.monkeyMenName,
                appearanceScriptableObjectIndex = monkeyMenData.appearanceScriptableObjectIndex,
                isInSpaceship = monkeyMenData.isInSpaceship,
                touristState = monkeyMenData.touristState,
                destination = JsonHelper.FromVector3ToString(monkeyMenData.destination),
                spaceshipGuid = monkeyMenData.spaceshipGuid,
                position = JsonHelper.FromVector3ToString(transform.position),
                rotation = JsonHelper.FromQuaternionToString(transform.rotation),
                needs = sortedNeedsDictionnary
            };
        }

        public void LoadFromSavedData() {
            if (monkeyMenData.characterType != CharacterType.MERCHIMP) SetColors();
            
            if (monkeyMenData.isInSpaceship) return;
            
            transform.position = monkeyMenData.position;
            transform.rotation = monkeyMenData.rotation;
            
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            _navMeshAgent.velocity = new Vector3(1, 1, 1);

            destination = monkeyMenData.destination;

            if (monkeyMenData.characterType == CharacterType.PIRATE) {
                gameObject.AddComponent<PirateBehaviour>();
            }
        }
    }
}

