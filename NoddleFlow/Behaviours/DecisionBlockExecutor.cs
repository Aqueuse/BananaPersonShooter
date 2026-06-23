using System.Threading.Tasks;
using UnityEngine;

namespace Behaviours {
    public class DecisionBlockExecutor : AiBlockExecutor {
        public string choiceIndexVariableUuid; 
        
        public override async Task Execute(GraphExecutor graphExecutor) {
            var choiceIndex = graphExecutor.runtimeGraph.intData[choiceIndexVariableUuid];

            if (choiceIndex < 0) {
                Debug.LogError("choixIndex must be >= 0");
                await Task.CompletedTask;
            }
            
            var nextExecutorUuid = graphExecutor.runtimeGraph.branchData[executorUuid][choiceIndex];
            
            if (!graphExecutor.canceling)
                await graphExecutor.runtimeGraph.executors[nextExecutorUuid].Execute(graphExecutor);
        }
    }
}