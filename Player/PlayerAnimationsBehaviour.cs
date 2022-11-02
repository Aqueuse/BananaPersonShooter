using UnityEngine;

namespace Player {
    public class PlayerAnimationsBehaviour : StateMachineBehaviour {
        private Rigidbody _playerRigidBody;
        private readonly Vector3 _highJump = new Vector3(0, 4, 0);

        private void OnEnable() {
            _playerRigidBody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        }
    
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("jump")) {
                if (stateInfo.normalizedTime * 100 > 30 && stateInfo.normalizedTime * 100 < 35) {
                    _playerRigidBody.velocity = _highJump;
                }
            }
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("throw")) {
                BananaMan.Instance.tpsPlayerAnimator.ExitArmsOnlyLayer();
            }
            if (stateInfo.IsTag("searching")) {
                BananaMan.Instance.tpsPlayerAnimator.ExitArmsOnlyLayer();
            }
        }
    }
}
