using System.Threading.Tasks;
using UnityEngine;

namespace NoddleFlow.Behaviours {
    public class DebugBlockExecutor : AiBlockExecutor {
        public string messageVariableUuid;

        public override async Task Execute(GraphExecutor graphExecutor) {
            Debug.Log($"[Debug] {graphExecutor.runtimeGraph.stringData[messageVariableUuid]}");
            
            if (!graphExecutor.canceling)
                await graphExecutor.runtimeGraph.executors[outputUuidTrigger].Execute(graphExecutor);
        }
    }
}