using UnityEngine;

namespace Player {
    public class PlayerAnimationsBehaviour : StateMachineBehaviour {

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("throw")) {
                BananaMan.Instance.tpsPlayerAnimator.ExitArmsOnlyLayer();
            }
            if (stateInfo.IsTag("searching")) {
                BananaMan.Instance.tpsPlayerAnimator.ExitArmsOnlyLayer();
            }

            if (stateInfo.IsTag("roll") || stateInfo.IsName("standing jump end") || stateInfo.IsName("jump when sprint end")) {
                BananaMan.Instance.GetComponent<PlayerController>().isRolling = false;
            }

            if (stateInfo.IsName("standing jump end")) {
                BananaMan.Instance.tpsPlayerAnimator.IsGrounded(true);
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("roll") || stateInfo.IsName("jump when sprint start") || stateInfo.IsName("standing jump start")) {
                BananaMan.Instance.GetComponent<PlayerController>().isRolling = true;
            }
        }
    }
}
