using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace InGame.Player {
    public class TpsPlayerAnimator : MonoBehaviour {
        private Animator _playerAnimator;

        //Animation String IDs
        private readonly int _playerZMovementAnimationId = Animator.StringToHash("Movement Z");
        private readonly int _playerXMovementAnimationId = Animator.StringToHash("Movement X");

        private static readonly int RollID = Animator.StringToHash("ROLL");
        private static readonly int Getup = Animator.StringToHash("GETUP");
        private static readonly int IsGroundedID = Animator.StringToHash("IsGrounded");
        private static readonly int IsJumpingID = Animator.StringToHash("IsJumping");
        private static readonly int IsFallingFrontwardID = Animator.StringToHash("IsDyingFrontward");

        private void Start() {
            _playerAnimator = GetComponent<Animator>();
        }

        public void UpdateMovementAnimation(float movementIntensityZ, float movementIntensityX) {
            _playerAnimator.SetFloat(_playerZMovementAnimationId, movementIntensityZ);
            _playerAnimator.SetFloat(_playerXMovementAnimationId, movementIntensityX);
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

        public void FallFrontward() {
            _playerAnimator.SetBool(IsFallingFrontwardID, true);
        }

        public void GetUp() {
            _playerAnimator.SetBool(IsFallingFrontwardID, false);
        }

        public void SetBananaGunLayerWeight(bool active) {
            _playerAnimator.SetLayerWeight(1, active ? 1 : 0);
        }
    }
}
