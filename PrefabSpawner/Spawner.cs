using Building;
using Random = UnityEngine.Random;
using UnityEngine;

namespace PrefabSpawner {
    public class Spawner : MonoBehaviour {
        [SerializeField] private GameObject[] prefabs;

        [SerializeField] private GameObject terrain;
        [SerializeField] private GameObject deplacables;

        private Mesh _mesh;
        private int _treesCounter;

        public void SpawnThemAll() {
            if (terrain.GetComponent<MeshCollider>() == null) terrain.AddComponent<MeshCollider>();
            terrain.tag = "vegetationMask";
            transform.position = terrain.transform.position;

            _mesh = terrain.GetComponent<MeshCollider>().sharedMesh;

            foreach (var meshTriangleIndex in _mesh.triangles) {
                var vertex1 = _mesh.vertices[_mesh.triangles[meshTriangleIndex]];
                var vertex2 = _mesh.vertices[_mesh.triangles[meshTriangleIndex + 1]];
                var vertex3 = _mesh.vertices[_mesh.triangles[meshTriangleIndex + 2]];
                
                var random1 = RandomVector3(vertex1, vertex2);
                var random2 = RandomVector3(random1, vertex3);

                var randomPosition = terrain.transform.TransformPoint(random2);
                    
                if (Physics.Raycast(randomPosition, Vector3.down, out RaycastHit raycastHit)) {
                    if (raycastHit.transform.tag.Equals("vegetationMask")) {
                        var prefabIndex = Random.Range(0, prefabs.Length);
                        
                        var spawnedPrefab = Instantiate(prefabs[prefabIndex], raycastHit.point, Quaternion.identity, deplacables.transform);
                        spawnedPrefab.transform.rotation = new Quaternion(0, Random.Range(-1, 1), 0, 1f);

                        if (spawnedPrefab.GetComponent<Debris>() != null) {
                            spawnedPrefab.GetComponent<Debris>().prefabIndex = prefabIndex;
                        }

                        _treesCounter++;
                    }
                }
            }
            Debug.Log("spawned "+_treesCounter);
        }
        
        private Vector3 RandomVector3(Vector3 min, Vector3 max) {
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        public void RemoveThemAll() {
            foreach (Transform prefab in deplacables.transform) {
                DestroyImmediate(prefab.gameObject);
            }

            Debug.Log(deplacables.GetComponentsInChildren<Transform>().Length);
        }
    }
}

