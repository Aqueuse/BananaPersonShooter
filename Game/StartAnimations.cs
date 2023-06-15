using Tweaks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game {
    public class StartAnimations : MonoSingleton<StartAnimations> {
        [SerializeField] private GameObject bananaPrefab;
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

        private void Update() {
            if (UnityEngine.Input.GetMouseButtonDown(0) && !ObjectsReference.Instance.uiManager.isOnMenu) {
                var ray = ObjectsReference.Instance.gameManager.cameraMain.ScreenPointToRay(UnityEngine.Input.mousePosition);
                var direction = ray.GetPoint(1) - ray.GetPoint(0);
                var spawnedBanana = Instantiate(bananaPrefab, ray.GetPoint(2), Quaternion.LookRotation(direction));
                spawnedBanana.GetComponent<Rigidbody>().velocity = spawnedBanana.transform.forward * 10f;
            }
            else if(UnityEngine.Input.GetMouseButtonDown(1) && !ObjectsReference.Instance.uiManager.isOnMenu) {
				var ray = ObjectsReference.Instance.gameManager.cameraMain.ScreenPointToRay(UnityEngine.Input.mousePosition);
				var direction = ray.GetPoint(1) - ray.GetPoint(0);
				var spawnedBanana = Instantiate(bananaPrefab, ray.GetPoint(2), Quaternion.LookRotation(direction));
				spawnedBanana.AddComponent<TrailRendererRandom>();
				spawnedBanana.GetComponent<Rigidbody>().velocity = spawnedBanana.transform.forward * 10f;
			}
        }
    }
}
