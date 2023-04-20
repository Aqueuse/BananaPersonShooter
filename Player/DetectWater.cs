using UnityEngine;

namespace Player {
    public class DetectWater : MonoBehaviour {
        private PlayerController _playerController;
        private Rigidbody _playerRigidbody;

        private void Start() {
            _playerController = ObjectsReference.Instance.bananaMan.GetComponent<PlayerController>();
            _playerRigidbody = _playerController.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other) {
            _playerController.isInWater = true;
            _playerController.speed = 6f;
            _playerRigidbody.maxLinearVelocity = 10;
        }

        private void OnTriggerExit(Collider other) {
            _playerController.isInWater = false;
            _playerRigidbody.maxLinearVelocity = 40;
        }
    }
}