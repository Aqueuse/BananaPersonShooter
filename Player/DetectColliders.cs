using UnityEngine;

namespace Player {
    public class DetectColliders : MonoBehaviour {
        private PlayerController _playerController;
        private Rigidbody _playerRigidbody;

        private int _waterLayer;
        
        private void Start() {
            _playerController = ObjectsReference.Instance.playerController;
            _playerRigidbody = _playerController.GetComponent<Rigidbody>();

            _waterLayer = LayerMask.NameToLayer("Water");
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == _waterLayer) {
                _playerController.isInWater = true;
                _playerController.speed = 6f;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.layer == _waterLayer) {
                _playerController.isInWater = false;
            }
        }
    }
}