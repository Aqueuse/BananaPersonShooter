using Gestion.Buildables.Plateforms;
using UnityEngine;

namespace Gestion.Buildables {
    public class WorkbenchAnimationBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("closing")) {
                animator.GetComponentInParent<Plateform>().FinishHidingWorkbench();
            }
        }
    }
}
