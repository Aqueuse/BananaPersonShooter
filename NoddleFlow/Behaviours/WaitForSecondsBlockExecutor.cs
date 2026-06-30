using System.Threading.Tasks;

namespace NoddleFlow.Behaviours {
    public class WaitForSecondsBlockExecutor : AiBlockExecutor {
        public string secondsVariableUuid;
        
        public override async Task Execute(GraphExecutor graphExecutor) {
            if (graphExecutor.runtimeGraph.intData.TryGetValue(secondsVariableUuid, out var dataExecutor)) {
                await Task.Delay(dataExecutor * 1000);
            }
            
            if (!graphExecutor.canceling)
                await graphExecutor.runtimeGraph.executors[outputUuidTrigger].Execute(graphExecutor);
        }
    }
}