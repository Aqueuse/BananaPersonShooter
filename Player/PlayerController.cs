using Game;
using Input;
using UI.InGame;
using UnityEngine;
using HoaxGames;

namespace Player {
    public class PlayerController : MonoBehaviour {
        private CharacterController _characterController;
        private TpsPlayerAnimator _tpsPlayerAnimatorScript;
        private FootIK _footIKScript;

        public float baseMovementSpeed = 6f;
        public float sprintMovementSpeed;
        private const float Gravity = -30f;
        private const float TerminalVelocity = 100f;
        private const float JumpHeight = 7f;

        public bool isFocusCamera;
        public bool isRolling;
        private bool _isJumping;
        public bool canMove = true;
        
        //Stored Values
        private Vector3 _rawInputMovement;
        public Transform mainCameraTransform;

        // store current jump velocity 
        private float _verticalVelocity;

        private Vector3 _movement;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;
        private Quaternion _cameraRotation;

        private float _inputAngle;
        private Vector3 _playerPosition;
        
        private void Start() {
            _tpsPlayerAnimatorScript = gameObject.GetComponent<TpsPlayerAnimator>();
            _footIKScript = gameObject.GetComponent<FootIK>();
            _characterController = GetComponent<CharacterController>();

            if (Camera.main != null) mainCameraTransform = Camera.main.transform;

            _verticalVelocity = 0.0f;
            baseMovementSpeed = 6f;
        }

        private void FixedUpdate() {
            if (GameManager.Instance.isGamePlaying && canMove) {
                // ugly but opti
                _cameraRotation.x = 0;
                _cameraRotation.y = mainCameraTransform.transform.rotation.y;
                _cameraRotation.z = 0;
                _cameraRotation.w = mainCameraTransform.rotation.w;
                _cameraRotation = _cameraRotation.normalized;
                
                _rawInputMovement.x = GameActions.Instance.move.x;
                _rawInputMovement.y = 0;
                _rawInputMovement.z = GameActions.Instance.move.y; // Y en input => Z pour le player (forward)
                
                _rawInputMovement = Vector3.ClampMagnitude(_rawInputMovement, 1); // clamp the speed in diagonal

                _inputAngle = Vector2.SignedAngle(Vector2.up, new Vector2(-_rawInputMovement.x, _rawInputMovement.z));

                _playerPosition = transform.position;

                if (!isFocusCamera && !BananaMan.Instance.isGrabingBananaGun) {
                    // rotate follow the input
                    if (_rawInputMovement != Vector3.zero) {
                        transform.rotation = Quaternion.AngleAxis(_inputAngle, Vector3.up) * _cameraRotation;
                    }
                }
                else {
                    // rotate strictly follow the camera 
                    transform.rotation = _cameraRotation;
                }

                // check if jumping
                _isJumping = _verticalVelocity > 0;

                // compute controller movement
                _movement = _cameraRotation * _rawInputMovement * (baseMovementSpeed * Time.fixedDeltaTime);

                // integrate gravity, caping terminal fall velocity at 10m/s
                _verticalVelocity = Mathf.Clamp(_verticalVelocity + Gravity * Time.fixedDeltaTime, -TerminalVelocity,
                    TerminalVelocity);
                // add vertical velocity to movement
                // We offset by the Gravity * Time.fixedDeltaTime to ensure we don't "fall" when walking down stairs
                _movement += Vector3.up * (_verticalVelocity * Time.fixedDeltaTime);

                // check if on ground BEFORE the _movement to apply a greater downward force
                // to avoid falling when walking down stairs
                if (_footIKScript.getGroundedResult().isGrounded && !_isJumping) {
                    _movement += Vector3.up * (Gravity * Time.fixedDeltaTime);

                    _verticalVelocity = 0f;
                    BananaMan.Instance.isInAir = false;
                }

                // apply movement to controller
                _characterController.Move(_movement);

                // check if we stopped falling (i.e. hit the ground "when going down")
                // TODO: for now this check only allows us to reset vertical velocity when we hit the ground
                // we will also need to check for collision with the ceiling to stop "going up" when we hit it
                if (_footIKScript.getGroundedResult().isGrounded && !_isJumping) {
                }

                _tpsPlayerAnimatorScript.UpdateMovementAnimation(_rawInputMovement.z * baseMovementSpeed,
                    _rawInputMovement.x * baseMovementSpeed);

                // is the player moving ?
                _currentPosition = _playerPosition;
                _tpsPlayerAnimatorScript.IsMoving(_currentPosition != _lastPosition);
                _lastPosition = _currentPosition;

                UIFace.Instance.MoveFaceAnimation(_rawInputMovement.magnitude);
            }
        }

        public void PlayerJump() {
            if (!BananaMan.Instance.isInAir && !isRolling) {
                if (!BananaMan.Instance.isRagdoll && !isFocusCamera) {
                    // we physically jump by setting our vertical velocity
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    BananaMan.Instance.isInAir = true;
                    _tpsPlayerAnimatorScript.Jump();
                    _isJumping = true;
                }
                else {
                    if (_rawInputMovement.z >= 0) {
                        // we physically jump by setting our vertical velocity
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                        BananaMan.Instance.isInAir = true;
                        _tpsPlayerAnimatorScript.Jump();
                        _isJumping = true;
                    }
                }
            }
        }

        public void PlayerRoll() {
            if (!BananaMan.Instance.isInAir) {
                _tpsPlayerAnimatorScript.Roll();
                GetComponent<CharacterController>().height = 1;
                GetComponent<CharacterController>().center = new Vector3(0, 0.44f, 0);

                GetComponent<CapsuleCollider>().height = 0.90f;
            }
        }

        public void PlayerSprint() {
            if (!BananaMan.Instance.isInAir && !isRolling) {
                baseMovementSpeed = sprintMovementSpeed;
            }
        }

        public void PlayerStopSprint() {
            baseMovementSpeed = 6f;
        }
        
        public void ResetPlayer() {
            baseMovementSpeed = 6f;
            _tpsPlayerAnimatorScript.UpdateMovementAnimation(0, 0);
        }
    }
}