using Bananas;
using Data.Monkeys;
using Game.Steam;
using Tags;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Monkeys {
    public class Monkey : MonoBehaviour {
        public MonkeyDataScriptableObject monkeyDataScriptableObject;
        
        public MonkeySounds monkeySounds;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;

        public MonkeyState monkeyState;
        private bool _isRunningToPlayer;
        private bool _isAttackingPlayer;
        private bool _isCatchingPlayer;

        private bool _isNearPlayer;
        public bool hasGrabbedBanana;

        private float bananaValue;
        
        private static readonly int Grab = Animator.StringToHash("GRAB");
        
        private void Start() {
            var navMeshTriangulation = NavMesh.CalculateTriangulation();
                
            var vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out var navMeshHit, 2f, 0)) {
                _navMeshAgent.Warp(navMeshHit.position);
            }

            transform.localScale = new Vector3(1, 1, 1);
        }

        private void Feed(float addedBananaValue) {
            bananaValue = addedBananaValue;
            _animator.SetTrigger(Grab);
        }

        public void Eat() {
            if (monkeyDataScriptableObject.sasiety < 50) {
                monkeyDataScriptableObject.sasiety += bananaValue;
                ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness(this);
            }

            if (monkeyDataScriptableObject.sasiety >= 50 && ObjectsReference.Instance.steamIntegration.isGameOnSteam) {
                ObjectsReference.Instance.steamIntegration.UnlockAchievement(SteamAchievement.STEAM_ACHIEVEMENT_MONKEY_FEEDED); 
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
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.BANANA)) return;
            
            hasGrabbedBanana = true;

            Feed(other.GetComponent<Banana>().bananasDataScriptableObject.sasiety);
            Destroy(other.gameObject);
        }
    }
}