using InGame.Monkeys.Chimpirates;
using InGame.Monkeys.Chimptouristes;
using UnityEngine;

namespace InGame.Monkeys {
    public class MonkeyMenAnimationsBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("visitorAction")) {
                animator.GetComponent<TouristBehaviour>().FillNeed();
            }
            
            if (stateInfo.IsTag("break")) {
                animator.GetComponent<PirateBehaviour>().GoBackToTeleporter();
            }
        }
    }
}