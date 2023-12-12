using Bananas;
using ItemsProperties.Monkeys;
using Tags;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Monkeys {
    public class Monkey : MonoBehaviour {
        public MonkeyPropertiesScriptableObject monkeyPropertiesScriptableObject;
        
        public MonkeySounds monkeySounds;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        
        private bool _isNearPlayer;

        public bool smelledBananasOnBananaMan;
        
        private float bananaValue;
        
        private static readonly int Grab = Animator.StringToHash("GRAB");
        
        private void Start() {
            var navMeshTriangulation = NavMesh.CalculateTriangulation();

            var vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);

            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out var navMeshHit, 2f, 0)) {
                _navMeshAgent.Warp(navMeshHit.position);
            }

            transform.localScale = new Vector3(1, 1, 1);

            InvokeRepeating(nameof(IncreaseHunger), 0, 30);
        }

        public void SearchForBananaManBananas() {
            if (monkeyPropertiesScriptableObject.sasiety < 5 & !smelledBananasOnBananaMan) {
                smelledBananasOnBananaMan = true;
                ObjectsReference.Instance.uiQueuedMessages.AddMessage("the monkey smelled bananas !");
            }
        }
        
        private void Feed(float addedBananaValue) {
            bananaValue = addedBananaValue;
            _navMeshAgent.speed = 0;
            _animator.SetTrigger(Grab);
        }

        public void Eat() {
            if (monkeyPropertiesScriptableObject.sasiety < 10) {
                monkeyPropertiesScriptableObject.sasiety += bananaValue;
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
            if (monkeyPropertiesScriptableObject.sasiety > 0)
                monkeyPropertiesScriptableObject.sasiety -= 1;
            SearchForBananaManBananas();
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
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.BANANA)) return;

            Feed(other.GetComponent<Banana>().bananasDataScriptableObject.sasiety);
            Destroy(other.gameObject);
        }
    }
}