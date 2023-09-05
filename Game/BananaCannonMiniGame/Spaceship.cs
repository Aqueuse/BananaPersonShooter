using Tags;
using UnityEngine;

namespace Game.BananaCannonMiniGame {
    public class Spaceship : MonoBehaviour {
        [SerializeField] private Rigidbody[] debrisRigidbodies;

        private Vector3 _arrivalPosition;

        private bool _isPropulsing;
        private float _propulsionSpeed;
        private float _step;
        private float _distanceToArrival;

        private Transform _spaceshipTransform;
        private Vector3 _spaceshipPosition;

        public bool isExploded;
        
        private SpaceshipDestination _spaceshipDestination = SpaceshipDestination.EXIT;

        private void Start() {
            _spaceshipTransform = transform;
            isExploded = false;
        }

        private void Update() {
            _spaceshipPosition = _spaceshipTransform.position;
            _step = _propulsionSpeed * Time.deltaTime;
            
            _spaceshipPosition = Vector3.MoveTowards(_spaceshipPosition, _arrivalPosition, _step);
            _spaceshipTransform.position = _spaceshipPosition;
            
            _spaceshipTransform.LookAt(_arrivalPosition, Vector3.up);

            _distanceToArrival = Vector3.Distance(_spaceshipPosition, _arrivalPosition);

            if (_distanceToArrival > 0.1f) return;
            
            if (_spaceshipDestination == SpaceshipDestination.EXIT) {
                _arrivalPosition = BananaCannonMiniGameManager.Instance.spaceshipsSpawner.entryTransform.position;
                _spaceshipDestination = SpaceshipDestination.ENTRY;
            }

            else {
                _arrivalPosition = BananaCannonMiniGameManager.Instance.spaceshipsSpawner.exitTransform.position;
                _spaceshipDestination = SpaceshipDestination.EXIT;
            }
        }

        public void InitiatePropulsion(Vector3 destination, float speed) {
            _spaceshipDestination = SpaceshipDestination.EXIT;
            _arrivalPosition = destination;
            _propulsionSpeed = speed;
        }

        private void OnTriggerEnter(Collider other) {
            if (!TagsManager.Instance.HasTag(other.gameObject, GAME_OBJECT_TAG.CANNON_MINI_GAME_PROJECTILE)) return;

            other.GetComponent<Projectile>().Destroy();
            
            isExploded = true;
            BananaCannonMiniGameManager.Instance.DecrementeSpaceshipQuantity();
            
            GetComponent<BoxCollider>().enabled = false;
            
            foreach (var debrisRigidbody in debrisRigidbodies) {
                debrisRigidbody.AddExplosionForce(10f, transform.position, 10f);
                debrisRigidbody.AddTorque(debrisRigidbody.position, ForceMode.Impulse);
            }
        }
    }
}