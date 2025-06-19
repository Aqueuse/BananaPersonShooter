using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace InGame.Monkeys {
    public class MonkeyMenAnimationsBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("visitorAction")) {
                animator.GetComponent<VisitorBehaviour>().FinishSatisfyNeed();
            }
        }
    }
}