using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InGame.MiniGames.SpaceTrafficControlMiniGame.debris {
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
            debrisByCharacterType = ObjectsReference.Instance.gameData.worldData.debrisToSPawnByCharacterType;

            navMeshTriangulation = NavMesh.CalculateTriangulation();

            foreach (var characterType in debrisByCharacterType) {
                var vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);

                if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out var navMeshHit, 200f, NavMesh.AllAreas)) {
                    Instantiate(
                        original: ObjectsReference.Instance.meshReferenceScriptableObject.GetRandomDebrisByCharacterType(characterType),
                        position: navMeshHit.position,
                        rotation:Quaternion.FromToRotation(transform.up, navMeshHit.normal),
                        parent: _debrisContainer
                    );
                }
            }

            domeCollider.enabled = true;
            enabled = false;
            
            ObjectsReference.Instance.gameData.worldData.piratesDebrisToSpawn = 0;
            ObjectsReference.Instance.gameData.worldData.visitorsDebrisToSpawn = 0;
            
            ObjectsReference.Instance.gameData.worldData.debrisToSPawnByCharacterType.Clear();
        }

        public void SpawnNewDebrisOnRaycastHit() {
            debrisByCharacterType = ObjectsReference.Instance.gameData.worldData.debrisToSPawnByCharacterType;
            
            domeCollider.enabled = false;

            foreach (var characterType in debrisByCharacterType) {
                _randomPositionInCircle = Random.insideUnitCircle * 500;
                _randomPositionInCircleVector3.x = _randomPositionInCircle.x;
                _randomPositionInCircleVector3.z = _randomPositionInCircle.y;
                
                _raycastOrigin = _randomPositionInCircleVector3 + transform.position;
                
                if (Physics.Raycast(origin: _raycastOrigin, direction: Vector3.down, hitInfo: out _raycastHit, maxDistance:1000, layerMask: raycastLayerMask)) {
                    Instantiate(
                        original: ObjectsReference.Instance.meshReferenceScriptableObject.GetRandomDebrisByCharacterType(characterType),
                        position: _raycastHit.point,
                        rotation:Quaternion.FromToRotation(transform.up, _raycastHit.normal),
                        parent: _debrisContainer
                    );
                }
            }

            domeCollider.enabled = true;
            enabled = false;

            ObjectsReference.Instance.gameData.worldData.piratesDebrisToSpawn = 0;
            ObjectsReference.Instance.gameData.worldData.visitorsDebrisToSpawn = 0;
            ObjectsReference.Instance.gameData.worldData.debrisToSPawnByCharacterType.Clear();
        }
    }
}
