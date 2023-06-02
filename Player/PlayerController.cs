﻿using UnityEngine;

namespace Player {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private Transform groundCheck;

        private TpsPlayerAnimator _tpsPlayerAnimatorScript;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private CapsuleCollider _capsuleCollider;

        private RaycastHit _slopeHit;
        private RaycastHit hitTest;
        
        public float jumpForce;

        public float speed;
        private const float BaseMovementSpeed = 6f;
        private const float SprintMovementSpeed = 15f;

        public bool isInWater;
        public bool isFocusCamera;
        public bool isRolling;
        public bool canMove = true;
        private bool _isGrounded;
        private bool _isHit;

        private float _damageCount;

        //Stored Values
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private Vector3 _rawInputMovement;
        public Transform mainCameraTransform;
        private Vector3 _cameraForward;
        private float _inputAngle;

        private readonly float _maxDistanceToCollide = 0.2f;
        private Vector3 _movement;
        private Vector3 _newPosition;
        private float _slopeAngle;
        private readonly float _maxSlopeAngle = 60f;
        private Quaternion _cameraRotation;
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _tpsPlayerAnimatorScript = gameObject.GetComponent<TpsPlayerAnimator>();

            if (Camera.main != null) mainCameraTransform = Camera.main.transform;

            speed = BaseMovementSpeed;

            _damageCount = 0;

            // TODO : _rigidbody.maxLinearVelocity = 40;
        }
        
        private void FixedUpdate() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying) return;

            // ugly but opti
            _cameraRotation.x = 0;
            _cameraRotation.y = mainCameraTransform.transform.rotation.y;
            _cameraRotation.z = 0;
            _cameraRotation.w = mainCameraTransform.rotation.w;
            _cameraRotation = _cameraRotation.normalized;
                
            _rawInputMovement.x = ObjectsReference.Instance.gameActions.move.x;
            _rawInputMovement.y = 0;
            _rawInputMovement.z = ObjectsReference.Instance.gameActions.move.y; // Y en input => Z pour le player (forward)
                
            _rawInputMovement = Vector3.ClampMagnitude(_rawInputMovement, 1); // clamp the speed in diagonal
            
            /////// ROTATION ///////
            _inputAngle = Vector2.SignedAngle(Vector2.up, new Vector2(-_rawInputMovement.x, _rawInputMovement.z));

            if (!ObjectsReference.Instance.bananaMan.isGrabingBananaGun) {
                if (_rawInputMovement.magnitude > 0) transform.rotation = Quaternion.AngleAxis(_inputAngle, Vector3.up) * _cameraRotation;
            }
            else {
                transform.rotation = _cameraRotation;
            }
            
            if (!canMove) return;
            
            ////// MOVEMENT ///////
            // If the input is null, stop the movement
            if (_rawInputMovement is { x: 0, z: 0 }) {
                _movement = Vector3.zero;
                PlayerStopSprint();
            }
            else {
                _cameraForward = Vector3.Scale(mainCameraTransform.forward, new Vector3(1, 0, 1)).normalized;
                _movement = (_rawInputMovement.z * _cameraForward + _rawInputMovement.x * mainCameraTransform.right).normalized * (speed * Time.fixedDeltaTime);
            }
            
            if (IsOnSlope()) _movement = GetSlopeMoveDirection(_movement);
            
            // collision prevention
            _isHit = _rigidbody.SweepTest(_movement.normalized, out hitTest, _maxDistanceToCollide);
            
            if (_isHit && !hitTest.collider.isTrigger) {
                // Vérifie si la normale de la collision est orientée vers le haut (collide contre le sol ne veux rien dire)
                _newPosition = hitTest.normal.y > 0.5f ? _rigidbody.position + _movement : _rigidbody.position;
            }
            else {
                _newPosition = _rigidbody.position + _movement;
            }
            
            _rigidbody.MovePosition(_newPosition);
            
            if (_rigidbody.velocity.y < -20) {
                _damageCount += 1;
            }

            if (_isGrounded && _damageCount > 0) {
                ObjectsReference.Instance.bananaMan.TakeDamage(_damageCount);
                _damageCount = 0;
            }

            // end jump
            if (Physics.Raycast(groundCheck.position, Vector3.down, 0.3f)) {
                _isGrounded = true;
                _animator.SetBool(IsJumping, false);
                _animator.SetBool(IsFalling, false);
            }
            
            else {
                _isGrounded = false;
                _animator.SetBool(IsFalling, true);
            }
            
            _tpsPlayerAnimatorScript.UpdateMovementAnimation(_rawInputMovement.z*speed, _rawInputMovement.x*speed);
            _tpsPlayerAnimatorScript.SetGrounded(_isGrounded);
            ObjectsReference.Instance.uiFace.MoveFaceAnimation(_rawInputMovement.magnitude);
        }

        public void PlayerJump() {
            if (_isGrounded && !isRolling) {
               _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
               _tpsPlayerAnimatorScript.Jump();
            }
        }

        public void PlayerRoll() {
            if (_isGrounded && !ObjectsReference.Instance.bananaMan.isGrabingBananaGun && isRolling == false) {
                if (_rawInputMovement.z != 0 || _rawInputMovement.x != 0) {
                     _tpsPlayerAnimatorScript.Roll();
                    _capsuleCollider.height = 0.90f;
                    _capsuleCollider.center = new Vector3(-0.01361084f, 0.44f, 1.027142e-11f);
                }
            }
        }

        public void PlayerSprint() {
            if (_isGrounded && !isRolling && !isInWater) speed = SprintMovementSpeed;
        }

        public void PlayerStopSprint() {
            speed = BaseMovementSpeed;
        }

        public void StopPlayer() {
            canMove = false;
            speed = 0;
            _tpsPlayerAnimatorScript.UpdateMovementAnimation(0, 0);
        }

        public void ResetPlayer() {
            speed = BaseMovementSpeed;
            _tpsPlayerAnimatorScript.UpdateMovementAnimation(0, 0);
        }

        private bool IsOnSlope() {
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 0.8f)) {
                _slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                return _slopeAngle < _maxSlopeAngle && _slopeAngle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 movementDirection) {
            return Vector3.ProjectOnPlane(movementDirection, _slopeHit.normal);
        }
    }
}