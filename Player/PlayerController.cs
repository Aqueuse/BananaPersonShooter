using Game;
using Input;
using UI.InGame;
using UnityEngine;

namespace Player {

    public class PlayerController : MonoBehaviour {
        private CharacterController _characterController;
        
        public float baseMovementSpeed = 6f;
        public float sprintMovementSpeed;
        private const float Gravity = -15f;
        private const float JumpHeight = 1f;

        public bool isFocusCamera;
        public bool isRolling;
        private bool isJumping;
        
        public bool canMove = true;
        
        private TpsPlayerAnimator _tpsPlayerAnimatorScript;
        
        //Stored Values
        private Vector3 _rawInputMovement;
        public Transform mainCameraTransform;
        private Vector3 _movement;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;
        private Quaternion cameraRotation;

        private float inputAngle;
        private Vector3 playerPosition;        

        private float jumpCounter;
        
        private void Start() {
            _tpsPlayerAnimatorScript = gameObject.GetComponent<TpsPlayerAnimator>();
            _characterController = GetComponent<CharacterController>();

            jumpCounter = 0f;

            if (Camera.main != null) mainCameraTransform = Camera.main.transform;
        }
        
        private void Update() {
            if (GameManager.Instance.isGamePlaying && canMove) {
                cameraRotation = new Quaternion(0, mainCameraTransform.transform.rotation.y, 0, mainCameraTransform.rotation.w).normalized;
                
                _rawInputMovement = new Vector3(GameActions.Instance.move.x, 0,GameActions.Instance.move.y); // Y en input => Z pour le player (forward)
                _rawInputMovement = Vector3.ClampMagnitude(_rawInputMovement, 1); // clamp the speed in diagonal

                inputAngle = Vector2.SignedAngle(Vector2.up, new Vector2(-_rawInputMovement.x, _rawInputMovement.z));

                playerPosition = transform.position;

                if (!isFocusCamera && !BananaMan.Instance.isGrabingBananaGun) {  // rotate follow the input
                    if (_rawInputMovement != Vector3.zero) {
                        transform.rotation = Quaternion.AngleAxis(inputAngle, Vector3.up) * cameraRotation;
                    }
                }
                else {   // rotate strictly follow the camera 
                    transform.rotation = cameraRotation;
                }
                
                _movement = cameraRotation * _rawInputMovement * (baseMovementSpeed * Time.deltaTime);
                
                if (jumpCounter < JumpHeight && isJumping) {
                    jumpCounter += Time.deltaTime;
                    _movement.y += 5 * Time.deltaTime;
                    BananaMan.Instance.isInAir = true;
                }
                else { // normal gravity
                    jumpCounter = 0f;
                    _movement.y += Gravity * Time.deltaTime;
                    isJumping = false;
                }
                
                _characterController.Move(_movement);
                
                _tpsPlayerAnimatorScript.UpdateMovementAnimation(_rawInputMovement.z * baseMovementSpeed,
                    _rawInputMovement.x * baseMovementSpeed);
                
                // is the player moving ?
                _currentPosition = playerPosition;
                _tpsPlayerAnimatorScript.IsMoving(_currentPosition != _lastPosition);
                _lastPosition = _currentPosition;
                
                UIFace.Instance.MoveFaceAnimation(_rawInputMovement.magnitude);
            }
        }
        
        public void FocusCamera() {
            if (!isFocusCamera) {
                _tpsPlayerAnimatorScript.FocusCamera(true);
                isFocusCamera = true;
                return;
            }

            _tpsPlayerAnimatorScript.FocusCamera(false);
            isFocusCamera = false;
        }
        
        public void PlayerJump() {
            if (!BananaMan.Instance.isInAir && !isRolling) {
                if (!BananaMan.Instance.isRagdoll && !isFocusCamera) {
                    BananaMan.Instance.isInAir = true;
                    _tpsPlayerAnimatorScript.Jump();
                    isJumping = true;
                }
                else {
                    if (_rawInputMovement.z >= 0) {
                        BananaMan.Instance.isInAir = true;
                        _tpsPlayerAnimatorScript.Jump();
                        isJumping = true;
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
    }
}