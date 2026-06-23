using System.Threading.Tasks;
using UnityEngine;

namespace Behaviours {
    public class AiBlockExecutor : ScriptableObject {
        public string executorUuid;
        public string outputUuidTrigger;
        
        public virtual async Task Execute(GraphExecutor graphExecutor) {
            await Task.Yield();
        }
    }
}