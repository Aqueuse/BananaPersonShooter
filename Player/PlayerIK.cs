using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player {
    public class PlayerIK : MonoBehaviour {
        [SerializeField] private Transform ikTargetLeftHand;
        [SerializeField] private MultiAimConstraint headAimConstraint;
        [SerializeField] private MultiAimConstraint backAimConstraint;

        private Animator _animator;

        private float _footExtended;
        
        public bool isLeftFootGrounded;
        public bool isRightFootGrounded;

        private float _leftFootDistanceToPlayerRoot;
        private float _rightFootDistanceToPlayerRoot;
        
        private Vector3 _footPosition;
        private Vector3 _terrainOrientation;
        
        private void Start() {
            _animator = GetComponent<Animator>();
            isLeftFootGrounded = false;
            isRightFootGrounded = false;

            _footExtended = 0.1818f;
            SetAimConstraint(false);
        }
        
        public void SetAimConstraint(bool active) {
            if (active) {
                headAimConstraint.weight = 1;
                backAimConstraint.weight = 1;
            }
            else {
                headAimConstraint.weight = 0;
                backAimConstraint.weight = 0;
            }
        }

        private void OnAnimatorIK(int layerIndex) {
            _leftFootDistanceToPlayerRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.LeftFoot), _animator.rootPosition);
            _rightFootDistanceToPlayerRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.RightFoot), _animator.rootPosition);
            
            if (_leftFootDistanceToPlayerRoot < _footExtended && !isLeftFootGrounded) {
                ObjectsReference.Instance.audioManager.PlayFootstep();
                isLeftFootGrounded = true;
            }

            if (_leftFootDistanceToPlayerRoot >= _footExtended) {
                isLeftFootGrounded = false;
            }

            if (_rightFootDistanceToPlayerRoot < _footExtended && !isRightFootGrounded) {
                ObjectsReference.Instance.audioManager.PlayFootstep();
                isRightFootGrounded = true;
            }

            if (_rightFootDistanceToPlayerRoot >= _footExtended) {
                isRightFootGrounded = false;
            }
            
            if (ObjectsReference.Instance.bananaMan.isGrabingBananaGun) {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                _animator.SetIKPosition(AvatarIKGoal.LeftHand, ikTargetLeftHand.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, ikTargetLeftHand.rotation);
            }
            else {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
