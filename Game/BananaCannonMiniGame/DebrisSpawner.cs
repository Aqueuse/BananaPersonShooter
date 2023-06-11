using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class DebrisSpawner : MonoBehaviour {
        [SerializeField] private LayerMask raycastLayerMask;
        [SerializeField] private MeshCollider domeCollider;

        private GameObject[] _debrisPrefab;
        private Transform _debrisContainer;

        private RaycastHit _raycastHit;
        private int _debrisQuantity;

        private Vector2 _randomPositionInCircle;
        private Vector3 _randomPositionInCircleVector3;
        private Vector3 _raycastOrigin;

        private void Start() {
            _debrisPrefab = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.debrisPrefab;
            _randomPositionInCircle = new Vector2();
            _randomPositionInCircleVector3 = new Vector3();
            _raycastOrigin = new Vector3();
        }
        
        public void SpawnNewDebrisOnMap(Transform mapDebrisContainer) {
            domeCollider.enabled = false;
            _debrisQuantity = ObjectsReference.Instance.mapsManager.currentMap.debrisToSpawn;
            _debrisContainer = mapDebrisContainer;

            for (var i = 0; i < _debrisQuantity; i++) {
                _randomPositionInCircle = Random.insideUnitCircle * 500;
                _randomPositionInCircleVector3.x = _randomPositionInCircle.x;
                _randomPositionInCircleVector3.z = _randomPositionInCircle.y;
                
                _raycastOrigin = _randomPositionInCircleVector3 + transform.position;
                
                if (Physics.Raycast(origin: _raycastOrigin, direction: Vector3.down,
                        hitInfo: out _raycastHit, maxDistance:1000, layerMask: raycastLayerMask)) {
                    Instantiate(
                        original: _debrisPrefab[Random.Range(0, _debrisPrefab.Length - 1)],
                        position: _raycastHit.point,
                        rotation:Quaternion.FromToRotation(transform.up, _raycastHit.normal),
                        parent: _debrisContainer
                    );
                }
                
                if (i == _debrisQuantity - 1) {
                    domeCollider.enabled = true;
                    enabled = false;
                }
            }
        }
    }
}
