using System.Threading.Tasks;

namespace Behaviours {
    public class StartBlockExecutor : AiBlockExecutor {
        public override async Task Execute(GraphExecutor graphExecutor) {
            await Task.Delay(10);
            if (!graphExecutor.canceling)
                await graphExecutor.runtimeGraph.executors[outputUuidTrigger].Execute(graphExecutor);
        }
   }
}