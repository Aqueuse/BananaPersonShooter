using System.Threading.Tasks;
using UnityEngine;

namespace Behaviours {
    public class EndBlockExecutor : AiBlockExecutor {
        public string destroyYourselfUuid;
        public string isLoopingUuid;
        
        public override async Task Execute(GraphExecutor graphExecutor) {
            if (graphExecutor.runtimeGraph.boolData[destroyYourselfUuid]) {
                Destroy(graphExecutor.gameObject);
            }

            else {
                if (graphExecutor.runtimeGraph.boolData[isLoopingUuid]) {
                    // search for the start node to loop
                    foreach (var aiBlockExecutor in graphExecutor.runtimeGraph.executors.Values) {
                        if (aiBlockExecutor.GetType() == typeof(StartBlockExecutor)) {
                            if (!graphExecutor.canceling)
                                await graphExecutor.runtimeGraph.executors[aiBlockExecutor.executorUuid].Execute(graphExecutor);
                        }
                    }
                }

                else {
                    graphExecutor.CancelTask();
                }
            }
        }
    }
}