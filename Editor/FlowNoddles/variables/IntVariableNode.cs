using Unity.GraphToolkit.Editor;

namespace Editor.FlowNoddles.variables {
    public class IntVariableNode : Node {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddOutputPort<int>("variable").Build();
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context) {
            context.AddOption<string>(name: "uuid").ShowInInspectorOnly().Build();
            context.AddOption<int>(name: "value").ShowInInspectorOnly().Build();
        }
    }
}