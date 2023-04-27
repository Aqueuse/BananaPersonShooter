using Enums;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;

namespace Monkeys {
    public class Monkey : MonoBehaviour {
        public UIMonkey associatedUI;
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

        private void Start() {
            sasiety = ObjectsReference.Instance.mapsManager.currentMap.monkeySasiety;
            ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
            associatedUI.SetSliderValue(ObjectsReference.Instance.monkeysManager.colorByMonkeyState[monkeyState]);

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            
            NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
                
            int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out NavMeshHit navMeshHit, 2f, 0)) {
                _navMeshAgent.Warp(navMeshHit.position);
            }

            transform.localScale = new Vector3(1, 1, 1);
        }

        public void Feed(float addedBananaValue) {
            if (sasiety < 50) {
                _navMeshAgent.SetDestination(ObjectsReference.Instance.bananaMan.transform.position);
                sasiety += addedBananaValue;
                ObjectsReference.Instance.mapsManager.currentMap.monkeySasiety = sasiety;
                ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
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
    }
}