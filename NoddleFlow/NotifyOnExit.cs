using Behaviours;
using UnityEngine;

public class NotifyOnExit : StateMachineBehaviour {
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var executor = FindFirstObjectByType<PlayAnimationBlockExecutor>();
        executor?.NotifyAnimationEnded();
    }
}