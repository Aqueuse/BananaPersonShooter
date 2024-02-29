using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using UnityEngine;

namespace InGame.Animation {
    public class WorkbenchAnimationBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsTag("closing")) {
                animator.GetComponentInParent<PlateformBehaviour>().FinishHidingWorkbench();
            }
        }
    }
}
