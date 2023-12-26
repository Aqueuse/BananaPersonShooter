using UnityEngine;

namespace Game.Monkeys.Visichimps {
    public class VisitorAnimationsBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("rest")) {
                animator.GetComponent<VisitorBehaviour>().FillNeed();
            }
        }
    }
}