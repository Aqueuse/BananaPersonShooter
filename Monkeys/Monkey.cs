using Bananas;
using Game.Steam;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Monkeys {
    public class Monkey : MonoBehaviour {
        public UIMonkey associatedUI;
        public MonkeySounds monkeySounds;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private float _maxHappiness;
        public float happiness;
        public MonkeyState monkeyState;

        public float sasiety;

        private bool _isRunningToPlayer;
        private bool _isAttackingPlayer;
        private bool _isCatchingPlayer;

        private bool _isNearPlayer;
        public bool hasGrabbedBanana;

        private static readonly int Grab = Animator.StringToHash("GRAB");
        
        private void Start() {
            sasiety = ObjectsReference.Instance.mapsManager.currentMap.monkeySasiety;
            ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
            
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            monkeySounds = GetComponentInChildren<MonkeySounds>();
            
            var navMeshTriangulation = NavMesh.CalculateTriangulation();
                
            var vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out var navMeshHit, 2f, 0)) {
                _navMeshAgent.Warp(navMeshHit.position);
            }

            transform.localScale = new Vector3(1, 1, 1);
        }

        private void Feed(float addedBananaValue) {
            _animator.SetTrigger(Grab);

            if (sasiety < 50) {
                sasiety += addedBananaValue;
                ObjectsReference.Instance.mapsManager.currentMap.monkeySasiety = sasiety;
                ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
            }

            if (sasiety >= 50 && ObjectsReference.Instance.steamIntegration.isGameOnSteam) {
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
            if (!other.CompareTag("Banana")) return;
            
            hasGrabbedBanana = true;

            Feed(other.GetComponent<Banana>().bananasDataScriptableObject.sasiety);
            Destroy(other.gameObject);
        }
    }
}