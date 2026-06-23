using Unity.GraphToolkit.Editor;

namespace Editor.FlowNoddles.variables {
    public class StringVariableNode : Node {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddOutputPort<string>("variable").Build();
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context) {
            context.AddOption<string>(name: "uuid").ShowInInspectorOnly().Build();
            context.AddOption<string>(name: "value").ShowInInspectorOnly().Build();
        }
    }
}