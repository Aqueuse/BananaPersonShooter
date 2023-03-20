using System.Collections;
using Tweaks;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game {
    public class StartScreen : MonoSingleton<StartScreen> {
        [SerializeField] private GameObject cinematiqueGorilla;
        [SerializeField] private Transform cinematiqueGorillaSpawnPoint;
        [SerializeField] private GameObject bananaPrefab;
        
        // syncrhonize navmeshagent with animator
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
            SpawnCinematiqueMonkey();
        }

        private void Update() {
            if (UnityEngine.Input.GetMouseButtonDown(0) && !UIManager.Instance.isOnMenu) {
                Ray ray = GameManager.Instance.cameraMain.ScreenPointToRay(UnityEngine.Input.mousePosition);
                Vector3 direction = ray.GetPoint(1) - ray.GetPoint(0);
                GameObject spawnedBanana = Instantiate(bananaPrefab, ray.GetPoint(2), Quaternion.LookRotation(direction));
                spawnedBanana.GetComponent<Rigidbody>().velocity = spawnedBanana.transform.forward * 10f;
            }
            else if(UnityEngine.Input.GetMouseButtonDown(1)  && !UIManager.Instance.isOnMenu) {
				Ray ray = GameManager.Instance.cameraMain.ScreenPointToRay(UnityEngine.Input.mousePosition);
				Vector3 direction = ray.GetPoint(1) - ray.GetPoint(0);
				GameObject spawnedBanana = Instantiate(bananaPrefab, ray.GetPoint(2), Quaternion.LookRotation(direction));
				spawnedBanana.AddComponent<TrailRendererRandom>();
				spawnedBanana.GetComponent<Rigidbody>().velocity = spawnedBanana.transform.forward * 10f;
			}
        }
        
        private void SpawnCinematiqueMonkey() {
            NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
                
            int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
            var monkey = Instantiate(cinematiqueGorilla, cinematiqueGorillaSpawnPoint.position, Quaternion.identity);

            if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out NavMeshHit navMeshHit, 2f, 0)) {
                monkey.GetComponent<NavMeshAgent>().Warp(navMeshHit.position);
            }

            monkey.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
