using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

namespace PrefabSpawner {
    public class Spawner : MonoBehaviour {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private GameObject prefabParent;
        [SerializeField] private GameObject terrain;

        private Dictionary<GameObject, Vector3> _treesPosition;

        private Mesh _mesh;
        
        public void SpawnThemAll() {
            _mesh = terrain.GetComponent<MeshCollider>().sharedMesh;

            _treesPosition = new Dictionary<GameObject, Vector3>();
            
            foreach (var meshTriangleIndex in _mesh.triangles) {
                var vertex1 = _mesh.vertices[_mesh.triangles[meshTriangleIndex]];
                var vertex2 = _mesh.vertices[_mesh.triangles[meshTriangleIndex + 1]];
                var vertex3 = _mesh.vertices[_mesh.triangles[meshTriangleIndex + 2]];
            
                var random1 = RandomVector3(vertex1, vertex2);
                var random2 = RandomVector3(random1, vertex3);

                var randomPosition = transform.TransformPoint(random2);
                
                if (Physics.Raycast(randomPosition+Vector3.up*10, Vector3.down, out RaycastHit raycastHit)) {
                    if (raycastHit.transform.tag.Equals("vegetationMask")) {
                        var spawnedPrefab = Instantiate(prefabs[Random.Range(0, prefabs.Length)], raycastHit.point, Quaternion.identity, prefabParent.transform);
                        spawnedPrefab.transform.rotation = new Quaternion(0, Random.Range(-1, 1), 0, 0);
                        _treesPosition.Add(spawnedPrefab, raycastHit.point);
                    }
                }
            }
            Debug.Log("spawned "+_treesPosition.Count+ " trees");
        }
        
        private Vector3 RandomVector3(Vector3 min, Vector3 max) {
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }
        

        public void RemoveThemAll() {
            foreach (Transform prefab in prefabParent.transform) {
                DestroyImmediate(prefab.gameObject);
            }
        }
    }
}

