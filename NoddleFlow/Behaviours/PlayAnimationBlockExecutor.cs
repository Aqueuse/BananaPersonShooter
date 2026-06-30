using System.Threading.Tasks;
using UnityEngine;

namespace NoddleFlow.Behaviours {
    /// <summary>
    /// AI block responsible for playing an animation via an Animator trigger, then waiting
    /// for the animation to finish before resuming the sequence.
    /// </summary>
    /// <remarks>
    /// Simple solution: a StateMachineBehaviour that notifies the context;
    /// Specifically, a <see cref="StateMachineBehaviour"/> (or any other notifier)
    /// is expected to signal the end of the animation by setting
    /// <c>isCompleted</c> to <c>true</c>. The block:
    /// 1) resets the trigger,
    /// 2) sets the trigger,
    /// 3) waits for the completion notification,
    /// 4) resets the trigger.
    /// </remarks>
    public class PlayAnimationBlockExecutor : AiBlockExecutor {
        public string animationTarget;
        public string animationName;
        public bool ended;
        
        public override async Task Execute(GraphExecutor graphExecutor) {
            var associatedTargets = FindObjectsByType<AiTarget>(FindObjectsSortMode.None);
            
            if (associatedTargets.Length == 0) {
                Debug.LogError("AITarget not found on scene. Please add one as a component of your target GameObject");
                return;
            }
            
            foreach (var associatedTarget in associatedTargets) {
                if (associatedTarget.uuid == animationTarget) {
                    var animator = associatedTarget.gameObject.GetComponent<Animator>();

                    if (animator != null && animationName != null) {
                        animator.ResetTrigger(animationName);
                        animator.SetTrigger(animationName);

                        // Solution simple: StateMachineBehaviour qui notifie le contexte;
                        await WaitUntilAnimationEnds();
                    }
                }
            }

            if (!graphExecutor.canceling)
                await graphExecutor.runtimeGraph.executors[outputUuidTrigger].Execute(graphExecutor);
        }
        
        private async Task WaitUntilAnimationEnds(int sleep = 50) {
            // Attente passive (évite le freeze)
            while (!ended) {
                await Task.Delay(sleep);
            }
        }

        public void NotifyAnimationEnded() {
            ended = true;
        }
    }
}