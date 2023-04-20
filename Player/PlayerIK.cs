using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player {
    public class PlayerIK : MonoBehaviour {
        [SerializeField] private Transform ikTargetLeftHand;
        [SerializeField] private MultiAimConstraint headAimConstraint;
        [SerializeField] private MultiAimConstraint backAimConstraint;

        private Animator _animator;

        private float footExtended;
        
        public bool isLeftFootGrounded;
        public bool isRightFootGrounded;

        private float leftFootDistanceToPlayerRoot;
        private float rightFootDistanceToPlayerRoot;
        
        private Vector3 footPosition;
        private Vector3 terrainOrientation;
        
        private void Start() {
            _animator = GetComponent<Animator>();
            isLeftFootGrounded = false;
            isRightFootGrounded = false;

            footExtended = 0.1818f;
        }

        private void Update() {
            if (ObjectsReference.Instance.bananaMan.isGrabingBananaGun) {
                headAimConstraint.weight = 1;
                backAimConstraint.weight = 1;
            }
            else {
                headAimConstraint.weight = 0;
                backAimConstraint.weight = 0;
            }
        }

        private void OnAnimatorIK(int layerIndex) {
            leftFootDistanceToPlayerRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.LeftFoot), _animator.rootPosition);
            rightFootDistanceToPlayerRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.RightFoot), _animator.rootPosition);
            
            if (leftFootDistanceToPlayerRoot < footExtended && !isLeftFootGrounded) {
                ObjectsReference.Instance.audioManager.PlayFootstep();
                isLeftFootGrounded = true;
            }

            if (leftFootDistanceToPlayerRoot >= footExtended) {
                isLeftFootGrounded = false;
            }

            if (rightFootDistanceToPlayerRoot < footExtended && !isRightFootGrounded) {
                ObjectsReference.Instance.audioManager.PlayFootstep();
                isRightFootGrounded = true;
            }

            if (rightFootDistanceToPlayerRoot >= footExtended) {
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
