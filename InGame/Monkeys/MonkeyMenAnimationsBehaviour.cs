using InGame.Monkeys.Chimpirates;
using InGame.Monkeys.Visichimps;
using UnityEngine;

namespace InGame.Monkeys {
    public class MonkeyMenAnimationsBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("visitorAction")) {
                animator.GetComponent<VisitorBehaviour>().FillNeed();
            }
            
            if (stateInfo.IsTag("break")) {
                animator.GetComponent<PirateBehaviour>().GoBackToSas();
            }
        }
    }
}