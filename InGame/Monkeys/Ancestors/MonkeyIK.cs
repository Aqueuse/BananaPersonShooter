using UnityEngine;

namespace InGame.Monkeys.Ancestors {
    public class MonkeyIK : MonoBehaviour {
        [SerializeField] private MonkeySounds _monkeySounds;
        private Animator _animator;

        private float _footExtended;
        private float _handExtended;
        
        public bool isLeftFootGrounded;
        public bool isRightFootGrounded;

        public bool isLeftHandGrounded;
        public bool isRightHandGrounded;

        private float _leftFootDistanceToMonkeyRoot;
        private float _rightFootDistanceToMonkeyRoot;

        private float _leftHandDistanceToMonkeyRoot;
        private float _rightHandDistanceToMonkeyRoot;

        private Vector3 _footPosition;
        private Vector3 _terrainOrientation;
        
        private void Start() {
            _animator = GetComponent<Animator>();
            isLeftFootGrounded = false;
            isRightFootGrounded = false;

            isLeftHandGrounded = false;
            isRightHandGrounded = false;

            _footExtended = 0.1818f;
            _handExtended = 0.1818f;
        }
        
        private void OnAnimatorIK(int layerIndex) {
            _leftFootDistanceToMonkeyRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.LeftFoot), _animator.rootPosition);
            _rightFootDistanceToMonkeyRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.RightFoot), _animator.rootPosition);

            _leftHandDistanceToMonkeyRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.LeftHand), _animator.rootPosition);
            _rightHandDistanceToMonkeyRoot = Vector3.Distance(_animator.GetIKPosition(AvatarIKGoal.RightHand), _animator.rootPosition);
            
            //// FOOT
            if (_leftFootDistanceToMonkeyRoot < _footExtended && !isLeftFootGrounded) {
                _monkeySounds.PlayFootstep();
                isLeftFootGrounded = true;
            }
        
            if (_leftFootDistanceToMonkeyRoot >= _footExtended) {
                isLeftFootGrounded = false;
            }
        
            if (_rightFootDistanceToMonkeyRoot < _footExtended && !isRightFootGrounded) {
                _monkeySounds.PlayFootstep();
                isRightFootGrounded = true;
            }
            
            if (_rightFootDistanceToMonkeyRoot >= _footExtended) {
                isRightFootGrounded = false;
            }
            
            ///// HAND
            if (_leftHandDistanceToMonkeyRoot < _handExtended && !isLeftHandGrounded) {
                _monkeySounds.PlayFootstep();
                isLeftHandGrounded = true;
            }
        
            if (_leftHandDistanceToMonkeyRoot >= _handExtended) {
                isLeftHandGrounded = false;
            }
        
            if (_rightHandDistanceToMonkeyRoot < _handExtended && !isRightHandGrounded) {
                _monkeySounds.PlayFootstep();
                isRightHandGrounded = true;
            }
            
            if (_rightHandDistanceToMonkeyRoot >= _handExtended) {
                isRightHandGrounded = false;
            }
        }
    }
}
