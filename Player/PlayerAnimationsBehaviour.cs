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

                BananaMan.Instance.GetComponent<CharacterController>().height = 1.82f;
                BananaMan.Instance.GetComponent<CharacterController>().center = new Vector3(0, 0.88f, 0);

                BananaMan.Instance.GetComponent<CapsuleCollider>().height = 1.85f;
            }

            if (stateInfo.IsName("standing jump end")) {
                BananaMan.Instance.tpsPlayerAnimator.IsGrounded(true);
            }

            if (stateInfo.IsTag("slide left interface")) {
                GameManager.Instance.PauseGame(true);
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("roll") || stateInfo.IsName("jump when sprint start") || stateInfo.IsName("standing jump start")) {
                BananaMan.Instance.GetComponent<PlayerController>().isRolling = true;
            }
        }
    }
}
