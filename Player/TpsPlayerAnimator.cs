using UnityEngine;

namespace Player {
    public class TpsPlayerAnimator : MonoBehaviour {
        private Animator _playerAnimator;
        
        //Animation String IDs
        private int _playerZMovementAnimationId;
        private int _playerXMovementAnimationId;
        private int _playerJumpAnimationId;

        private int _layerArmed;
        private int _layerUnarmed;
        
        private void Start() {
            _playerAnimator = GetComponent<Animator>();
            _playerZMovementAnimationId = Animator.StringToHash("Movement Z");
            _playerXMovementAnimationId = Animator.StringToHash("Movement X");
            _playerJumpAnimationId = Animator.StringToHash("JUMP");

            _layerUnarmed = _playerAnimator.GetLayerIndex("Unarmed");
            _layerArmed = _playerAnimator.GetLayerIndex("Armed");
        }
    
        public void UpdateMovementAnimation(float movementIntensityZ, float movementIntensityX) {
            _playerAnimator.SetFloat(_playerZMovementAnimationId, movementIntensityZ);
            _playerAnimator.SetFloat(_playerXMovementAnimationId, movementIntensityX);
        }

        public void Jump() {
            _playerAnimator.SetTrigger(_playerJumpAnimationId);
        }

        public void SwitchToUnarmedLayer() {
            _playerAnimator.SetLayerWeight(_layerUnarmed, 1);
            _playerAnimator.SetLayerWeight(_layerArmed, 0);
        }

        public void SwitchToArmedLayer() {
            _playerAnimator.SetLayerWeight(_layerUnarmed, 0);
            _playerAnimator.SetLayerWeight(_layerArmed, 1);
        }

    }
}
