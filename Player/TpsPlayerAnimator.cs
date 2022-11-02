using UnityEngine;

namespace Player {
    public class TpsPlayerAnimator : MonoBehaviour {
        private Animator _playerAnimator;
        
        //Animation String IDs
        private int _playerZMovementAnimationId;
        private int _playerXMovementAnimationId;
        private int _playerJumpAnimationId;

        private int _layerArmsOnly;
        
        private void Start() {
            _playerAnimator = GetComponent<Animator>();
            _playerZMovementAnimationId = Animator.StringToHash("Movement Z");
            _playerXMovementAnimationId = Animator.StringToHash("Movement X");
            _playerJumpAnimationId = Animator.StringToHash("JUMP");

            _layerArmsOnly = _playerAnimator.GetLayerIndex("Arms Layer");
        }
    
        public void UpdateMovementAnimation(float movementIntensityZ, float movementIntensityX) {
            _playerAnimator.SetFloat(_playerZMovementAnimationId, movementIntensityZ);
            _playerAnimator.SetFloat(_playerXMovementAnimationId, movementIntensityX);
        }

        public void Jump() {
            _playerAnimator.SetTrigger(_playerJumpAnimationId);
        }

        public void ThrowAnimation() {
            _playerAnimator.SetLayerWeight(_layerArmsOnly, 1);
            _playerAnimator.SetTrigger("throw");
        }
        
        public void BananaNotFound() {
            _playerAnimator.SetLayerWeight(_layerArmsOnly, 1);
            _playerAnimator.SetTrigger("searching");
        }

        public void ExitArmsOnlyLayer() {
            _playerAnimator.SetLayerWeight(_layerArmsOnly, 0);
        }
    }
}
