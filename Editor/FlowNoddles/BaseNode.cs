using System;
using Behaviours;
using Unity.GraphToolkit.Editor;

namespace Editor.FlowNoddles {
    public class BaseNode : Node {
        public readonly string uuid = Guid.NewGuid().ToString();
        
        protected override void OnDefineOptions(IOptionDefinitionContext context) {
            context.AddOption<string>(name: "uuid").WithDefaultValue(uuid).ShowInInspectorOnly();
        }

        public virtual AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            return null;
        }
    }
}