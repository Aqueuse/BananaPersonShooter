using Gestion.BuildablesBehaviours;
using UnityEngine;

namespace StateMachineBehaviours {
    public class WorkbenchAnimationBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("closing")) {
                animator.GetComponentInParent<PlateformBehaviour>().FinishHidingWorkbench();
            }
        }
    }
}
