using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviours {
    public class FollowTransformBlockExecutor : AiBlockExecutor {
        public string aiTargetUuid;
        public string aiAgentUuid;
        public string isFollowingBoolUuid;
        
        [HideInInspector] public Transform targetTransform;
        [HideInInspector] public NavMeshAgent navMeshAgent;

        public override async Task Execute(GraphExecutor graphExecutor) {
            if (aiTargetUuid == null) {
                Debug.LogError("Target is not defined");
                return;
            }
            
            var associatedTargets = FindObjectsByType<AiTarget>(FindObjectsSortMode.None);
            
            if (associatedTargets.Length == 0) {
                Debug.LogError("AITarget not found on scene. Please add one as a component of your target GameObject");
                return;
            }
            
            foreach (var associatedTarget in associatedTargets) {
                if (aiTargetUuid == associatedTarget.uuid) {
                    targetTransform = associatedTarget.transform;
                }

                if (aiAgentUuid == associatedTarget.uuid) {
                    navMeshAgent = associatedTarget.GetComponent<NavMeshAgent>();
                }
            }
            
            if (targetTransform == null) {
                Debug.LogError("target not found on the scene. Have you forget to associate an AITarget with the uuid "+aiTargetUuid+" ?");
                return;
            }

            if (navMeshAgent == null) {
                Debug.LogError("target not found on the scene. Have you forget to associate an AITarget with the uuid "+aiAgentUuid+" ?");
                return;
            }
            
            while (graphExecutor.runtimeGraph.boolData[isFollowingBoolUuid]) {
                await Task.Delay(100); // attend 0.1s avant de re-check
                if (graphExecutor == null) return;
                navMeshAgent.SetDestination(targetTransform.position);
            }
            
            if (!graphExecutor.canceling)
                await graphExecutor.runtimeGraph.executors[outputUuidTrigger].Execute(graphExecutor);
        }
    }
}