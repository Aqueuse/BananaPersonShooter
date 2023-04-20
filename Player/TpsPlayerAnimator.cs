using UnityEngine;

namespace Player {
    public class TpsPlayerAnimator : MonoBehaviour {
        private Animator _playerAnimator;
        private int _layerArmsOnly;
        private int _freeMoveLayer;
        private int _moverLayer;

        //Animation String IDs
        private readonly int _playerZMovementAnimationId = Animator.StringToHash("Movement Z");
        private readonly int _playerXMovementAnimationId = Animator.StringToHash("Movement X");
        
        private static readonly int RollID = Animator.StringToHash("ROLL");
        private static readonly int Getup = Animator.StringToHash("GETUP");
        private static readonly int IsGroundedID = Animator.StringToHash("IsGrounded");
        private static readonly int IsJumpingID = Animator.StringToHash("IsJumping");

        private void Start() {
            _playerAnimator = GetComponent<Animator>();
            _freeMoveLayer = _playerAnimator.GetLayerIndex("FREE MOVE");
            _moverLayer = _playerAnimator.GetLayerIndex("MOVER");
        }
    
        public void UpdateMovementAnimation(float movementIntensityZ, float movementIntensityX) {
            _playerAnimator.SetFloat(_playerZMovementAnimationId, movementIntensityZ);
            _playerAnimator.SetFloat(_playerXMovementAnimationId, movementIntensityX);
        }
        
        public void GrabBananaGun() {
            _playerAnimator.SetLayerWeight(_freeMoveLayer, 0);
            _playerAnimator.SetLayerWeight(_moverLayer, 1);
        }
        
        public void Jump() {
            _playerAnimator.SetBool(IsJumpingID, true);
        }
        
        public void SetGrounded(bool isGrounded) {
            _playerAnimator.SetBool(IsGroundedID, isGrounded);
        }
        
        public void Roll() {
            _playerAnimator.SetTrigger(RollID);
        }

        public void GetUp() {
            _playerAnimator.SetTrigger(Getup);
        }

        public void FocusCamera(bool isFocusCam) {
            if (isFocusCam) {
                _playerAnimator.SetLayerWeight(_moverLayer, 0);
                _playerAnimator.SetLayerWeight(_freeMoveLayer, 1);
            }
            else {
                _playerAnimator.SetLayerWeight(_freeMoveLayer, 0);
                _playerAnimator.SetLayerWeight(_moverLayer, 0);
            }
        }
    }
}
