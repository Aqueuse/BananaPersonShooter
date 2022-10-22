using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerController : MonoBehaviour {
        [Header("Movement Settings")] 
        public float movementSpeed = 3f;

        private Rigidbody _playerRigidbody;
        private TpsPlayerAnimator _tpsPlayerAnimator;
        private Transform _playerTransform;

        //Stored Values
        private Vector3 _rawInputMovement;
        private Transform _mainCameraTransform;
        
        private void Start() {
            _playerRigidbody = GetComponent<Rigidbody>();
            _tpsPlayerAnimator = GetComponentInChildren<TpsPlayerAnimator>();
            _playerTransform = GetComponent<Transform>();

            if (Camera.main != null) _mainCameraTransform = Camera.main.transform;
        }

        private void FixedUpdate() {
            if (_rawInputMovement != Vector3.zero || BananaMan.Instance.isArmed) {
                transform.rotation = new Quaternion(0, _mainCameraTransform.transform.rotation.y, 0, _mainCameraTransform.rotation.w);
            }

            Move();
            _tpsPlayerAnimator.UpdateMovementAnimation(_rawInputMovement.z * movementSpeed, _rawInputMovement.x * movementSpeed);
        }
        
        private void Move() {
            _playerRigidbody.MovePosition(_playerTransform.position + _playerTransform.TransformDirection(_rawInputMovement) * (movementSpeed * Time.deltaTime));
        }

        public void PlayerJump(InputAction.CallbackContext value) {
            if (value.performed && _rawInputMovement.z >= 0) {
                _tpsPlayerAnimator.Jump();
            }
        }

        public void PlayerMovement(InputAction.CallbackContext value) {
            Vector2 inputMovement = value.ReadValue<Vector2>();
            _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y); // Y en input => Z pour le player (forward)
        }
        
        public void PlayerSprint(InputAction.CallbackContext value) {
            movementSpeed = 6f;

            if (value.canceled) {
                movementSpeed = 3f;
            }
        }
    }
}
