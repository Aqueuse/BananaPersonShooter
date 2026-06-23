using Unity.GraphToolkit.Editor;

namespace Editor.FlowNoddles.variables {
    public class BoolVariableNode : Node {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddOutputPort<bool>("variable").Build();
        }
        
        protected override void OnDefineOptions(IOptionDefinitionContext context) {
            context.AddOption<string>(name: "uuid").ShowInInspectorOnly().Build();
            context.AddOption<bool>(name: "value").ShowInInspectorOnly().Build();
        }
    }
}