using UnityEngine;

namespace Player {
    public class TpsPlayerAnimator : MonoBehaviour {
        private Animator _playerAnimator;
        private int _layerArmsOnly;
        private int _focusLayer;
        private int _freeMoveLayer;
        private int _moverLayer;

        //Animation String IDs
        private readonly int _playerZMovementAnimationId = Animator.StringToHash("Movement Z");
        private readonly int _playerXMovementAnimationId = Animator.StringToHash("Movement X");
        private readonly int _playerThrowAnimationId = Animator.StringToHash("throw");
        
        private static readonly int JumpID = Animator.StringToHash("JUMP");
        private static readonly int RollID = Animator.StringToHash("ROLL");
        private static readonly int Getup = Animator.StringToHash("GETUP");
        private static readonly int IsInAirID = Animator.StringToHash("IS IN AIR");
        private static readonly int IsMovingID = Animator.StringToHash("IS MOVING");
        private static readonly int IsGroundedID = Animator.StringToHash("IS GROUNDED");

        private void Start() {
            _playerAnimator = GetComponent<Animator>();
            _layerArmsOnly = _playerAnimator.GetLayerIndex("Arms Layer");

            _focusLayer = _playerAnimator.GetLayerIndex("FOCUS");
            _freeMoveLayer = _playerAnimator.GetLayerIndex("FREE MOVE");
            _moverLayer = _playerAnimator.GetLayerIndex("MOVER");
        }
    
        public void UpdateMovementAnimation(float movementIntensityZ, float movementIntensityX) {
            _playerAnimator.SetFloat(_playerZMovementAnimationId, movementIntensityZ);
            _playerAnimator.SetFloat(_playerXMovementAnimationId, movementIntensityX);
        }

        public void ThrowAnimation() {
            _playerAnimator.SetLayerWeight(_layerArmsOnly, 1);
            _playerAnimator.SetTrigger(_playerThrowAnimationId);
        }

        public void GrabMover() {
            _playerAnimator.SetLayerWeight(_focusLayer, 0);
            _playerAnimator.SetLayerWeight(_freeMoveLayer, 0);
            _playerAnimator.SetLayerWeight(_moverLayer, 1);
        }
        
        public void Jump() {
            _playerAnimator.SetTrigger(JumpID);
        }

        public void IsInAir(bool isInAir) {
            _playerAnimator.SetBool(IsInAirID, isInAir);
        }
        
        public void IsGrounded(bool isGrounded) {
            _playerAnimator.SetBool(IsGroundedID, isGrounded);
        }

        public void IsMoving(bool isMoving) {
            _playerAnimator.SetBool(IsMovingID, isMoving);
        }

        public void Roll() {
            _playerAnimator.SetTrigger(RollID);
        }

        public void GetUp() {
            _playerAnimator.SetTrigger(Getup);
        }

        public void FocusCamera(bool isFocusCam) {
            if (isFocusCam) {
                _playerAnimator.SetLayerWeight(_freeMoveLayer, 0);
                _playerAnimator.SetLayerWeight(_moverLayer, 0);
                _playerAnimator.SetLayerWeight(_focusLayer, 1);
            }
            else {
                _playerAnimator.SetLayerWeight(_focusLayer, 0);
                _playerAnimator.SetLayerWeight(_moverLayer, 0);
                _playerAnimator.SetLayerWeight(_freeMoveLayer, 1);
            }
        }

        public void ExitArmsOnlyLayer() {
            _playerAnimator.SetLayerWeight(_layerArmsOnly, 0);
        }
    }
}
