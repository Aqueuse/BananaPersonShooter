using InGame.Bananas;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using InGame.Items.ItemsProperties.Monkeys;
using Tags;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Monkeys.Ancestors {
    public class Monkey : MonoBehaviour {
        public MonkeyPropertiesScriptableObject monkeyPropertiesScriptableObject;
        public string monkeyId;
        public float sasietyTimer;

        public MonkeySounds monkeySounds;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;

        private bool _isNearPlayer;

        public bool smelledBananasOnBananaMan;

        private float bananaValue;

        private static readonly int Grab = Animator.StringToHash("GRAB");

        private void OnEnable() {
            monkeyId = monkeyPropertiesScriptableObject.monkeyId;
            sasietyTimer = monkeyPropertiesScriptableObject.sasietyTimer;
            
            if (NavMesh.SamplePosition(monkeyPropertiesScriptableObject.lastPosition, out var navMeshHit, 2f, 0)) {
                _navMeshAgent.Warp(navMeshHit.position);
            }
            
            transform.localScale = new Vector3(1, 1, 1);

            InvokeRepeating(nameof(IncreaseHunger), 0, 30);
        }

        public void SearchForBananaManBananas() {
            smelledBananasOnBananaMan = true;
            ObjectsReference.Instance.uiQueuedMessages.AddMessage("the monkey smelled bananas !");
        }

        private void Feed(float addedBananaValue) {
            bananaValue = addedBananaValue;
            _navMeshAgent.speed = 0;
            _animator.SetTrigger(Grab);
        }

        public void Eat() {
            if (sasietyTimer < 10) {
                sasietyTimer += Mathf.CeilToInt(bananaValue);
            }

            else {
                if (smelledBananasOnBananaMan) {
                    smelledBananasOnBananaMan = false;
                    ObjectsReference.Instance.uiQueuedMessages.AddMessage("Monkey had enough bananas");
                }
            }

            _navMeshAgent.speed = 3.5f;
        }

        private void IncreaseHunger() {
            if (sasietyTimer > 0) {
                sasietyTimer -= 1;
            }
            else {
                SearchForBananaManBananas();
            }
        }

        public void PauseMonkey() {
            _navMeshAgent.speed = 0;
            _animator.speed = 0;
        }

        public void UnpauseMonkey() {
            _navMeshAgent.speed = 3.5f;
            _animator.speed = 1;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out BananaBehaviour bananaBehaviour)) {
                Feed(bananaBehaviour.bananasPropertiesScriptableObject.sasiety);
                Destroy(bananaBehaviour.gameObject);
            }
        }
    }
}