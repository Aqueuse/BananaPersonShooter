using UI.InGame;
using UI.InGame.Chimployee;
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
            
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            
            var navMeshTriangulation = NavMesh.CalculateTriangulation();
                
            var vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out var navMeshHit, 2f, 0)) {
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

            if (sasiety >=30 && !ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.FEED_MONKEY)) {
                ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.FEED_MONKEY);
                ObjectsReference.Instance.uiChimployee.InitDialogue(ChimployeeDialogue.chimployee_second_interaction);
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