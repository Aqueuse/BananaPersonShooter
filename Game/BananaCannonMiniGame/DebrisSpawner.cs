using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.BananaCannonMiniGame {
    public class DebrisSpawner : MonoBehaviour {
        [SerializeField] private LayerMask raycastLayerMask;
        [SerializeField] private MeshCollider domeCollider;

        [SerializeField] private Transform _debrisContainer;

        private RaycastHit _raycastHit;

        private Vector2 _randomPositionInCircle;
        private Vector3 _randomPositionInCircleVector3;
        private Vector3 _raycastOrigin;

        private NavMeshTriangulation navMeshTriangulation;

        private List<CharacterType> debrisByCharacterType;

        private void Start() {
            _randomPositionInCircle = new Vector2();
            _randomPositionInCircleVector3 = new Vector3();
            _raycastOrigin = new Vector3();
        }

        public void SpawnNewDebrisOnNavMesh() {
            debrisByCharacterType = ObjectsReference.Instance.mapsManager.currentMap.debrisToSPawnByCharacterType;

            navMeshTriangulation = NavMesh.CalculateTriangulation();

            Debug.Log(navMeshTriangulation.vertices.Length);

            foreach (var characterType in debrisByCharacterType) {
                var vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);

                if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out var navMeshHit, 200f, NavMesh.AllAreas)) {
                    Debug.Log("spawn debris");
                    Debug.Log(vertexIndex);
                    
                    var debris = Instantiate(
                        original: ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.GetRandomDebrisByCharacterType(characterType),
                        position: navMeshHit.position,
                        rotation:Quaternion.FromToRotation(transform.up, navMeshHit.normal),
                        parent: _debrisContainer
                    );
                    
                    ObjectsReference.Instance.mapsManager.currentMap.debrisPositions.Add(debris.transform.position);
                    ObjectsReference.Instance.mapsManager.currentMap.debrisRotations.Add(debris.transform.rotation);
                    ObjectsReference.Instance.mapsManager.currentMap.debrisTypes.Add(characterType);
                }
            }

            domeCollider.enabled = true;
            enabled = false;
            
            ObjectsReference.Instance.mapsManager.currentMap.piratesDebrisToSpawn = 0;
            ObjectsReference.Instance.mapsManager.currentMap.visitorsDebrisToSpawn = 0;
            
            ObjectsReference.Instance.mapsManager.currentMap.debrisToSPawnByCharacterType.Clear();
        }

        public void SpawnNewDebrisOnRaycastHit() {
            debrisByCharacterType = ObjectsReference.Instance.mapsManager.currentMap.debrisToSPawnByCharacterType;
            
            domeCollider.enabled = false;

            foreach (var characterType in debrisByCharacterType) {
                _randomPositionInCircle = Random.insideUnitCircle * 500;
                _randomPositionInCircleVector3.x = _randomPositionInCircle.x;
                _randomPositionInCircleVector3.z = _randomPositionInCircle.y;
                
                _raycastOrigin = _randomPositionInCircleVector3 + transform.position;
                
                if (Physics.Raycast(origin: _raycastOrigin, direction: Vector3.down, hitInfo: out _raycastHit, maxDistance:1000, layerMask: raycastLayerMask)) {
                    var debris = Instantiate(
                        original: ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.GetRandomDebrisByCharacterType(characterType),
                        position: _raycastHit.point,
                        rotation:Quaternion.FromToRotation(transform.up, _raycastHit.normal),
                        parent: _debrisContainer
                    );
                    
                    ObjectsReference.Instance.mapsManager.currentMap.debrisPositions.Add(debris.transform.position);
                    ObjectsReference.Instance.mapsManager.currentMap.debrisRotations.Add(debris.transform.rotation);
                    ObjectsReference.Instance.mapsManager.currentMap.debrisTypes.Add(characterType);
                }
            }

            domeCollider.enabled = true;
            enabled = false;

            ObjectsReference.Instance.mapsManager.currentMap.piratesDebrisToSpawn = 0;
            ObjectsReference.Instance.mapsManager.currentMap.visitorsDebrisToSpawn = 0;
            ObjectsReference.Instance.mapsManager.currentMap.debrisToSPawnByCharacterType.Clear();
        }
    }
}
