using System.Threading.Tasks;
using UnityEngine;

namespace NoddleFlow.Behaviours {
    public class DestroyTargetBlockExecutor : AiBlockExecutor {
        public string targetUuid;

        public override async Task Execute(GraphExecutor graphExecutor) {
            var associatedTargets = FindObjectsByType<AiTarget>(FindObjectsSortMode.None);

            if (associatedTargets.Length == 0) {
                Debug.LogError("AITarget not found on scene. Please add one as a component of your target GameObject");
                return;
            }

            foreach (var associatedTarget in associatedTargets) {
                if (targetUuid != associatedTarget.uuid) continue;
                
                if (associatedTarget.GetComponentInChildren<AiRuntimeGraph>()) {
                    Debug.LogError("Don't use Destroy Target on yourself, use EndNode with Destroy=true instead");
                }

                else {
                    Destroy(associatedTarget.gameObject);
                }
            }
            
            if (!graphExecutor.canceling)
                await graphExecutor.runtimeGraph.executors[outputUuidTrigger].Execute(graphExecutor);
        }
    }
}