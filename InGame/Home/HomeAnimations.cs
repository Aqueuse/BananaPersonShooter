using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.Home {
    public class HomeAnimations : MonoSingleton<HomeAnimations> {
        public NavMeshAgent gorillaNavMeshAgent;

        // synchronize navmeshagent with animator
        private Transform _transform;
        private float _worldDeltaPositionX;
        private float _worldDeltaPositionY;
        private Vector2 _deltaPosition;
        private Vector3 _worldDeltaPosition;
        private bool _shouldMove;
        private float _smooth;

        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        
        private void Start() {
            var navMeshTriangulation = NavMesh.CalculateTriangulation();
            var vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
            
            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out var navMeshHit, 2f, 0)) {
                gorillaNavMeshAgent.Warp(navMeshHit.position);
            }

            gorillaNavMeshAgent.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
