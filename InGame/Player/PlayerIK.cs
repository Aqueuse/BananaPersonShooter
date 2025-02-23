using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace InGame.Player {
    public class PlayerIK : MonoBehaviour {
        [SerializeField] private Rig bananaManRig;
        
        [SerializeField] private TwoBoneIKConstraint rightHandConstraint;
        [SerializeField] private AimConstraint backAimConstraint;
        [SerializeField] private LookAtConstraint headLookAtConstraint;

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

            headLookAtConstraint.weight = 1;
        }
        
        public void SetAimConstraint(bool active) {
            if (active) {
                rightHandConstraint.weight = 1;
                backAimConstraint.weight = 1;
                backAimConstraint.constraintActive = true;
                headLookAtConstraint.weight = 1;
                headLookAtConstraint.constraintActive = true;
            }
            else {
                rightHandConstraint.weight = 0;
                backAimConstraint.weight = 0;
                backAimConstraint.constraintActive = false;
                headLookAtConstraint.weight = 0;
                headLookAtConstraint.constraintActive = false;
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

            if (_rightFootDistanceToPlayerRoot < _footExtended & !isRightFootGrounded) {
                ObjectsReference.Instance.audioManager.PlayFootstep();
                isRightFootGrounded = true;
            }

            if (_rightFootDistanceToPlayerRoot >= _footExtended) {
                isRightFootGrounded = false;
            }
        }
        
        public void SetGrabbedBananaGunRigWeight(bool isGrabbing) {
            if (isGrabbing) {
                bananaManRig.weight = 1;
            }
            else {
                bananaManRig.weight = 0;
            }
        }
    }
}
