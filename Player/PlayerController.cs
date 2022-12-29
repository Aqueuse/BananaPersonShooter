using System.Collections;
using Cameras;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {

    public class PlayerController : MonoBehaviour {
        [Header("Movement Settings")] public float baseMovementSpeed = 6f;
        public float sprintMovementSpeed = 9f;
        private const float JumpImpulse = 400f;

        public bool isFocusCamera;
        public bool isRolling;
        private bool _isRagdoll;
        public bool canMove = true;
        
        private TpsPlayerAnimator _tpsPlayerAnimatorScript;
        private Animator _playerAnimator;
        private Rigidbody _rigidbody;
        private RagDoll _ragDoll;

        private int _damageCounter;

        //Stored Values
        private Vector3 _rawInputMovement;
        private Transform _mainCameraTransform;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private void Start() {
            _tpsPlayerAnimatorScript = gameObject.GetComponent<TpsPlayerAnimator>();
            _playerAnimator = gameObject.GetComponent<Animator>();
            _ragDoll = gameObject.GetComponent<RagDoll>();
            _rigidbody = GetComponent<Rigidbody>();

            if (Camera.main != null) _mainCameraTransform = Camera.main.transform;
        }

        private void FixedUpdate() {
            if (_isRagdoll) {
                _damageCounter++;
            }

            if (GameManager.Instance.isGamePlaying && !_isRagdoll && canMove) {
                Quaternion cameraRotation = new Quaternion(0, _mainCameraTransform.transform.rotation.y, 0,
                    _mainCameraTransform.rotation.w).normalized;

                var inputAngle = Vector2.SignedAngle(Vector2.up, new Vector2(-_rawInputMovement.x, _rawInputMovement.z));

                var playerPosition = transform.position;

                if (!isFocusCamera && !BananaMan.Instance.isGrabingMover) {  // rotate follow the input
                    if (_rawInputMovement != Vector3.zero) {
                        _rigidbody.rotation = Quaternion.AngleAxis(inputAngle, Vector3.up) * cameraRotation;
                    }
                }
                else {   // rotate strictly follow the camera 
                    _rigidbody.rotation = cameraRotation;
                }
                
                var moveToRotate = cameraRotation * _rawInputMovement * (baseMovementSpeed * Time.fixedDeltaTime);
                _rigidbody.MovePosition(playerPosition + new Vector3(moveToRotate.x, 0, moveToRotate.z));
                
                _tpsPlayerAnimatorScript.UpdateMovementAnimation(_rawInputMovement.z * baseMovementSpeed,
                    _rawInputMovement.x * baseMovementSpeed);

                // is the player moving ?
                _currentPosition = playerPosition;
                _tpsPlayerAnimatorScript.IsMoving(_currentPosition != _lastPosition);
                _lastPosition = _currentPosition;
                
                UIFace.Instance.MoveFaceAnimation(_rawInputMovement.magnitude);
            }
        }
        
        public void FocusCamera(InputAction.CallbackContext context) {
            if (context.performed) {
                if (!isFocusCamera) {
                    _tpsPlayerAnimatorScript.FocusCamera(true);
                    isFocusCamera = true;
                    return;
                }

                _tpsPlayerAnimatorScript.FocusCamera(false);
                isFocusCamera = false;
            }
        }

        public void FreeCamera() {
            _tpsPlayerAnimatorScript.FocusCamera(false);
            isFocusCamera = false;
        }
        
        public void PlayerJump(InputAction.CallbackContext value) {
            if (value.performed) {
                if (!BananaMan.Instance.isInAir) {
                    if (!_isRagdoll) {
                        if (!isFocusCamera) {
                            BananaMan.Instance.isInAir = true;
                            _tpsPlayerAnimatorScript.Jump();
                            PlayerVerticalImpulse();
                        }
                        else {
                            if (_rawInputMovement.z >= 0) {
                                BananaMan.Instance.isInAir = true;
                                _tpsPlayerAnimatorScript.Jump();
                                PlayerVerticalImpulse();
                            }
                        }
                    }
                }
            }
        }

        public void PlayerVerticalImpulse() {
            _rigidbody.AddForce(0, JumpImpulse, 0, ForceMode.Impulse);
            BananaMan.Instance.isInAir = true;
        }

        public void PlayerRoll(InputAction.CallbackContext value) {
            if (value.performed) {
                _tpsPlayerAnimatorScript.Roll();
            }
        }

        public void PlayerMovement(InputAction.CallbackContext callbackContext) {
            Vector2 inputMovement = callbackContext.ReadValue<Vector2>();
            _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y); // Y en input => Z pour le player (forward)
        }

        public void PlayerSprint(InputAction.CallbackContext value) {
            if (value.performed) {
                baseMovementSpeed = sprintMovementSpeed;
            }

            if (value.canceled) {
                baseMovementSpeed = 6f;
            }
        }
        
        public void PlayerRagdollAgainstCollider(Collider objectCollider, float force) {
            if (!isRolling) {
                _damageCounter = 0;
                _isRagdoll = true;

                _playerAnimator.enabled = false;
                _ragDoll.SetRagDoll(true);
                UIFace.Instance.GetGooglyEyes();

                _rigidbody.AddForce(force*(transform.position - objectCollider.transform.position), ForceMode.Impulse);

                StartCoroutine(SetPlayerBackFromRagdoll());
            }
        }
        
        IEnumerator SetPlayerBackFromRagdoll() {
            yield return new WaitForSeconds(1.5f);
            
            _ragDoll.SetRagDoll(false);

            _playerAnimator.enabled = true;
            _tpsPlayerAnimatorScript.GetUp();
            _isRagdoll = false;

            yield return new WaitForSeconds(0.7f);
            
            BananaMan.Instance.TakeDamage(_damageCounter);
            
            yield return null;
        }

        public void Die() {
            _isRagdoll = true;
            _playerAnimator.enabled = false;

            canMove = false;
            _ragDoll.SetRagDoll(true);
        }

        public void ResetToPlayable() {
            BananaMan.Instance.GetComponent<RagDoll>().SetRagDoll(false);
            _isRagdoll = false;
            isRolling = false;
            canMove = true;

            _playerAnimator.enabled = true;
            _mainCameraTransform.GetComponent<ThirdPersonOrbitCamBasic>().enabled = true;
            UIFace.Instance.Die(false);
            UIFace.Instance.GetHurted(BananaMan.Instance.health < 50);
        }
    }
}